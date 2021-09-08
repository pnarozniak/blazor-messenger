using System.Collections.Generic;
using System.Linq;

namespace messanger.Client.Models
{
    public class QueryParams
    {
        private readonly Dictionary<string, object> _keyValuePairs;

        public QueryParams()
        {
            _keyValuePairs = new Dictionary<string, object>();
        }

        public void Add(string paramName, object paramValue)
        {
            if (paramValue is null) return;

            _keyValuePairs.Add(paramName, paramValue.ToString());
        }

        public override string ToString()
        {
            var queryString = _keyValuePairs
                .Aggregate(
                    string.Empty,
                    (current, keyValuePair) => current + $"{keyValuePair.Key}={keyValuePair.Value}&");

            if (queryString[^1..] == "&") queryString = queryString.Remove(queryString.Length - 1);

            return queryString;
        }
    }
}