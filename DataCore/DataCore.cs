using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DataCore
{
    public class DataCore<T> where T : class, new()
    {
        /// <summary>
        /// Never use the keyword "Using" with this property. 
        /// Because of how JIT handles reference types and value types, 
        /// it will not create a copy of the instance and end it in the execution block, it will end the property's instance.
        /// </summary>s
        private SqlConnection Connection { get; set; }

        public DataCore(SqlConnection connection)
        {
            Connection = connection;
        }

        public void ExecuteCommand(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, Connection);

            Connection.Open();
            sqlCommand.ExecuteNonQuery();
            Connection.Close();
        }

        public void ExecuteCommand(string sql, object param)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, Connection);
            sqlCommand.Parameters.AddRange(GetParameters(param).ToArray());

            Connection.Open();
            sqlCommand.ExecuteNonQuery();
            Connection.Close();
        }

        public List<T> ExecuteQuery(string sql, object param)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, Connection);
            sqlCommand.Parameters.AddRange(GetParameters(param).ToArray());

            Connection.Open();
            DataTable dataTable = GetDataTable(sqlCommand);
            Connection.Close();

            return GetObjList(dataTable);
        }

        public List<T> ExecuteQuery(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, Connection);

            Connection.Open();
            DataTable dataTable = GetDataTable(sqlCommand);
            Connection.Close();

            return GetObjList(dataTable);
        }

        private DataTable GetDataTable(SqlCommand sqlCommand)
        {
            DataTable dataTable = new DataTable();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
            dataAdapter.Fill(dataTable);

            dataAdapter.Dispose();

            return dataTable;
        }

        private IEnumerable<SqlParameter> GetParameters(object param)
        {
            foreach (PropertyInfo property in param.GetType().GetProperties())
            {
                yield return new SqlParameter($"@{property.Name}", property.GetValue(param));
            }
        }

        private List<T> GetObjList(DataTable table)
        {
            try
            {
                List<T> list = new List<T>();

                foreach (DataRow row in table.Rows)
                {
                    T obj = (T)Activator.CreateInstance(typeof(T));

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);

                            object typedValue = Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType);

                            propertyInfo.SetValue(obj, typedValue);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}