using Common2.Interfaces;
using System.Configuration;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Deployment.Application;

namespace Web.Service
{
    public static class EnvironmentSettings
    {
        private static string EnvType = (string)null;
        private static string EnvTypeEx = (string)null;
        public const string configFilePath = "\\\\aqr\\shares\\FS012\\Config\\app.config";
        private static Dictionary<string, string> EnvServers;

        static EnvironmentSettings()
        {
            EnvironmentSettings.GetEnvironmentType();
            EnvironmentSettings.LoadEnvironmentServersSettings();
        }

        public static EnvironmentSettings.Environments EnvironmetType
        {
            get
            {
                return EnvironmentSettings._EnvironmetType;
            }
            set
            {
                EnvironmentSettings._EnvironmetType = value;
            }
        }

        public static string ApplicationDeploymentSource { get; private set; }

        public static string ApplicationDeploymentVersion { get; private set; }

        public static EnvironmentSettings.Environments _EnvironmetType { get; set; } = EnvironmentSettings.Environments.UNKNOWN;

        public static string GetEnvironmentType()
        {
            if (EnvironmentSettings.EnvType == null)
            {
                string typeUsingNewLogic = EnvironmentSettings.GetEnvironmentTypeUsingNewLogic();
                if (typeUsingNewLogic != null)
                {
                    EnvironmentSettings.EnvType = typeUsingNewLogic == null ? "" : typeUsingNewLogic;
                    EnvironmentSettings.Environments? environmentFromString = EnvironmentSettings.GetEnvironmentFromString(typeUsingNewLogic);
                    if (environmentFromString.HasValue)
                        EnvironmentSettings.EnvironmetType = environmentFromString.Value;
                }
            }
            return EnvironmentSettings.EnvType;
        }

        public static string GetEnvironmentTypeEx()
        {
            if (EnvironmentSettings.EnvTypeEx == null)
            {
                string envType;
                if (EnvironmentSettings.ShouldUseNewEnvironmentSettingsLogic())
                {
                    envType = EnvironmentSettings.GetEnvironmentTypeUsingNewLogic();
                }
                else
                {
                    RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\AQR");
                    envType = registryKey == null ? ConfigurationManager.AppSettings["Environment"] : (string)registryKey.GetValue("Environment");
                }
                if (envType != null)
                {
                    EnvironmentSettings.EnvTypeEx = "_" + envType;
                    EnvironmentSettings.Environments? environmentFromString = EnvironmentSettings.GetEnvironmentFromString(envType);
                    if (environmentFromString.HasValue)
                        EnvironmentSettings.EnvironmetType = environmentFromString.Value;
                }
                else
                    EnvironmentSettings.EnvTypeEx = string.Empty;
            }
            return EnvironmentSettings.EnvTypeEx;
        }

        public static bool ShouldRunInSandbox(string service)
        {
            try
            {
                RegistryKey aqrKey = EnvironmentSettings.GetAqrKey();
                if (aqrKey != null)
                    return (uint)Convert.ToInt32(aqrKey.GetValue("Sandbox" + service, (object)0)) > 0U;
            }
            catch (Exception ex)
            {
                //ILoggingService.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Error((object)"Error checking service sandboxing registry setting", ex);
            }
            return false;
        }

        public static bool ShouldRunInProcess(string service)
        {
            try
            {
                RegistryKey aqrKey = EnvironmentSettings.GetAqrKey();
                if (aqrKey != null)
                    return Convert.ToInt32(aqrKey.GetValue("Sandbox" + service, (object)0)) == 2;
            }
            catch (Exception ex)
            {
                //LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Error((object)"Error checking service sandboxing registry setting", ex);
            }
            return false;
        }

        public static bool ShouldRunInWCFTestHostProcess(string service)
        {
            try
            {
                RegistryKey aqrKey = EnvironmentSettings.GetAqrKey();
                if (aqrKey != null)
                    return Convert.ToInt32(aqrKey.GetValue("Sandbox" + service, (object)0)) == 3;
            }
            catch (Exception ex)
            {
                //LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Error((object)"Error checking service sandboxing registry setting", ex);
            }
            return false;
        }

        private static RegistryKey GetAqrKey()
        {
            return Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\AQR") ?? Registry.LocalMachine.OpenSubKey("Software\\AQR");
        }

