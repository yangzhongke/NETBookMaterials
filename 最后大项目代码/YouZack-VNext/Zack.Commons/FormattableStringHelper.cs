using System;
using System.Linq;

namespace Zack.Commons
{
    public static class FormattableStringHelper
    {
        public static string BuildUrl(FormattableString urlFormat)
        {
            var invariantParameters = urlFormat.GetArguments()
                .Select(a => FormattableString.Invariant($"{a}"));
            object[] escapedParameters = invariantParameters
              .Select(s => (object)Uri.EscapeDataString(s)).ToArray();
            return string.Format(urlFormat.Format, escapedParameters);
        }
    }
}
