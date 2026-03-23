using UnityEngine;
using System;
using System.Collections.Generic;

public static class UrlHelper
{
    public static Dictionary<string, string> GetQueryParams(string url)
    {
        var uri = new Uri(url);
        var query = uri.Query.TrimStart('?');
        var pairs = query.Split('&', StringSplitOptions.RemoveEmptyEntries);
        var result = new Dictionary<string, string>();

        foreach (var pair in pairs)
        {
            var parts = pair.Split('=');
            if (parts.Length == 2)
                result[parts[0]] = Uri.UnescapeDataString(parts[1]);
        }

        return result;
    }
}
