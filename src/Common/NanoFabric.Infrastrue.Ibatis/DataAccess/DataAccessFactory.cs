namespace NanoFabric.Infrastrue.Ibatis.DataAccess
{
    public class DataAccessFactory
    {
        public static IDataAccess Instance()
        {
            return Instance("");
        }

        public static IDataAccess Instance(string sqlMapConfig)
        {
            return DataAccessHelp.Create(sqlMapConfig);
        }
    }
}
