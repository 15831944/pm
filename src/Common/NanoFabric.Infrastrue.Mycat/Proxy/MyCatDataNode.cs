namespace NanoFabric.Infrastrue.Mycat.Proxy
{
    public class MyCatDataNode
    {
        public MyCatDatabaseHost Slave { get; set; }
        public MyCatDatabaseHost Master { get; set; }
    }
}
