namespace Config2_82配置基础2
{
    class Config
    {
        public string Name { get; set; }
        public Server Proxy { get; set; }
    }

    class Server
    {
        public string Address { get; set; }
        public int Port { get; set; }
    }
}