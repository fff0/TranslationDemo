using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FutureAuto.Machine.TranslationSoftware.Xml
{
    public static class XmlApi
    {
        /// <summary>
        /// 保存方法
        /// </summary>
        /// <returns></returns>
        public static bool Save<T>(string filePath,object data)
        {
            bool result = false;
            try
            {
                // 判断目录是否存在
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                using (FileStream fs =
                    new FileStream(filePath, FileMode.Create))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    xs.Serialize(fs, data);

                }
                result = true;
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        /// <summary>
        /// 加载配置参数
        /// </summary>
        /// <returns></returns>
        public static T Load<T>(string filePath) where T : class,new()
        {
            var data = new T();
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    data = xs.Deserialize(fs) as T;
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }
    }
}
