using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureAuto.Machine.TranslationSoftware
{
    /// <summary>
    /// API接口
    /// </summary>
    internal interface IApi
    {
        /// <summary>
        /// 调用接口返回数据
        /// </summary>
        /// <param name="q"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        string Get(string q, string from, string to);

        /// <summary>
        /// 获取翻译结果
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string GetResult(string data);
    }
}
