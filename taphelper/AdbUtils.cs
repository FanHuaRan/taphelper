using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace taphelper
{
    /// <summary>
    /// Adb辅助工具类
    /// </summary>
    public class AdbUtils
    {
        /// <summary>
        /// 获取像素大小
        /// </summary>
        /// <returns></returns>
        public static Tuple<int, int> GetCur()
        {
            using (Process process = CmdUtils.createCmd())
            {
                process.StandardInput.WriteLine("adb shell dumpsys window displays ");
                var row = "";

                while ((row = process.StandardOutput.ReadLine()) != null)
                {

                    if (row.Contains("cur="))
                    {
                        var items = row.Split(' ');
                        foreach (var item in items)
                        {
                            if (item.StartsWith("cur"))
                            {
                                var datas = item.Substring(4).Split('x');
                                var width = int.Parse(datas[0]);
                                var height = int.Parse(datas[1]);
                                return new Tuple<int, int>(width, height);
                            }
                        }
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// 获取事件空间大小
        /// </summary>
        /// <returns></returns>
        public static Tuple<int, int> GetEventWidthAndHeight()
        {
            using (Process process = CmdUtils.createCmd())
            {
                process.StandardInput.WriteLine("adb shell getevent -p");
                var row = "";
                var width = 0;
                var height = 0;
                while ((row = process.StandardOutput.ReadLine()) != null)
                {

                    if (row.Contains("0035") || row.Contains("0036"))
                    {

                        var items = row.Split(' ');
                        for (int i = 0, n = items.Length; i < n; i++)
                        {
                            if ("max".Equals(items[i]))
                            {
                                var value = items[i + 1];

                                if (row.Contains("0035"))
                                {
                                    width = int.Parse(value.Substring(0, value.Length - 1));

                                }
                                else
                                {
                                    height = int.Parse(value.Substring(0, value.Length - 1));

                                }
                            }
                        }

                        if (width != 0 && height != 0)
                        {
                            break;
                        }
                    }
                }

                return new Tuple<int, int>(width, height);

            }
        }

        /// <summary>
        /// 获取当前设备的数量
        /// </summary>
        /// <returns></returns>
        public static int GetDeviceNum()
        {
            using (Process process = CmdUtils.createCmd())
            {
                process.StandardInput.WriteLine("adb devices");

                var startCompute = false;
                var deviceNum = 0;

                var row = "";

                while ((row = process.StandardOutput.ReadLine()) != null)
                {
                    if (startCompute)
                    {
                        if (String.IsNullOrWhiteSpace((row)))
                        {
                            break;
                        }

                        deviceNum++;
                        Console.WriteLine(String.Format("设备{0}:{1}", deviceNum, row));


                    }
                    if ("List of devices attached ".Equals(row))
                    {
                        startCompute = true;
                    }
                }
                return deviceNum;
            }
        }

        /// <summary>
        /// 获取屏幕点击像素点
        /// 
        /// </summary>
        /// <param name="cur">像素空间</param>
        /// <param name="eventCur">事件空间</param>
        /// <returns></returns>
        public static Tuple<int, int> GetClickCurPoint(Tuple<int, int> cur, Tuple<int, int> eventCur)
        {
            int x = 0;
            int y = 0;
            using (Process getPintProcess = CmdUtils.createCmd())
            {

                getPintProcess.StandardInput.WriteLine("adb shell getevent");
                String line = null;

                while ((line = getPintProcess.StandardOutput.ReadLine()) != null)
                {
                    // /dev/input/event0: 0003 0035 00000341
                    // /dev/input/event0: 0003 0036 000008ec
                    if (line.Contains("0035"))
                    {
                        String[] items = line.Split(' ');
                        x = int.Parse(items[items.Length - 1], System.Globalization.NumberStyles.HexNumber);
                    }
                    else if (line.Contains("0036"))
                    {
                        String[] items = line.Split(' ');
                        y = int.Parse(items[items.Length - 1], System.Globalization.NumberStyles.HexNumber);
                    }

                    if (x != 0 && y != 0)
                    {
                        break;
                    }
                }
            }

            int curX = x * cur.Item1 / eventCur.Item1;
            int curY = y * cur.Item2 / eventCur.Item2;

            return new Tuple<int, int>(curX, curY);
        }

        /// <summary>
        /// 点击屏幕
        /// </summary>
        /// <param name="process"></param>
        /// <param name="point"></param>
        public static void click(Process process, Tuple<int, int> point)
        {
            process.StandardInput.WriteLine(String.Format("adb shell input tap {0} {1}", point.Item1, point.Item2));
        }
    }
}
