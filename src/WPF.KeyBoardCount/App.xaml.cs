using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WPF.KeyBoardCount
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static long keyCount = 0;  //当前计时段的数量
        private static long curKeyCount = 0;  //敲击键盘的总数
        private static string guidStr = Guid.NewGuid().ToString();
        public static string mac = "";

        private static MainWindow w;

        public static long GetKeyCount()
        {
            return keyCount;
        }

        public static long GetCurKeyCount()
        {
            return curKeyCount;
        }

        public static void ClearCurKeyCount()
        {
            curKeyCount = 0;
        }

        public static void AddKeyCount()
        {
            curKeyCount++;
            keyCount++;
        }

        public static string GetGuid()
        {
            return guidStr;
        }

        Thread writeThread;
        //KeyboardListener KListener = new KeyboardListener();
        KeyboardListener KListener;

        public static event RawKeyEventHandler hook_KeyDown;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //ChartWindow cw = new ChartWindow();
            //cw.Show();
            //return;
            App.mac = new HardHelper().GetMacAddress();
            KListener = new KeyboardListener();
            App.w = new MainWindow();
            App.w.Show();
            KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
            writeThread = new Thread(new ThreadStart(WriteData));
            writeThread.IsBackground = true;
            writeThread.Start();
        }

        private void WriteData()
        {
            XmlHelper xml = new XmlHelper("C:\\");
            xml.Run();
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs e)
        {
            curKeyCount++;
            keyCount++;
            hook_KeyDown(sender, e);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            KListener.Dispose();
            writeThread.Abort();
            XmlHelper xml = new XmlHelper("C:\\");
            xml.WriteData();
            KListener.Dispose();
        }

        public static void AllExit()
        {
            XmlHelper xml = new XmlHelper("C:\\");
            xml.WriteData();
        }

        public void OpenLineCount(object sender, StartupEventArgs e)
        {
            LineCount l = new LineCount();
            l.Show();
        }
    }
}
