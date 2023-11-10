using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FutureAuto.Machine.TranslationSoftware
{
    /// <summary>
    /// 百度翻译API
    /// </summary>
    public class BaiDuApi : IApi
    {
        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="q">待翻译的文本</param>
        /// <param name="from">文本语言</param>
        /// <param name="to">要翻译的语言</param>
        /// <returns></returns>
        public string Get(string q, string from = "zh", string to = "vie")
        {
            try
            {
                // 百度API限制，高级版1秒可以执行10次，标准版1秒执行1次，添加延时
                Thread.Sleep(150);

                string retString;
                // 百度翻译API注册ID和密钥
                var appid = "20220412001170118";
                var key = "3STcluZq_kHCbX59r0U5";

                //var appid = "20231031001864768";
                //var key = "j0cfRDkrU5hwUzq6Oruz";

                // 随机数
                var rd = new Random();
                var salt = rd.Next(100000).ToString();
                // 密钥组成的字符串
                var sign = BaiDuApi.EncryptString(appid + q + salt + key);

                var url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
                url += "q=" + HttpUtility.UrlEncode(q);
                url += "&from=" + from;
                url += "&to=" + to;
                url += "&appid=" + appid;
                url += "&salt=" + salt;
                url += "&sign=" + sign;
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.UserAgent = null;
                request.Timeout = 10000;
                var response = (HttpWebResponse)request.GetResponse();

                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();

                return retString;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 计算MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }

        /// <summary>
        /// 解析接口数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetResult(string data)
        {
            // 解析返回的JSON数据
            var datastr = JsonConvert.DeserializeObject<BaiDuApiResult>(data);

            if (datastr != null && datastr.trans_result.Count > 0)
            {
                return datastr.trans_result[0].dst;
            }
            return null;
        }
    }
}
