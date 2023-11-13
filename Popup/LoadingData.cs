using System;
using System.ComponentModel;

namespace FutureAuto.Machine.TranslationSoftware
{
    public class LoadingData : INotifyPropertyChanged
    {
        static volatile LoadingData m_Instance;

        /// <summary>
        /// 单实例
        /// </summary>
        public static LoadingData Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (typeof(LoadingData))
                    {
                        if (m_Instance == null)
                        {
                            m_Instance = new LoadingData();
                        }
                    }
                }

                return m_Instance;
            }
        }

        private int m_Totaldata;
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalData
        {
            get
            {
                return m_Totaldata;
            }
            set
            {
                m_Totaldata = value;
                OnPropertyChanged(nameof(TotalData));
            }
        }

        private int m_Accomplishdata;
        /// <summary>
        /// 完成数量
        /// </summary>
        public int AccomplishData
        {
            get
            {
                return m_Accomplishdata;
            }
            set
            {
                m_Accomplishdata = value;
                OnPropertyChanged(nameof(AccomplishData));
            }
        }

        private TimeSpan m_Time;
        /// <summary>
        /// 剩余所需时长
        /// </summary>
        public TimeSpan SurplusTime
        {
            get
            {
                return m_Time;
            }
            set
            {
                m_Time = value;
                OnPropertyChanged(nameof(SurplusTime));
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
