using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;

namespace taphelper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!envCheck())
            {
                return;
            }

            // 获取屏幕像素
            var cur = AdbUtils.GetCur();
            Console.WriteLine("屏幕像素：" + cur);

            // 获取事件空间大小
            var eventCur = AdbUtils.GetEventWidthAndHeight();
            Console.WriteLine("事件空间：" + eventCur);

            // 获取点击坐标点
            Console.WriteLine("请点击你想自动点击的位置:");
            var x = 0;
            var y = 0;
            using (Process getPintProcess = CmdUtils.createCmd())
            {

                getPintProcess.StandardInput.WriteLine("adb shell getevent");
                String line = null;

                var num = 0;
                while ((line = getPintProcess.StandardOutput.ReadLine()) != null && num < 3)
                {
                    // /dev/input/event0: 0003 0035 00000341
                    // /dev/input/event0: 0003 0036 000008ec
                    if (line.Contains("0035"))
                    {
                        var items = line.Split(' ');
                        x = int.Parse(items[items.Length - 1], System.Globalization.NumberStyles.HexNumber);
                        num++;
                        Console.WriteLine("第{0}次点击", num);
                    }
                    else if (line.Contains("0036"))
                    {
                        var items = line.Split(' ');
                        y = int.Parse(items[items.Length - 1], System.Globalization.NumberStyles.HexNumber);
                    }
                }
            }

            var curX = x * cur.Item1 / eventCur.Item1;
            var curY = y * cur.Item2 / eventCur.Item2;
            Console.WriteLine("最终点击像素点坐标，x:{0},y:{0}", curX, curY);



            Console.WriteLine("请输入点击的间隔时间（毫秒）：");
            var intervel = Console.ReadLine();
            var intervelMillions = int.Parse(intervel);

            Console.WriteLine("开始执行点击,如若需要结束程序前点击关闭或者输入q");
            using (var p = CmdUtils.createCmd())
            {
                AdbCronClicker.startClick(curX, curY, p, intervelMillions);

                // 点击点
                while (Console.ReadKey().Key != ConsoleKey.Q)
                {
                    // continue
                }
            }

            System.Environment.Exit(1);
        }


        /// <summary>
        /// 环境检查
        /// </summary>
        /// <returns></returns>
        private static bool envCheck()
        {
            Console.WriteLine("环境检查开始......");

            var deviceNum = AdbUtils.GetDeviceNum();
            
            if (deviceNum == 0)
            {
                Console.WriteLine("当前没有任何android设备连接");
                return false;
            }
            if (deviceNum > 1)
            {
                Console.WriteLine("当前有多余android设备连接，请断开多余设备，保留一台连接");
                return false;
            }

            Console.WriteLine("设备已就绪......");

            return true;
        }
    }
}
