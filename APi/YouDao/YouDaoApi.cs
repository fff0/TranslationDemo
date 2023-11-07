using FutureAuto.Machine.TranslationSoftware.APi.YouDao;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FutureAuto.Machine.TranslationSoftware
{
    /// <summary>
    /// 有道翻译APi
    /// </summary>
    public class YouDaoApi : IApi
    {
        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="q">待翻译的文本</param>
        /// <param name="from">文本语言</param>
        /// <param name="to">要翻译的语言</param>
        /// <returns></returns>
        public string Get(string q, string from, string to)
        {
            Thread.Sleep(500);
            string resStr = "";
            // 您的应用ID
            string APP_KEY = "5d5d73a084993bb7";
            // 您的应用密钥
            string APP_SECRET = "E7EHo5MmtBXawRwvb6WUaOiyMj4kmHNi";

            // 添加请求参数
            Dictionary<String, String[]> paramsMap = createRequestParams(q, from, to);
            // 添加鉴权相关参数
            AuthV3Util.addAuthParams(APP_KEY, APP_SECRET, paramsMap);
            Dictionary<String, String[]> header = new Dictionary<string, string[]>() { { "Content-Type", new String[] { "application/x-www-form-urlencoded" } } };
            // 请求api服务
            byte[] result = HttpUtil.doPost("https://openapi.youdao.com/api", header, paramsMap, "application/json");
            // 打印返回结果
            if (result != null)
            {
                resStr = System.Text.Encoding.UTF8.GetString(result);
            }

            return resStr;
        }

        /// <summary>
        /// 解析接口数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetResult(string data)
        {
            // 解析返回的JSON数据
            var datastr = JsonConvert.DeserializeObject<YouDaoApiResult>(data);

            if (datastr != null && datastr.translation.Count > 0)
            {
                return datastr.translation[0];
            }
            return null;
        }



        private static Dictionary<String, String[]> createRequestParams(string q, string from, string to)
        {
            // note: 将下列变量替换为需要请求的参数
            string vocabId = "general";

            return new Dictionary<string, string[]>() {
                { "q", new string[]{q}},
                {"from", new string[]{from}},
                {"to", new string[]{to}},
                {"vocabId", new string[]{vocabId}}
            };
        }
    }
}
