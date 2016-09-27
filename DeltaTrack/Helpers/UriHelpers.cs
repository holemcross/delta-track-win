using System;
using System.Collections.Generic;
using System.Text;

namespace DeltaTrack.Helpers
{
    public static class UriHelpers
    {
        public static Uri CreateWithQuery(Uri uri, Dictionary<string, string> paramaters)
        {
            var queryStr = new StringBuilder();

            var queryOpChar = string.IsNullOrWhiteSpace(uri.Query) ? string.Empty : uri.Query.Substring(1) + "&";

            foreach (var param in paramaters)
            {
                queryStr.Append($"{queryOpChar}{param.Key}={param.Value}");
                queryOpChar = "&";
            }

            return new UriBuilder(uri)
            {
                Query = queryStr.ToString()
            }.Uri;
        }
    }
}
