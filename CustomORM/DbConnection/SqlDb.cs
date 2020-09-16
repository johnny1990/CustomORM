using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CustomORM.DbConnection
{
    public static class SqlDb
    {
        public static string MainConnectionString
        {
            get
            {
                return conns != null ? conns : System.Configuration.ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
            }
            set
            {
                conns = value;
            }
        }
        static string conns = null;
        public static IDataReader Get(List<SqlParameter> paramsList, string My_SP)
        {

            SqlConnection con = new SqlConnection(MainConnectionString);

            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = con;
            command.CommandText = My_SP + "_get";
            command.CommandTimeout = 0;
            DataTable dt = new DataTable();
            foreach (var param in paramsList)
            {
                command.Parameters.Add(param);
            }
            try
            {
                con.Open();
                using (TransactionScope ts = new TransactionScope())
                {
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return dt.CreateDataReader();
        }


        public static long? Insert(List<SqlParameter> paramsList, string My_SP)
        {
            long? ID = null;
            SqlConnection con = new SqlConnection(MainConnectionString);
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = con;
            command.CommandText = My_SP + "_create";
            command.CommandTimeout = 0;
            DataTable dt = new DataTable();
            foreach (var param in paramsList)
            {
                command.Parameters.Add(param);
            }
            try
            {
                con.Open();
                using (TransactionScope ts = new TransactionScope())
                {
                    ID = Convert.ToInt64(command.ExecuteScalar());
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return ID;

        }

        public static void Delete(List<SqlParameter> paramsList, string My_SP)
        {
            SqlConnection con = new SqlConnection(MainConnectionString);

            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = con;
            command.CommandText = My_SP + "_delete";
            command.CommandTimeout = 0;
            DataTable dt = new DataTable();
            foreach (var param in paramsList)
            {
                command.Parameters.Add(param);
            }
            try
            {
                con.Open();
                using (TransactionScope ts = new TransactionScope())
                {
                    command.ExecuteNonQuery();
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
        }

        public static void Update(List<SqlParameter> paramsList, string My_SP)
        {
            SqlConnection con = new SqlConnection(MainConnectionString);

            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = con;
            command.CommandText = My_SP + "_update";
            command.CommandTimeout = 0;
            DataTable dt = new DataTable();
            foreach (var param in paramsList)
            {
                command.Parameters.Add(param);
            }
            try
            {
                con.Open();
                using (TransactionScope ts = new TransactionScope())
                {
                    command.ExecuteNonQuery();
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
        }
    }
}
