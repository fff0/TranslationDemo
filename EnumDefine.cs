using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FutureAuto.Machine.TranslationSoftware
{
    public enum EnumDefineType : byte
    {
        /// <summary>
        /// 中文
        /// </summary>
        [Description("中文")]
        zh,

        /// <summary>
        /// 英文
        /// </summary>
        [Description("英文")]
        en,

        /// <summary>
        /// 越南语
        /// </summary>
        [Description("越南文")]
        vie,
    }

    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class EnumExtend
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="enumvalue"></param>
        /// <returns></returns>
        public static string GetTypeEnumName(this Enum enumvalue)
        {
            FieldInfo fieldInfo = enumvalue.GetType().GetField(enumvalue.ToString());

            object[] value = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (value.Length > 0)
            {
                return ((DescriptionAttribute)value[0]).Description;
            }

            return enumvalue.ToString();
        }

        /// <summary>
        /// 获取枚举描述数组
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static List<string> GetEnumNameList(this Array array)
        {
            List<string> strings = new List<string>();

            foreach (var item in array)
            {
                if (item is Enum)
                {
                    strings.Add(((Enum)item).GetTypeEnumName());
                }
            }

            return strings;
        }
    }
}
