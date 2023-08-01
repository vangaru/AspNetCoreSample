using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AspNetCoreSample.RouteConstraints;

public sealed class UsZipcodeRouteConstraint : IRouteConstraint
{
    public const string RouteConstraintName = "uszipcode";

    private static readonly Regex UsZipCodeRegex;


    static UsZipcodeRouteConstraint()
    {
        UsZipCodeRegex = new Regex(@"^[0-9]{5}", RegexOptions.CultureInvariant);
    }


    public bool Match(
        HttpContext httpContext,
        IRouter route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var routeValue))
        {
            return false;
        }

        var usZip = Convert.ToString(routeValue);

        return usZip is not null && UsZipCodeRegex.IsMatch(usZip);
    }
}