using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FutureAuto.Machine.TranslationSoftware
{
    /// <summary>
    /// Message.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingWindow : Window
    {
        /// <summary>
        /// 弹框
        /// </summary>
        public LoadingWindow()
        {
            InitializeComponent();
            this.DataContext = LoadingData.Instance;
        }
    }
}
