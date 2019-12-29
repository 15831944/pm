using NanoFabric.Infrastrue.Ibatis.DataAccess;
using System;
using System.Collections;

namespace NanoFabric.Infrastrue.Ibatis.BLBase
{
    public class BaseBL : HTAbstractBLBaseTransactionLogic
    {
        public static IDataAccess doAccess;

        public BaseBL()
        {
            doAccess = dataAccess;

        }

        public T DBOperation<T>(Func<Hashtable, T> operation, Hashtable inParameter)
        {
            try
            {
                T result = operation(inParameter);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType(), ex);
                throw ex;
            }
            finally
            {
                this.dataAccess.Close();
            }
        }

        public T DBOperationWithTrans<T>(Func<Hashtable, T> operation, Hashtable inParameter)
        {
            try
            {
                this.dataAccess.BeginTransaction();
                T result = operation(inParameter);
                this.dataAccess.Commit();
                return result;
            }
            catch (Exception ex)
            {
                this.dataAccess.Rollback();
                LogHelper.WriteLog(this.GetType(), ex);
                throw ex;
            }
            finally
            {
                this.dataAccess.Close();
            }
        }
    }
}