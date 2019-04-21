using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace taphelper
{
    class SichuanPeopleClub
    {
        public static void Main(string[] args)
        {
            //if (!envCheck())
            //{
            //    Console.ReadLine();
            //    return;
            //}

            // 获取屏幕像素
            var cur = AdbUtils.GetCur();
            Console.WriteLine("屏幕像素：" + cur);

            // 获取事件空间大小
            var eventCur = AdbUtils.GetEventWidthAndHeight();
            Console.WriteLine("事件空间：" + eventCur);

            Console.WriteLine("请进入四川人设小程序主页后输入回车：");
            Console.ReadLine();

            Console.WriteLine("请点击'四川人设系统窗口单位业务技能练兵比武网络答题竞赛活动':");
            var initPoint = AdbUtils.GetClickCurPoint(cur, eventCur);
            Console.WriteLine(initPoint);

            Console.WriteLine("请点击'答题检测'按钮");
            var answerCheck = AdbUtils.GetClickCurPoint(cur, eventCur);
            Console.WriteLine(answerCheck);

            Console.WriteLine("请点击'答题注意事项确认'按钮");
            var noticeConfirem = AdbUtils.GetClickCurPoint(cur, eventCur);
            Console.WriteLine(noticeConfirem);

            Console.WriteLine("请点击'答案选项A按钮，只能点击一个");
            var aAnswer = AdbUtils.GetClickCurPoint(cur, eventCur);
            Console.WriteLine(aAnswer);

            Console.WriteLine("请点击'答案选项D按钮，只能点击一个");
            var dAnswer = AdbUtils.GetClickCurPoint(cur, eventCur);
            Console.WriteLine(dAnswer);
            var answerHeight = (dAnswer.Item2 - aAnswer.Item2) / 3;

            Console.WriteLine("请点击'下一题'按钮");
            var newQuestion = AdbUtils.GetClickCurPoint(cur, eventCur);
            Console.WriteLine(newQuestion);

            Console.WriteLine("请点击'提交答卷'按钮");
            var submit = AdbUtils.GetClickCurPoint(cur, eventCur);
            Console.WriteLine(submit);

            Console.WriteLine("请点击'现在交卷'按钮");
            var submitNow = AdbUtils.GetClickCurPoint(cur, eventCur);
            Console.WriteLine(submitNow);

            Console.WriteLine("请点击左上角'返回'按钮");
            var returnNow = AdbUtils.GetClickCurPoint(cur, eventCur);
            Console.WriteLine(returnNow);

            Console.WriteLine("请输入点击的间隔时间（毫秒）：");
            var intervelMillions = int.Parse(Console.ReadLine());

            Console.WriteLine("请输入想做的单选题数量：");
            var questionNum = int.Parse(Console.ReadLine());

            Console.WriteLine("请输入回车键从主页开始做题");
            Console.ReadLine();
            while (true)
            {
                using (var process = CmdUtils.createCmd())
                {
                    Console.WriteLine("开始做题......");
                    AdbUtils.click(process, initPoint);
                    Console.WriteLine(String.Format("已经点击'四川人设系统窗口单位业务技能练兵比武网络答题竞赛活动'，point:{0}", initPoint));
                    Thread.Sleep(intervelMillions);

                    AdbUtils.click(process, answerCheck);
                    Console.WriteLine(String.Format("已经点击'答题检测'按钮，point:{0}", answerCheck));
                    Thread.Sleep(intervelMillions);

                    AdbUtils.click(process, noticeConfirem);
                    Console.WriteLine(String.Format("已经点击'答题注意事项确认'按钮，point:{0}", noticeConfirem));
                    Thread.Sleep(intervelMillions);

                    for (var i = 0; i < questionNum; i++)
                    {
                        // 防止答案位置变化，所以在那个x位置的y方向上多点几次
                        var x = aAnswer.Item1;
                        var y = aAnswer.Item2 - 2 * answerHeight;
                        for (int j = 0; j < 7; j++)
                        {
                            y += answerHeight;
                            var answer = new Tuple<int, int>(x, y);
                            AdbUtils.click(process, answer);
                            Console.WriteLine(String.Format("第{0}题，已经点击按钮，point:{1}", (i + 1), answer));
                            Thread.Sleep(2000);
                        }
                        // Thread.Sleep(intervelMillions);

                        AdbUtils.click(process, newQuestion);
                        Console.WriteLine(String.Format("已经点击'下一题'按钮，point:{0}", newQuestion));
                        Thread.Sleep(intervelMillions);
                    }



                    AdbUtils.click(process, submit);
                    Console.WriteLine(String.Format("已经点击'提交答卷'按钮'，point:{0}", submit));
                    Thread.Sleep(intervelMillions);

                    AdbUtils.click(process, submitNow);
                    Console.WriteLine(String.Format("已经点击'现在交卷'按钮'，point:{0}", submitNow));
                    Thread.Sleep(intervelMillions);

                    AdbUtils.click(process, returnNow);
                    Console.WriteLine(String.Format("已经点击'返回'按钮'，point:{0}", returnNow));
                    Thread.Sleep(intervelMillions);


                    AdbUtils.click(process, submitNow);
                    Console.WriteLine(String.Format("已经点击'返回'按钮2'，point:{0}", returnNow));
                    Thread.Sleep(intervelMillions);

                    // Console.WriteLine("请手工返回主页后输入回车键后继续做题");
                }
            }
        }

        /// <summary>
        /// 环境检查
        /// </summary>
        /// <returns></returns>
        private static bool envCheck()
        {
            Console.WriteLine("环境检查开始......");

            int deviceNum = AdbUtils.GetDeviceNum();

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
