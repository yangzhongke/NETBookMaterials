using System.Collections.Generic;

namespace ConfigServices
{
    class LayeredConfigReader : IConfigReader
    {
        private readonly IEnumerable<IConfigProvider> configProviders;
        public LayeredConfigReader(IEnumerable<IConfigProvider> configProviders)
        {
            this.configProviders = configProviders;
        }
        public string GetValue(string name)
        {
            //从多个IConfigProvider中读取配置，如果后面加入的IConfigProvider
            //也提供了前面有的配置项，则后面覆盖的前面的
            string value =null;
            foreach(var provider in configProviders)
            {
                string pValue = provider.GetValue(name);
                if(pValue!=null)//如果读到了，就覆盖之前的
                {
                    value = pValue;
                }
            }
            return value;
        }
    }
}