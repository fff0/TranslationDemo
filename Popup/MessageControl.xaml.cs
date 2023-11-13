using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FutureAuto.Machine.TranslationSoftware
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageControl : UserControl, INotifyPropertyChanged
    {
        public MessageControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// 【异步】延时关闭提示文本
        /// </summary>
        /// <param name="mestype">文本提示类型</param>
        /// <param name="text">文本提示文字</param>
        /// <param name="time">显示时间</param>
        public async void SetMessageValueAsync(MessageType mestype, string text, int time = 3000)
        {

            MessageBoxVisibility = Visibility.Visible;
            this.MesType = (byte)mestype;
            this.MessageText = text;

            await Task.Run(async () =>
            {
                await Task.Delay(time);
                this.Dispatcher.Invoke(() =>
                {
                    MessageBoxVisibility = Visibility.Collapsed;
                });
            });
        }

        /// <summary>
        /// 设置提示文本
        /// </summary>
        /// <param name="mestype">文本提示类型</param>
        /// <param name="text">文本提示文字</param>
        public void SetMessageValue(MessageType mestype, string text)
        {
            MessageBoxVisibility = Visibility.Visible;
            this.MesType = (byte)mestype;
            this.MessageText = text;
        }

        /// <summary>
        /// 关闭提示文本
        /// </summary>
        public void CloseMessageTip()
        {
            this.MessageBoxVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 提示文本类型
        /// </summary>
        public enum MessageType : byte
        {
            Error = 0,
            Hint = 1,
            Success = 2,
        }

        private byte m_MesType;
        /// <summary>
        /// 当前提示文本类型
        /// </summary>
        public byte MesType
        {
            get
            {
                return m_MesType;
            }
            set
            {
                m_MesType = value;
                OnPropertyChanged(nameof(MesType));
            }
        }


        private string m_MessageText;
        /// <summary>
        /// 提示文字
        /// </summary>
        public string MessageText
        {
            get
            {
                return m_MessageText;
            }
            set
            {
                m_MessageText = value;
                OnPropertyChanged(nameof(MessageText));
            }
        }

        private Visibility m_MessageBoxVisibility = Visibility.Collapsed;
        /// <summary>
        /// 是否显示提示文本
        /// </summary>
        public Visibility MessageBoxVisibility
        {
            get
            {
                return m_MessageBoxVisibility;
            }
            set
            {
                m_MessageBoxVisibility = value;
                OnPropertyChanged(nameof(MessageBoxVisibility));
            }
        }

        /// <summary>
        /// 属性变更通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性变更函数
        /// </summary>
        /// <param name="propertyName">属性名</param>
        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
