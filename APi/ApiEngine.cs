namespace FutureAuto.Machine.TranslationSoftware
{
    /// <summary>
    /// API引擎类
    /// </summary>
    internal class ApiEngine
    {
        /// <summary>
        /// 翻译API
        /// </summary>
        public IApi TranslateApi
        {
            get;
            set;
        }

        /// <summary>
        /// 初始化API引擎
        /// </summary>
        /// <param name="type"></param>
        public void Init(EnumApiType type = EnumApiType.BaiDu)
        {
            switch (type)
            {
                case EnumApiType.BaiDu:
                    this.TranslateApi = new BaiDuApi();
                    break;
                case EnumApiType.YouDao:
                    this.TranslateApi = new YouDaoApi();
                    break;
                default:
                    this.TranslateApi = new BaiDuApi();
                    break;
            }
        }
    }
}
