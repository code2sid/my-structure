export function createEnvironmentConfig(baseUri: string, isProd: boolean) {
  return {
    configUrl: `${baseUri}api/config`,
    calculatorUrl: `${baseUri}api/calculator`,
    summaryUrl: `${baseUri}api/summary`,
    requestUrl: `${baseUri}api/request`,
    statusUrl: `${baseUri}api/status`,
    countriesUrl: `${baseUri}api/countries`,
    currenciesUrl: `${baseUri}api/currencies`,
    userUrl: `${baseUri}api/user`,
    groupingUrl: `${baseUri}api/grouping`,
    eventTypesUrl: `${baseUri}api/event-types`,
    production: isProd
  };
};
