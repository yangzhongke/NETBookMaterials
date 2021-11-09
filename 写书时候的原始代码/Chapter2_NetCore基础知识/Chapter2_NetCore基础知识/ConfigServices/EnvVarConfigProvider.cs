using System;

namespace ConfigServices
{
    class EnvVarConfigProvider : IConfigProvider
    {
        public string GetValue(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}