using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TapHelper
{
    public class AdbUtils
    {
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
                        string[] items = row.Split(' ');
                        foreach (var item in items)
                        {
                            if (item.StartsWith("cur"))
                            {
                                var datas = item.Substring(4).Split('x');
                                int width = int.Parse(datas[0]);
                                int height = int.Parse(datas[1]);
                                return new Tuple<int, int>(width, height);
                            }
                        }
                    }
                }

                return null;
            }
        }

        public static Tuple<int, int> GetEventWidthAndHeight()
        {
            using (Process process = CmdUtils.createCmd())
            {
                process.StandardInput.WriteLine("adb shell getevent -p");
                var row = "";
                int width = 0;
                int height = 0;
                while ((row = process.StandardOutput.ReadLine()) != null)
                {

                    if (row.Contains("0035") || row.Contains("0036"))
                    {

                        string[] items = row.Split(' ');
                        for (int i = 0, n = items.Length; i < n; i++)
                        {
                            if ("max".Equals(items[i]))
                            {
                                var value = items[i+1];

                                if (row.Contains("0035"))
                                {
                                    width = int.Parse(value.Substring(0,value.Length -1));

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

        public static int GetDeviceNum()
        {
            using (Process process = CmdUtils.createCmd())
            {
                //向process窗口发送输入信息
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
    }
}
