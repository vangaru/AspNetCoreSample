using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AspNetCoreSample.RouteConstraints;

public sealed class UkPostcodeRouteConstraint : IRouteConstraint
{
    public const string RouteConstraintName = "ukpostcode";

    private static readonly Regex UkPostcodeRegex;


    static UkPostcodeRouteConstraint()
    {
        UkPostcodeRegex = new Regex(@"^[A-Z]{1,2}[0-9R][0-9A-Z]?", RegexOptions.CultureInvariant);
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

        var ukZip = Convert.ToString(routeValue);

        return ukZip is not null && UkPostcodeRegex.IsMatch(ukZip);
    }
}