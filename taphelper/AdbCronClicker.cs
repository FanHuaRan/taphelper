using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace taphelper
{
    public class AdbCronClicker
    {
        private readonly int clickX;

        private readonly int clickY;

        private int clickCounter = 0;

        private readonly Process clickAdbProcess;

        public AdbCronClicker(int clickX, int clickY, Process clickAdbProcess)
        {
            this.clickX = clickX;
            this.clickY = clickY;
            this.clickAdbProcess = clickAdbProcess;
        }

        public static void startClick(int clickX, int clickY, Process clickAdbProcess, int intervelMillions)
        {
            var cronClicker = new AdbCronClicker(clickX, clickY, clickAdbProcess);
            var timer = new Timer();
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Interval = intervelMillions;
            timer.Elapsed += new ElapsedEventHandler(cronClicker.sendClickComand);

            timer.Start();


        }

        private void sendClickComand(object source, ElapsedEventArgs e)
        {

            clickAdbProcess.StandardInput.WriteLine(String.Format("adb shell input tap {0} {1}", clickX, clickY));
            Console.WriteLine(String.Format("success click android, x:{0}, y:{1}, counter:{2}", clickX, clickY, ++clickCounter));
        }
    }
}
