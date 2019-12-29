using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using SC.Common.Log;
using System.Collections.Generic;

namespace NanoFabric.Infrastrue.Ibatis.DataAccess
{
    internal class Mapper
    {
        //protected static readonly ILog logger = LogFactory.GetLogger();

        private static volatile Dictionary<string, ISqlMapper> _mappers = new Dictionary<string, ISqlMapper>();

        protected static void Configure(object obj)
        {

        }

        /// <summary>
        /// 实现IBatis接口
        /// </summary>
        /// <param name="connectConfig"></param>
        protected static ISqlMapper InitMapper(string sqlMapConfig)
        {

            ConfigureHandler handler = new ConfigureHandler(Configure);
            DomSqlMapBuilder builder = new DomSqlMapBuilder();

            ISqlMapper mp = null;
            try
            {
                if (_mappers.ContainsKey(sqlMapConfig))
                {
                    mp = _mappers[sqlMapConfig];
                }
                else
                {

                    mp = builder.ConfigureAndWatch(sqlMapConfig, handler);
                    _mappers.Add(sqlMapConfig, mp);
                }
            }
            catch (System.Exception e)
            {
                Logger.Error(e.StackTrace);
                return null;
                //logger.Exception(e.Message, e);
            }

            return mp;
        }

        public static ISqlMapper Instance(string sqlMapConfig)
        {
            ISqlMapper mp;

            if (!_mappers.TryGetValue(sqlMapConfig, out mp))
            {
                lock (_mappers)
                {
                    mp = InitMapper(sqlMapConfig);
                }
            }

            return mp;
        }
    }
}
