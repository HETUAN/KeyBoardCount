using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace WPF.KeyBoardCount
{
    /// <summary>
    /// ChartWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChartWindow : Window
    {
        public ChartWindow()
        {
            InitializeComponent();
        }

        private void ChartWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {

                XmlHelper xmlHelper = new XmlHelper("C:\\");
                XmlDocument doc = xmlHelper.LoadXML();

                Dictionary<string, int> dayCounts = new Dictionary<string, int>();
                XmlNodeList nodes = doc.SelectNodes("/tdata/days/onedaydata");
                if (nodes != null)
                {
                    foreach (XmlNode node in nodes)
                    {
                        string date = node.Attributes["day"].Value;
                        DateTime dt = DateTime.Parse(date);
                        if (dt < DateTime.Today.AddMonths(-1))
                            break;
                        dayCounts.Add(date, Convert.ToInt32(node.InnerText));
                    }
                }
                if (dayCounts.Count <= 0)
                    return;
                int maxVal = 0;
                foreach (var dayCountsValue in dayCounts.Values)
                {
                    if (dayCountsValue > maxVal)
                        maxVal = dayCountsValue;
                }

                double width = this.Width - 50;
                double height = this.Height - 80;
                DrawLine(0, height, width, height, 2, System.Windows.Media.Brushes.Tomato);  //X
                DrawLine(50, 20, 50, height + 20, 2, System.Windows.Media.Brushes.Tomato);  //Y

                double xNum = width / dayCounts.Count;
                int yCount = maxVal / 5000;
                double yNum = height / yCount;

                for (int i = 1; i < yCount; i++)
                {
                    double h = height - yNum * i;
                    DrawLine(30, h, 50, h, 2, System.Windows.Media.Brushes.Tomato);
                }

                int j = 0;
                foreach (string key in dayCounts.Keys)
                {
                    double w = j * xNum + 50;
                    j++;
                    DrawLine(w, height, w, height + 20, 2, System.Windows.Media.Brushes.Tomato);
                }

                int oldVal = 0;
                int idx = 0;
                foreach (KeyValuePair<string, int> kv in dayCounts)
                {
                    if (oldVal == 0)
                    {
                        oldVal = kv.Value;
                        continue;
                    }
                    idx++;
                    double h1 = (1.00000 - Convert.ToDouble(oldVal) / Convert.ToDouble(maxVal)) * height;
                    double h2 = (1.00000 - Convert.ToDouble(kv.Value) / Convert.ToDouble(maxVal)) * height;
                    DrawLine(idx * xNum, h1, (idx + 1) * xNum, h2, 2, System.Windows.Media.Brushes.Black);
                    oldVal = kv.Value;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        private void DrawString(string testString)
        {
            TextBox tb = new TextBox();

        }

        private void DrawLine(double x1, double y1, double x2, double y2, double thickness, System.Windows.Media.Brush color)
        {

            Line xline = new Line();
            xline.Stroke = color;//System.Windows.Media.Brushes.Red;
            xline.X1 = x1;
            xline.Y1 = y1;
            xline.X2 = x2;
            xline.Y2 = y2;
            xline.HorizontalAlignment = HorizontalAlignment.Left;
            xline.VerticalAlignment = VerticalAlignment.Center;
            xline.StrokeThickness = thickness;
            DrawBoard.Children.Add(xline);
        }
    }
}
