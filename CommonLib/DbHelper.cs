
using System.Data.SqlClient;
using System.Data;

namespace CommonLib
{
    public enum ConnectionStringType
    {
        EnterpriseManagement = 1,//企业管理信息系统
        ElectricitySupplier = 2,//售电公司
        ERP = 3,//ERP
        Test =4,
    }

    public class DbHelper
    {
        private  string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
         static SqlConnection sqlConnection;
         static SqlTransaction sqlTransaction;
        public DbHelper()
        {
        }
        public void StartForTransaction()
        {
            sqlConnection = new SqlConnection(ConnStr);
            sqlConnection.Open();
            sqlTransaction = sqlConnection.BeginTransaction();
        }
        public void Commit()
        {
            sqlTransaction.Commit();
            sqlConnection.Close();
        }
        public void Rollback()
        {
            sqlTransaction.Rollback();
            sqlConnection.Close();
        }
        public DbHelper(ConnectionStringType type)
        {
            switch (type)
            {
                case ConnectionStringType.EnterpriseManagement:
                    ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["EnterpriseManagement"].ToString();
                    break;
                case ConnectionStringType.ElectricitySupplier:
                    ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ElectricitySupplier"].ToString();
                    break;
                case ConnectionStringType.ERP:
                    ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ERP"].ToString();
                    break;
                case ConnectionStringType.Test:
                    ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ToString();
                    break;
                default:
                    ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                    break;
            }
        }
        
        public int ExcuteNonQuery(string sql,  params SqlParameter[] sp)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(sp);

                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public int ExcuteNonQueryForTransaction(string sql, params SqlParameter[] sp)
        {
            using (SqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(sp);
                cmd.Transaction = sqlTransaction;
                return cmd.ExecuteNonQuery();
            }
        }
        public object ExcuteSclar(string sql,  params SqlParameter[] sp)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(sp);
                    return cmd.ExecuteScalar();
                }
            }
        }
        public SqlDataReader ExcuteReader(string sql,  params SqlParameter[] sp)
        {
            SqlConnection conn = new SqlConnection(ConnStr);
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(sp);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
        public DataTable Adaperter(string sql, params SqlParameter[] sp)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter(sql, conn))
                {
                    ada.SelectCommand.CommandType = CommandType.Text;
                    ada.SelectCommand.Parameters.AddRange(sp);
                    DataSet ds = new DataSet();
                    ada.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }

        //public int ExcuteNonQuery(string sql, params SqlParameter[] sp)
        //{
        //    return ExcuteNonQuery(sql, CommandType.Text, sp);
        //}
        //public object ExcuteSclar(string sql, params SqlParameter[] sp)
        //{
        //    return ExcuteSclar(sql, CommandType.Text, sp);
        //}
        //public SqlDataReader ExcuteReader(string sql, params SqlParameter[] sp)
        //{
        //    return ExcuteReader(sql, CommandType.Text, sp);
        //}
        //public DataTable Adaperter(string sql, params SqlParameter[] sp)
        //{
        //    return Adaperter(sql, CommandType.Text, sp);
        //}


    }
}
