using System.Collections.Generic;

namespace FutureAuto.Machine.TranslationSoftware
{
    /// <summary>
    /// 翻译接口返回数据
    /// </summary>
    internal class BaiDuApiResult
    {
        /// <summary>
        /// 传入的语言语种
        /// </summary>
        public string from { get; set; }

        /// <summary>
        /// 要翻译的语种
        /// </summary>
        public string to { get; set; }

        /// <summary>
        /// 接口返回结果
        /// </summary>
        public List<Data> trans_result { get; set; } = new List<Data>();
    }

    /// <summary>
    /// 翻译接口返回数据
    /// </summary>
    public class Data
    {
        /// <summary>
        /// 传入的数据
        /// </summary>
        public string src { get; set; }

        /// <summary>
        /// 翻译后的数据
        /// </summary>
        public string dst { get; set; }
    }
}
