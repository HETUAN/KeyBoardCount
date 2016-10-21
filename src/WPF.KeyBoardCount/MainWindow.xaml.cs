using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.KeyBoardCount
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    { 
        private System.Windows.Forms.NotifyIcon notifyIcon;
        public MainWindow()
        {
            InitializeComponent();
            //bind global event delegate
            App.hook_KeyDown += new RawKeyEventHandler(Hook_KeyDown);

            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            this.notifyIcon.BalloonTipText = "系统监控中... ...";
            this.notifyIcon.ShowBalloonTip(2000);
            this.notifyIcon.Text = "系统监控中... ...";
            this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);//new System.Drawing.Icon(@"AppIcon.ico");
            this.notifyIcon.Visible = true;
            //打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("Open");
            open.Click += new EventHandler(Show);
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += new EventHandler(CloseW);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left) this.Show(o, e);
            });
        }

        /// <summary>
        /// key press global event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Hook_KeyDown(object sender, RawKeyEventArgs args)
        {
            Lab_Count.Content = App.GetKeyCount().ToString();
        }

        #region  系统托盘实现

        private void Show(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            this.WindowState = System.Windows.WindowState.Normal;
            this.Activate();
            this.Topmost = true;
            this.Focus();
        }

        private void Hide(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void CloseW(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                Hide(sender, e); 
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        { 
            MessageBoxResult result = MessageBox.Show("确定要关闭吗？", "提示", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                this.notifyIcon.Visible = false;
                this.notifyIcon.Dispose(); 
                e.Cancel = false; 
            }
            else
            {
                this.ShowInTaskbar = false;
                this.Visibility = System.Windows.Visibility.Hidden;
                e.Cancel = true;
            }
        }
    }
}
