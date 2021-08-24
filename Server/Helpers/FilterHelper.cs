using System.Linq;

namespace messanger.Server.Helpers
{
    public static class FilterHelper
    {
        public static string GetFirstPart(string filter)
        {
            return !filter.Contains(" ") ? filter : filter.Split(" ")[0];
        }

        public static string GetLastPart(string filter)
        {
            return !filter.Contains(" ") ? "" : string.Join("", filter.Split(" ").Skip(1));
        }
    }
}