        public static string GetEnvironmentTypeUsingNewLogic()
        {
            string str = EnvironmentSettings.GetEnvironmentFromDeployment();
            if (str == null)
            {
                str = EnvironmentSettings.GetEnvironmentFromStartupDirectory();
                if (str == null)
                {
                    str = ConfigurationManager.AppSettings["Environment"];
                    if (str == null)
                    {
                        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\AQR");
                        if (registryKey != null)
                            str = (string)registryKey.GetValue("Environment");
                    }
                }
            }
            return str;
        }

        public static bool ShouldUseNewEnvironmentSettingsLogic()
        {
            if (Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal) || Process.GetCurrentProcess().ProcessName.StartsWith("xdesproc", StringComparison.Ordinal))
                return true;
            return string.Equals(ConfigurationManager.AppSettings[nameof(ShouldUseNewEnvironmentSettingsLogic)], "true", StringComparison.InvariantCultureIgnoreCase);
        }

        public static EnvironmentSettings.Environments? GetEnvironmentFromString(string envType)
        {
            if (envType.ToUpper() == "TEST")
                return new EnvironmentSettings.Environments?(EnvironmentSettings.Environments.QA);
            EnvironmentSettings.Environments result;
            if (Enum.TryParse<EnvironmentSettings.Environments>(envType, true, out result))
                return new EnvironmentSettings.Environments?(result);
            return new EnvironmentSettings.Environments?();
        }

