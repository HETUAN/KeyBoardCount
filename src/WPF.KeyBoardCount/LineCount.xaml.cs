using System;
using System.Collections.Generic;
using System.IO;
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

namespace WPF.KeyBoardCount
{
    /// <summary>
    /// LineCount.xaml 的交互逻辑
    /// </summary>
    public partial class LineCount : Window
    {
        public LineCount()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog1.Description = "请选择文件夹";
            //folderBrowserDialog1.ShowNewFolderButton = true;
            //folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Personal;
            System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                if (folderName != "" && Directory.Exists(folderName))
                {
                    CountPath.Text = folderName;
                }
                else
                {
                    MessageBox.Show("所选路径有问题！");
                }
            }
        }

        /// <summary>
        /// 刷新行数
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="files"></param>
        public void RefData(int lines, int files)
        {
            this.CurRow.Content = lines;
            this.CurFile.Content = files;
        }

        /// <summary>
        /// 开始扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(CountPath.Text.Trim()))
            {
                FileCount fc = new FileCount(this);
                fc.LinesOfFolder(CountPath.Text.Trim());
            }
            else
            {
                MessageBox.Show("路径不正确！");
            }
        }

    }
}
