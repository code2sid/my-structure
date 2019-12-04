using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Service
{
    public class HttpPaasAuthToken
    {
        /// <summary>
        /// Access Token
        /// </summary>
        public string AccessToken { get; }
        /// <summary>
        /// Time when Token Expires
        /// </summary>
        private DateTime ExpiresAt { get; }
        /// <summary>
        /// Type of the token
        /// </summary>
        public string TokenType { get; }

        /// <summary>
        /// Constructor parses string with Token received from PingFed 
        /// </summary>
        /// <param name="tokenString">token string received from PingFed</param>
        public HttpPaasAuthToken(string tokenString)
        {
            var tokens = tokenString.Split('&');

            var accessToken = tokens[0].Split('=');
            AccessToken = accessToken[1];

            var tokenType = tokens[1].Split('=');
            TokenType = tokenType[1];

            var expiresIn = tokens[2].Split('=');
            ExpiresAt = DateTime.Now.AddSeconds(long.Parse(expiresIn[1]));
        }

        /// <summary>
        /// HttpPaasAuthToken constructor
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="expiresIn">Expires in (seconds)</param>
        /// <param name="tokenType">Type of the token</param>
        public HttpPaasAuthToken(string accessToken, long expiresIn, string tokenType)
        {
            AccessToken = accessToken;
            ExpiresAt = DateTime.Now.AddSeconds(expiresIn);
            TokenType = tokenType;
        }

        /// <summary>
        /// Check if token expired already 
        /// (curent system time used for check)
        /// </summary>
        /// <returns>true if expired</returns>
        public bool Expired()
        {
            return Expired(DateTime.Now);
        }

        /// <summary>
        /// Check if token expired already 
        /// (curent system time used for check and tolerance)
        /// </summary>
        /// <param name="tolerance">tolerance in seconds</param>
        /// <returns>true if expired</returns>
        public bool Expired(long tolerance)
        {
            return Expired(DateTime.Now.AddSeconds(-tolerance));
        }

        /// <summary>
        /// Check if token expired by time
        /// (tolerance applies)
        /// </summary>
        /// <param name="dateTime">Time for check</param>
        /// <param name="tolerance">tolerance in seconds</param>
        /// <returns>true if expired</returns>
        public bool Expired(DateTime dateTime, long tolerance)
        {
            return ExpiresAt.CompareTo(dateTime.AddSeconds(-tolerance)) <= 0;
        }

        /// <summary>
        /// Check if token expired by time
        /// (tolerance applies)
        /// </summary>
        /// <param name="dateTime">Time for check</param>
        /// <returns>true if expired</returns>
        private bool Expired(DateTime dateTime)
        {
            return ExpiresAt.CompareTo(dateTime) <= 0;
        }
    }
}
