using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Helper.DBMlib
{

    public static class DBMextension
    {
        public static void ExtendAddParameters(this OracleCommand cmd, object param, string prefix = ":")
        {

            var property = param.GetType().GetProperties().ToArray();

            foreach (var p in property)
            {
                cmd.Parameters.Add(prefix + p.Name, p.GetValue(param) ?? DBNull.Value);
            }
        }
        public static List<T> ToList<T>(this OracleDataReader reader) where T : new()
        {
            List<T> lst = new List<T>();

            var collumn = Enumerable.Range(0, reader.FieldCount).Select(f =>
            {
                return reader.GetName(f).ToLower();
            }).ToArray();

            var propertyInfos = typeof(T).GetProperties().Where(t => collumn.Contains(t.Name.ToLower()));

            while (reader.Read())
            {
                T obj = new T();
                foreach (var property in propertyInfos)
                {
                    if (reader[property.Name] != DBNull.Value)
                    {
                        try
                        {
                            Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                            object value = Convert.ChangeType(reader[property.Name], type);
                            property.SetValue(obj, value);
                        }
                        catch (Exception e)
                        {

                            throw new Exception("ERROR_CONVERT_DATATYPE: " + $"| {property.Name} |" +  e.Message);
                        }

                    }
                }
                lst.Add(obj);
            }

            return lst;
        }

        public static bool IsDbNull(this object obj)
        {
            return obj is DBNull;
        }

    }
}
