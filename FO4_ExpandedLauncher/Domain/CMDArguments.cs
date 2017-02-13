using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FO4_ExpandedLauncher.Domain
{
    public static class CMDArguments
    {
        private static List<string> arguments = new List<string>(Environment.GetCommandLineArgs());

        public static bool SkipLauncher()
        {
            if(arguments.Contains("-NoLauncher"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
