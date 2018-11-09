using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    internal static class Helpers
    {
        public static string AppendCssClasses(params string[] cssClasses)
        {
            var builder = new StringBuilder();
            foreach(var cl in cssClasses)
            {
                if (string.IsNullOrEmpty(cl))
                {
                    continue;
                }

                builder.Append($"{cl} ");
            }

            return builder.ToString();
        }
        
    }
}
