using System.ComponentModel;

namespace FutureAuto.Machine.TranslationSoftware
{
    /// <summary>
    /// 翻译APi类型
    /// </summary>
    enum EnumApiType : byte
    {
        /// <summary>
        /// 百度翻译APi
        /// </summary>
        [Description("百度")]
        BaiDu,

        /// <summary>
        /// 有道翻译APi
        /// </summary>
        [Description("有道")]
        YouDao,

        /// <summary>
        /// 腾讯翻译APi
        /// </summary>
        [Description("腾讯")]
        Tencent,
    }
}
