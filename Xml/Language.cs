using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Serialization;

namespace FutureAuto.Machine.TranslationSoftware
{
    /// <summary>
    /// XMl序列化类
    /// </summary>
    public class Language
    {
        [XmlIgnore]
        static Language  m_Serialize;

        /// <summary>
        /// 读取所选择的配置文件
        /// </summary>
        public static Language Instance(string filterpath)
        {
            lock (typeof(Language))
            {
                m_Serialize = FutureCent.Auto.Common.Config.XmlSerializer.DeserializeFromXml<Language>(filterpath) ?? new Language();

                return m_Serialize;
            }
        }

        [XmlElement]
        public Languages Languages
        {
            get;
            set;
        }= new Languages();


        /// <summary>
        /// 存盘
        /// </summary>
        public static void SaveXml(Language data,string filterpath)
        {
            FutureCent.Auto.Common.Config.XmlSerializer.SerializeToXml(data, filterpath);
        }
    }

    /// <summary>
    /// 文本类
    /// </summary>
    public class Languages
    {
        [XmlElement]
        public List<I> I
        {
            get;
            set;
        } = new List<I>();
    }

    /// <summary>
    /// 语言
    /// </summary>
    public class I
    {
        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void SetLanguages(EnumDefineType type,string data)
        {
            switch (type)
            {
                case EnumDefineType.zh:
                    this.CN = data;
                    break;
                case EnumDefineType.en:
                    this.EN = data;
                    break;
                case EnumDefineType.vie:
                    this.VN = data;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 获取对应的语言
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetLanguage(EnumDefineType type)
        {
            switch (type)
            {
                case EnumDefineType.zh:
                    return this.CN;
                case EnumDefineType.en:
                    return this.EN;
                case EnumDefineType.vie:
                    return this.VN;
                default:
                    break;
            }
            return "选择的语言错误";
        }

        /// <summary>
        /// 中文
        /// </summary>
        public string CN
        {
            get;
            set;
        }

        /// <summary>
        /// 英文
        /// </summary>
        public string EN
        {
            get;
            set;
        }

        /// <summary>
        /// 越南文
        /// </summary>
        public string VN
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 函数集合类
    /// </summary>
    public static class FunctionExecution
    {
        /// <summary>
        /// 文件删除
        /// </summary>
        /// <param name="file"></param>
        public static void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// 去除除中文字符外的所有字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveNonChineseCharacters(string input)
        {
            // 使用正则表达式匹配中文字符
            string pattern = "[^\u4e00-\u9fa5]";
            Regex regex = new Regex(pattern);
            return regex.Replace(input, "");
        }
    }
}
