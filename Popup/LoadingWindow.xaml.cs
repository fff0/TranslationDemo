using System.Windows;

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