        public static string GetEnvironmentFromDeployment()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                EnvironmentSettings.ApplicationDeploymentVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                if (AppDomain.CurrentDomain.SetupInformation != null && AppDomain.CurrentDomain.SetupInformation.ActivationArguments != null)
                {
                    if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null)
                    {
                        if ((uint)AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Length > 0U)
                        {
                            Uri uri = new Uri(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0]);
                            Debug.WriteLine("ApplicationDeploymentUri " + uri.ToString());
                            EnvironmentSettings.ApplicationDeploymentSource = uri.Host;
                            string lower = uri.Host.ToLower();
                            if (lower == "aqrweb")
                                return "PROD";
                            if (lower == "st0sweb12")
                                return "STAGING";
                            return lower == "aqrtest" ? "QA" : "QA";
                        }
                    }
                    else
                    {
                        EnvironmentSettings.ApplicationDeploymentSource = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ApplicationIdentity.CodeBase;
                        Debug.WriteLine("ApplicationDeploymentSource " + EnvironmentSettings.ApplicationDeploymentSource);
                        if (EnvironmentSettings.ApplicationDeploymentSource.ToLower().IndexOf("dev/application/tradeworkflow/bin/publish") > 0)
                            return "DEV";
                        if (EnvironmentSettings.ApplicationDeploymentSource.ToLower().IndexOf("qa/application/tradeworkflow/bin/publish") > 0)
                            return "QA";
                        if (EnvironmentSettings.ApplicationDeploymentSource.ToLower().IndexOf("testframework/application/tradeworkflow/bin/publish") > 0)
                            return "TESTFRAMEWORK";
                        if (EnvironmentSettings.ApplicationDeploymentSource.ToLower().IndexOf("http://aqrweb") >= 0)
                            return "PROD";
                        if (EnvironmentSettings.ApplicationDeploymentSource.ToLower().IndexOf("http://st0sweb12") >= 0)
                            return "STAGING";
                        if (EnvironmentSettings.ApplicationDeploymentSource.ToLower().StartsWith("file://"))
                            EnvironmentSettings.ApplicationDeploymentSource = EnvironmentSettings.ApplicationDeploymentSource.ToLower().Substring("file:".Length).Replace('/', '\\');
                        Debug.WriteLine("ApplicationDeploymentSource Adjusted " + EnvironmentSettings.ApplicationDeploymentSource);
                        string startupDirectory = EnvironmentSettings.GetEnvironmentFromStartupDirectory(EnvironmentSettings.ApplicationDeploymentSource);
                        if (startupDirectory != null)
                            return startupDirectory;
                    }
                }
                return "QA";
            }
            EnvironmentSettings.ApplicationDeploymentSource = Environment.MachineName;
            return (string)null;
        }

        public static string GetEnvironmentFromStartupDirectory()
        {
            return EnvironmentSettings.GetEnvironmentFromStartupDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }

        public static string GetEnvironmentFromStartupDirectory(string baseDirectory)
        {
            if (baseDirectory != null)
            {
                Match match = Regex.Match(baseDirectory, "^\\\\\\\\((aqr)|(aqrcapital.com))\\\\shares\\\\FS[0-9A-F]+\\\\(?<env>[a-z]+)\\\\", RegexOptions.IgnoreCase);
                if (match.Success)
                    return match.Groups["env"].Value.ToUpper();
                foreach (KeyValuePair<string, List<string>> keyValuePair in (IEnumerable<KeyValuePair<string, List<string>>>)new Dictionary<string, List<string>>()
        {
          {
            "PROD",
            new List<string>() { "PRD", "PROD" }
          },
          {
            "STAGING",
            new List<string>() { "STG", "STAGING" }
          },
          {
            "QA",
            new List<string>() { "QA1", "QA2", "QA3", "QA" }
          },
          {
            "DEV",
            new List<string>() { "DEV" }
          }
        })
                {
                    if (keyValuePair.Value.Any<string>((Func<string, bool>)(expectedFolderName => baseDirectory.Contains(string.Format("\\{0}\\", (object)expectedFolderName)))))
                        return keyValuePair.Key;
                }
            }
            return (string)null;
        }

        public static string GetWebServerForEnvironment(EnvironmentSettings.Environments env)
        {
            return EnvironmentSettings.EnvServers.GetOrNull<string, string>(env.ToString() + "Server") ?? "aqrweb";
        }

        public static string GetServiceURIForEnvironment(EnvironmentSettings.Environments env)
        {
            return EnvironmentSettings.EnvServers.GetOrNull<string, string>(env.ToString() + "ServiceURI");
        }

        public static string GetSetting(string key)
        {
            return EnvironmentSettings.EnvServers.GetOrNull<string, string>(key);
        }

        public static string GetDefaultConfigForEnvironment(EnvironmentSettings.Environments env)
        {
            string key = ((int)env).ToString() + "DefaultConfig";
            string orNull = EnvironmentSettings.EnvServers.GetOrNull<string, string>(key);
            if (orNull != null)
                return orNull;
            throw new Exception(string.Format("No '{0}' defined in {1}", (object)key, (object)EnvironmentSettings.GetConfigFilePath()));
        }

        public static string AppSettings(string name)
        {
            return ConfigurationManager.AppSettings[name + EnvironmentSettings.GetEnvironmentTypeEx()];
        }

        public static string GetWebServiceRedirectionServer()
        {
            return EnvironmentSettings.AppSettings("WebServerHost");
        }

        public static string GetEnvironmentConnectionStringName(string connectionStringName)
        {
            return connectionStringName + EnvironmentSettings.GetEnvironmentType();
        }

        public static string GetEnvironmentConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[EnvironmentSettings.GetEnvironmentConnectionStringName(connectionStringName)].ConnectionString;
        }

        public static string GetWebServer()
        {
            EnvironmentSettings.GetEnvironmentType();
            return EnvironmentSettings.GetWebServerForEnvironment(EnvironmentSettings.EnvironmetType);
        }

        private static void LoadEnvironmentServersSettings()
        {
            System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap()
            {
                ExeConfigFilename = EnvironmentSettings.GetConfigFilePath()
            }, ConfigurationUserLevel.None);
            if (configuration != null)
            {
                AppSettingsSection appSettings = configuration.AppSettings;
                if (appSettings != null)
                    EnvironmentSettings.EnvServers = appSettings.Settings.OfType<KeyValueConfigurationElement>().ToDictionary<KeyValueConfigurationElement, string, string>((Func<KeyValueConfigurationElement, string>)(s => s.Key), (Func<KeyValueConfigurationElement, string>)(s => s.Value));
            }
            if (EnvironmentSettings.EnvServers != null)
                return;
            EnvironmentSettings.EnvServers = new Dictionary<string, string>();
        }

        internal static T GetOrNull<K, T>(this IDictionary<K, T> dict, K key) where T : class
        {
            T obj;
            dict.TryGetValue(key, out obj);
            return obj;
        }

        internal static string GetConfigFilePath()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\AQR");
            if (registryKey != null)
            {
                string str = (string)registryKey.GetValue("ConfigFilePath");
                if (!string.IsNullOrEmpty(str))
                    return str;
            }
            return "\\\\aqr\\shares\\FS012\\Config\\app.config";
        }

        public enum Environments
        {
            DEV = 0,
            QA = 1,
            PROD = 2,
            UAT = 4,
            STAGING = 8,
            UNKNOWN = 10, // 0x0000000A
            TESTFRAMEWORK = 16, // 0x00000010
            REGRESSIONA = 32, // 0x00000020
            REGRESSIONB = 64, // 0x00000040
        }
    }
}
