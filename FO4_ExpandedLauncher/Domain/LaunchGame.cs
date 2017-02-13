using System.Diagnostics;
using System.IO;
using System.Windows;

namespace FO4_ExpandedLauncher
{
    public static class LaunchGame
    {
        public static void Launch(string _exe)
        {
            if(File.Exists(_exe))
            {
                //Start Game
                Process GameExe = new Process();
                GameExe.StartInfo.FileName = _exe;
                GameExe.Start();

                //Elevate Process
                GameExe.PriorityClass = ProcessPriorityClass.AboveNormal;
            }
            else
            {
                MessageBox.Show($"{_exe} not found!","Error!");
            }
            
        }
    }
}
