using System;
using System.Collections.Generic;
using System.Web;

namespace AspNetCoreSample.Foundation.WeatherApi.UriUtils;

public static class UriExtensions
{
    public static Uri AddQueryParameters(this Uri uri, IReadOnlyDictionary<string, string> queryParameters)
    {
        var uriBuilder = new UriBuilder(uri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        foreach (var queryParameter in queryParameters)
        {
            query[queryParameter.Key] = queryParameter.Value;
        }

        uriBuilder.Query = query.ToString() ?? String.Empty;

        return uriBuilder.Uri;
    }
    
    public static Uri AddQueryParameter(this Uri uri, string paramName, string paramValue)
    {
        return uri.AddQueryParameters(new Dictionary<string, string> { { paramName, paramValue } });
    }
}