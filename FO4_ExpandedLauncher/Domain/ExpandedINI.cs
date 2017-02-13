using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FO4_ExpandedLauncher.Domain
{
    class ExpandedINI
    {
        private static string LauncherINI = "ExpandedLauncher.ini";

        public static void CreateINI()
        {
            if(File.Exists(LauncherINI) == false)
            {
                IniFile ini = new IniFile(LauncherINI);
                ini.Write("CustomExecutable","false", "ExpandedLauncher");
                ini.Write("CustomExecutableName", "f4se_loader.exe", "ExpandedLauncher");
                ini.Write("AlternateBackground", "false", "ExpandedLauncher");
            }
        }

        public static string GetCustomEXE()
        {
            IniFile ini = new IniFile(LauncherINI);
            if (ini.Read("CustomExecutable", "ExpandedLauncher") == "true")
            {
                Console.WriteLine("Custom Executable Loaded!");
                return ini.Read("CustomExecutableName", "ExpandedLauncher");
            }
            return "NULL";
        }

        public static bool GetAlternativeBG()
        {
            IniFile ini = new IniFile(LauncherINI);
            if (ini.Read("AlternateBackground", "ExpandedLauncher") == "true")
            {
                return true;
            }
            return false;
        }


        public static void WriteOptions(bool foo, string bar)
        {
            IniFile ini = new IniFile(LauncherINI);
            ini.Write("CustomExecutable", foo.ToString(), "ExpandedLauncher");
            ini.Write("CustomExecutableName", bar, "ExpandedLauncher");
        }

        public static Tuple<bool, string> GetOptions()
        {
            IniFile ini = new IniFile(LauncherINI);
            bool foo = Convert.ToBoolean(ini.Read("CustomExecutable", "ExpandedLauncher"));
            string bar = ini.Read("CustomExecutableName", "ExpandedLauncher");
            return Tuple.Create(foo, bar);
        }
    }
}
