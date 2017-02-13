using FO4_ExpandedLauncher.Domain;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using static FO4_ExpandedLauncher.Domain.ExpandedINI;

namespace FO4_ExpandedLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string Executable = "Fallout4.exe";

        //Catch Commandline arguments
        private static List<string> arguments = new List<string>(Environment.GetCommandLineArgs());

        //Current xInput controller
        private Controller xController;

        //Controller input listener thread
        private BackgroundWorker xListener = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
            CreateINI();

            if(GetCustomEXE() != "NULL")
            {
                Executable = GetCustomEXE();
            }

            if(GetAlternativeBG())
            {
                System.Windows.Media.Imaging.BitmapImage altBG = new System.Windows.Media.Imaging.BitmapImage();
                altBG.BeginInit();
                altBG.UriSource = new Uri("../Backgrounds/powerarmor.png", UriKind.Relative);
                altBG.EndInit();
                Background.Source = altBG;
            }

            //Menu Item click handlers
            PlayButton.MouseLeftButtonDown += new MouseButtonEventHandler(PlayButton_Click);
            OptionsButton.MouseLeftButtonDown += new MouseButtonEventHandler(OptionsButton_Click);
            ExitButton.MouseLeftButtonDown += new MouseButtonEventHandler(ExitButton_Click);

            //Left Mouse button handler
            MouseLeftButtonDown += new MouseButtonEventHandler(MainWindow_MouseDown);

            //xInput Initialization
            getCurrentController();

            //xInput listener
            xListener.DoWork += xListener_DoWork;

            //Skip launcher, if "-NoLauncher" arg is set
            if (CMDArguments.SkipLauncher() == true)
            {
                LaunchGame.Launch(Executable);

                //Terminate Launcher
                Application.Current.Shutdown();
            }
        }

        //Gets the currently connected controller
        public void getCurrentController()
        {
            //currentXInputDevice.cs
            currentXInputDevice getDevice = new currentXInputDevice();
            xController = getDevice.getActiveController();

            if(xController != null)
            {
                Console.WriteLine("Starting xListener thread");
                xListener.RunWorkerAsync();
            }
            else
            {
                Console.WriteLine("No controller detected");
            }
        }

        private void xListener_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("xListener Started");
            currentXInputDevice xDevice = new currentXInputDevice();

            while (xController.IsConnected)
            {
                var buttons = xController.GetState().Gamepad.Buttons;
                var pressedButton = xDevice.getPressedButton(buttons).ToString();

                //Check for buttons here!
                if (pressedButton != "None")
                {
                    if(pressedButton == "DPadDown")
                    {
                        Dispatcher.Invoke(new Action(()=> MoveFocus(0)));
                    }
                    else if(pressedButton == "DPadUp")
                    {
                        Dispatcher.Invoke(new Action(() => MoveFocus(1)));
                    }
                    else if(pressedButton == "A")
                    {
                        Dispatcher.Invoke(new Action(() => A_Press()));
                    }
                    else if(pressedButton == "B")
                    {
                        Dispatcher.Invoke(new Action(() => HideOptions()));
                    }
                }

                Thread.Sleep(100);
            }

            Console.WriteLine("Disposing of xListener thread!");
        }

        //A Button on controller
        private void A_Press()
        {
            if(PlayButton.IsFocused)
            {
                PlayButton_Click(null, null);
            }
            else if (OptionsButton.IsFocused)
            {
                OptionsButton_Click(null, null);
            }
            else if (ExitButton.IsFocused)
            {
                ExitButton_Click(null, null);
            }
        }

        //Move focus where needed, a crude way of doing it thou
        //int indicates direction
        private void MoveFocus(int e)
        {
            //0 = down
            if(e == 0)
            {
                if(PlayButton.IsFocused)
                {
                    OptionsButton.Focus();
                }
                else if(OptionsButton.IsFocused)
                {
                    ExitButton.Focus();
                }
                else if(ExitButton.IsFocused)
                {
                    PlayButton.Focus();
                }
                else
                {
                    PlayButton.Focus();
                }
            }

            //1 = up
            if(e == 1)
            {
                if (PlayButton.IsFocused)
                {
                    ExitButton.Focus();
                }
                else if (OptionsButton.IsFocused)
                {
                    PlayButton.Focus();
                }
                else if (ExitButton.IsFocused)
                {
                    OptionsButton.Focus();
                }
                else
                {
                    PlayButton.Focus();
                }
            }
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                Dispatcher.Invoke(new Action(() => DragMove()));
        }

        private void PlayButton_Click(object sender, MouseButtonEventArgs e)
        {
            LaunchGame.Launch(Executable);

            //Terminate Launcher
            Application.Current.Shutdown();
        }

        private void OptionsButton_Click(object sender, MouseButtonEventArgs e)
        {
            //Show options page
            ((Options)PageFrame.Content).RefreshOptions();
            PageFrame.Visibility = Visibility.Visible;
        }

        private void ExitButton_Click(object sender, MouseButtonEventArgs e)
        {
            //Terminate Launcher
            Application.Current.Shutdown();
        }

        public void HideOptions()
        {
            //Hide options page
            PageFrame.Visibility = Visibility.Collapsed;
        }

    }
}
