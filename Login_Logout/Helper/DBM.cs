using Helper.DBMlib;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Login_Logout.Helper
{
    public class DBM : IDisposable
    {
        public static string maincon = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        OracleConnection con;
        OracleTransaction trans;
        public DBM()
        {
            con = createConnection();
        }

        public void Commit()
        {
            if (trans != null)
            {
                trans.Commit();
            }
        }
        public void Rollback()
        {
            if (trans != null)
            {
                trans.Rollback();
            }
        }

        public void BeginTransaction()
        {
            trans = con.BeginTransaction();
        }

        //public int ExecuteNonQuery(string sql, CommandType commandType = CommandType.Text, params OracleParameter[] parameter)
        //{
        //    using (OracleCommand cmd = PrepqreCmd(sql, con, trans, commandType, parameter))
        //    {
        //        return cmd.ExecuteNonQuery();
        //    }
        //}

        public void Dispose()
        {
            con.Dispose();
            trans?.Dispose();
        }

        /* Static method */
        public static OracleConnection createConnection(bool open = true)
        {
            var conn = new OracleConnection(maincon);
            if (open)
            {
                conn.Open();
            }
            return conn;
        }
        private static OracleCommand PrepqreCmd2(string sql, OracleConnection conn, object parameter = null, CommandType commandType = CommandType.Text, string prefix = ":")
        {
            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.CommandType = commandType;
            if (parameter != null)
            {
                cmd.ExtendAddParameters(parameter, prefix);
            }
            return cmd;
        }
        private static OracleCommand PrepqreCmd(string sql, OracleConnection con = null, OracleTransaction trans = null, CommandType commandType = CommandType.Text, params OracleParameter[] parameter)
        {
            if (trans != null)
            {
                OracleCommand cmdtrans = new OracleCommand(sql, trans.Connection);
                cmdtrans.CommandType = commandType;
                cmdtrans.Parameters.AddRange(parameter);
                return cmdtrans;
            }

            OracleCommand cmd = new OracleCommand(sql, con);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameter);
            return cmd;
        }
        public static List<T> ExecuteReader<T>(string sql, CommandType commandType = CommandType.Text, params OracleParameter[] parameter) where T : new()
        {
            List<T> list = new List<T>();
            using (OracleConnection conn = createConnection())
            {
                using(OracleCommand cmd = PrepqreCmd(sql, conn, commandType: commandType, parameter: parameter))
                using (OracleDataReader rd = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    list = rd.ToList<T>();
                }
                
            }
            return list;
        }
        public static int ExecuteNonQuery(string sql, CommandType commandType = CommandType.Text, params OracleParameter[] parameter)
        {
             return ExecuteNonQuery(sql, null , commandType, parameter);
        }
        public static int ExecuteNonQuery(string sql, OracleTransaction trans = null, CommandType commandType = CommandType.Text, params OracleParameter[] parameter)
        {
            if (trans == null)
            {
                using (OracleConnection conn = createConnection())
                {
                    using (OracleCommand cmd = PrepqreCmd(sql, con: conn, commandType: commandType, parameter: parameter))
                    {
                       return cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                using (OracleCommand cmd = PrepqreCmd(sql, trans: trans, commandType: commandType, parameter: parameter))
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string sql, OracleTransaction trans = null, CommandType commandType = CommandType.Text, params OracleParameter[] parameter)
        {
            if (trans == null)
            {
                using (OracleConnection conn = createConnection())
                {
                    OracleCommand cmd = new OracleCommand(sql, conn)
                    {
                        CommandType = commandType
                    };
                    cmd.Parameters.AddRange(parameter);

                    object result = cmd.ExecuteScalar();
                    cmd.Dispose();
                    return result;
                }
            }
            else
            {
                using (OracleCommand cmd = new OracleCommand(sql, trans.Connection)
                {
                    CommandType = commandType,
                    Transaction = trans
                })
                {
                    cmd.Parameters.AddRange(parameter);
                    object result = cmd.ExecuteScalar();
                    return result;
                }
            }
        }
    }
}