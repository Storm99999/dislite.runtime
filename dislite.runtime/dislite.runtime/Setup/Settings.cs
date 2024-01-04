using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dislite.runtime.Setup
{
    internal class Settings
    {
        public static class Default
        {
            public static string token = "";
        }
        public static void SetToken(string tk) => Default.token = tk;
    }
}
