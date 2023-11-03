using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

using static FutureAuto.Machine.TranslationSoftware.MessageControl;

namespace FutureAuto.Machine.TranslationSoftware
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += ConfigViewPage_Loaded;

            this.DataContext = this;
        }

        #region 页面绑定数据

        /// <summary>
        /// 等待窗口
        /// </summary>
        private LoadingWindow loadingWindow;

        /// <summary>
        /// 翻译文件路径
        /// </summary>
        Language file { get; set; }

        /// <summary>
        /// XML数据
        /// </summary>
        Language TranslateText { get; set; } = new Language();
       
        /// <summary>
        /// 原文数据
        /// </summary>
        public ObservableCollection<ListBoxTextClass> InitializedDataList 
        { 
            get; 
            set;
        }=new ObservableCollection<ListBoxTextClass>();

        /// <summary>
        /// 译文数据
        /// </summary>
        public ObservableCollection<ListBoxTextClass> TranslateDataList
        {
            get;
            set;
        } = new ObservableCollection<ListBoxTextClass>();

        /// <summary>
        /// 用于存储当前翻译完成后的类型
        /// </summary>
        public EnumDefineType NowType
        {
            get;
            set;
        }

        private string m_TargetText = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        /// <summary>
        /// 选择的路径文本
        /// </summary>
        public string TargetText
        {
            get 
            {
                return m_TargetText; 
            }
            set
            {
                m_TargetText = value;
                OnPropertyChanged(nameof(TargetText));
            }
        }

        #endregion

        #region 加载方法

        /// <summary>
        /// 加载页面方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ConfigViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            m_from.ItemsSource = Enum.GetValues(typeof(EnumDefineType)).GetEnumNameList();
            if(m_from.Items.Count>0) m_from.SelectedIndex = 0;

            m_to.ItemsSource = Enum.GetValues(typeof(EnumDefineType)).GetEnumNameList();
            if (m_from.Items.Count > 0) m_to.SelectedIndex = 1;
        }

        #endregion

        #region 页面按钮事件

        /// <summary>
        /// 翻译语言切换按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutButton_Click(object sender, RoutedEventArgs e)
        {
            int index = m_from.SelectedIndex;
            m_from.SelectedIndex = m_to.SelectedIndex;
            m_to.SelectedIndex   = index;
        }

        /// <summary>
        /// 数据列表鼠标右键按钮选择点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem menuItem = (System.Windows.Controls.MenuItem)sender;
            string option = menuItem.Header as string;

            var text = (sender as System.Windows.Controls.MenuItem).DataContext as ListBoxTextClass;
            if (option == "修改")
            {
                text.IsReadOnly = false;
            }
            else if (option == "复制")
            {
                // 要复制到剪贴板的文本
                var textToCopy = text.DataValue;
                // 复制文本到剪贴板
                System.Windows.Clipboard.SetText(textToCopy);
            }
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 设置文件夹路径
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Xaml文件|*.xml; | All|*.*"
                };

                if (dialog.ShowDialog() == true)
                {
                   
                    this.m_SelectXml_TextBlockText.Text = dialog.FileName;
                    // 执行翻译
                    file = FutureAuto.Machine.TranslationSoftware.Language.Instance(dialog.FileName);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Translate_buttonClick(object sender, RoutedEventArgs e)
        {
            // 当选择了文件才执行翻译
            if (!string.IsNullOrEmpty(m_SelectXml_TextBlockText.Text) && file != null)
            {
                if (file.Languages.I.Count == 0)
                {
                    m_MessageBox.SetMessageValueAsync(MessageType.Error, "选择的文件中没有可翻译的数据！", 2000);
                    return;
                }

                loadingWindow = new LoadingWindow();
                // 订阅主窗口的拖动事件
                LocationChanged += MainWindow_LocationChanged;
                // 位于主窗口上方
                loadingWindow.Owner = this;
                LoadingData.Instance.TotalData = file.Languages.I.Count;
                // 当点击按钮时，就让进度条显示出来
                m_progressBar.Value = 0.01;

                try
                {
                    this.IsEnabled = false;

                    loadingWindow.Show();
                    InitializedDataList.Clear();
                    TranslateDataList.Clear();

                    var from = (EnumDefineType)m_from.SelectedIndex;
                    var to   = (EnumDefineType)m_to.SelectedIndex;

                    await Translate(file, from, to);
                }
                finally
                {
                    this.IsEnabled = true;
                    loadingWindow.Close();
                }
            }
            else
            {
                m_MessageBox.SetMessageValueAsync(MessageType.Hint, "请选择文件后再点击翻译按钮",2000);
            }
        }

       /// <summary>
        /// 翻译执行函数
        /// </summary>
        /// <param name="language">XML数据</param>
        /// <param name="from">原文语言</param>
        /// <param name="to">译文语言</param>
        /// <returns></returns>
        public Task Translate(Language language, EnumDefineType from, EnumDefineType to)
        {
            TranslateText = language;
            return Task.Run(() =>
            {
                string text = "";

                for (int i = 0; i < TranslateText.Languages.I.Count; i++)
                {
                    if (!string.IsNullOrEmpty(TranslateText.Languages.I[i].GetLanguage(from)))
                    {
                        text = (TranslateText.Languages.I[i].GetLanguage(from) );

                        // 调用百度翻译APi，得到翻译后的结果
                        var bb = HttpHelp.Get(text, from.ToString(), to.ToString());
                        // 解析返回的JSON数据
                        var data = JsonConvert.DeserializeObject<Result>(bb);

                        if (data != null && data.trans_result.Count > 0)
                        {
                            var cc = data.trans_result[0].dst;

                            // 给原数据 添加 翻译后的数据
                            TranslateText.Languages.I[i].SetLanguages(to, cc);
                        }
                    }
                    // 修改页面进度条表现
                    this.Dispatcher.Invoke(() =>
                    {
                        LoadingData.Instance.AccomplishData = i;
                        LoadingData.Instance.SurplusTime = TimeSpan.FromSeconds(Math.Floor((TranslateText.Languages.I.Count - i) * 0.3));

                        // 执行长时间文件，计算的进度保留小数后三位还是0时，让值为0.01,避免进度条闪烁消失一下再出现，
                        var value = double.Parse(((double)i / (double)TranslateText.Languages.I.Count).ToString("F3")) * 100;
                        m_progressBar.Value = value > 0 ? value : 0.01;
                    });
                }

                // 给页面列表添加数据
                this.Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < TranslateText.Languages.I.Count; i++)
                    {
                        InitializedDataList.Add(new ListBoxTextClass()
                        {
                            ID = $"{i + 1}:",
                            DataValue = $"{TranslateText.Languages.I[i].GetLanguage(from)}",
                            IsReadOnly = true,
                        });

                        TranslateDataList.Add(new ListBoxTextClass() 
                        { 
                            ID = $"{i + 1}:", 
                            DataValue = $"{TranslateText.Languages.I[i].GetLanguage(to)}" ,
                            IsReadOnly = true,
                            
                        });
                    }
                    // 存储当前翻译的类型
                    NowType = to;
                    m_progressBar.Value = 100;
                });
            });
        }

        /// <summary>
        /// 选择导出文件路径按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleckPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "请选择导出文件路径"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    return;
                }

                // 页面显示的路径名
                this.TargetText = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// 导出XML按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExportXml_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                if (!string.IsNullOrEmpty(TargetText))
                {
                    // 桌面文件路径
                    var datapath = Path.Combine(TargetText, "Aaa.xml");

                    if (TranslateDataList.Count == 0)
                    {
                        m_MessageBox.SetMessageValueAsync(MessageType.Error, "没有可导出的数据！");
                        return;
                    }

                    m_MessageBox.SetMessageValue(MessageType.Hint, "正在导出xml数据...");
                    await Task.Run(() =>
                    {
                        //根据文件路径,删除当前文件
                        FunctionExecution.DeleteFile(datapath);

                        for (int i = 0; i < TranslateDataList.Count; i++)
                        {
                            TranslateText.Languages.I[i].SetLanguages(NowType, TranslateDataList[i].DataValue);
                        }

                        FutureAuto.Machine.TranslationSoftware.Language.SaveXml(TranslateText, datapath);
                    });

                    m_MessageBox.SetMessageValueAsync(MessageType.Success, "导出xml成功！");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        #endregion

        #region 行为事件
        /// <summary>
        /// 订阅主窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            // 当主窗口位置发生变化时，将等待弹窗的位置更新为与主窗口一致
            if (loadingWindow != null && IsLoaded)
            {
                loadingWindow.Left = Left + (Width - loadingWindow.Width) / 2;
                loadingWindow.Top = Top + (Height - loadingWindow.Height) / 2;
            }
        }

        /// <summary>
        /// ListBoxItem取消选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_Unselected(object sender, RoutedEventArgs e)
        {
            var data = (sender as ListBoxItem).DataContext as ListBoxTextClass;
            data.IsReadOnly = true;
        }

        /// <summary>
        /// ListBoxItem鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var data = (sender as ListBoxItem).DataContext as ListBoxTextClass;
            data.IsReadOnly = false;

            //// 获取选中的项目
            //object selectedItem = m_TranslateList.SelectedItem;
            //// 使用 ScrollIntoView 方法将滚动条滚动到选中的项目
            //m_TranslateList.ScrollIntoView(selectedItem);
        }

        /// <summary>
        /// ListBox滚动条事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            // 当选中修改文本时，禁用掉滚动条同步事件，防止修改时滚动条移动到初始位置
            if ((m_TranslateList.SelectedIndex > 0 && TranslateDataList[m_TranslateList.SelectedIndex].IsReadOnly) || m_TranslateList.SelectedIndex < 0)
            {
                if (sender == m_DataList)
                {
                    SyncScroll(m_TranslateList, e.VerticalOffset);
                }
                else if (sender == m_TranslateList)
                {
                    SyncScroll(m_DataList, e.VerticalOffset);
                }
            }
        }

        /// <summary>
        /// 让两个ListBox同时滚动
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="verticalOffset"></param>
        private void SyncScroll(System.Windows.Controls.ListBox listBox, double verticalOffset)
        {
            var scrollViewer = FindVisualChild<ScrollViewer>(listBox);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset(verticalOffset);
            }
        }

        /// <summary>
        /// 获取滚动的位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        private T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                if (child != null && child is T found)
                {
                    return found;
                }
                else
                {
                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null)
                        return childItem;
                }
            }
            return null;
        }

        #endregion

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

    /// <summary>
    /// 数据列表数据
    /// </summary>
    public class ListBoxTextClass : INotifyPropertyChanged
    {
        /// <summary>
        /// 序号ID
        /// </summary>
        public string ID
        {
            get;
            set;
        }

        private string m_DataValue = "";
        /// <summary>
        /// 翻译文本
        /// </summary>
        public string DataValue
        {
            get
            {
                return m_DataValue;
            }
            set
           {
                m_DataValue = value;
                OnPropertyChanged(nameof(DataValue));
            }
        }

        private bool m_IsReadOnly = true;
        /// <summary>
        /// 指示文本是否可修改
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return m_IsReadOnly;
            }
            set
            {
                m_IsReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
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

