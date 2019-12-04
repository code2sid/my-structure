using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DataAccess
{
    internal static class DataExtensions
    {
        public static DbCommand CreateCommand(this DbConnection cnn, string text, CommandType commandType = CommandType.Text)
        {
            var command = cnn.CreateCommand();
            command.CommandText = text;
            command.CommandType = commandType;
            return command;
        }

        private static void AddParameter(this DbCommand command, string name, DbType type, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Value = value.ValueOrDbNull();
            command.Parameters.Add(parameter);
        }

        public static void AddParameter(this DbCommand command, string name, int? value)
        {
            command.AddParameter(name, DbType.Int32, value);
        }

        public static void AddParameter(this DbCommand command, string name, bool? value)
        {
            command.AddParameter(name, DbType.Boolean, value);
        }

        public static void AddParameter(this DbCommand command, string name, string value)
        {
            command.AddParameter(name, DbType.String, value);
        }

        public static void AddParameter(this DbCommand command, string name, DateTime? value)
        {
            command.AddParameter(name, DbType.DateTime, value);
        }

        public static object ValueOrDbNull<T>(this T value) where T : class
        {
            return (object)value ?? DBNull.Value;
        }

        public static async Task<DataSet> ExecuteDataSetAsync(this DbCommand command)
        {
            using (var reader = await command.ExecReaderAwaitable())
            {
                return ReadDataSet(reader);
            }
        }

        public static ConfiguredTaskAwaitable<DbDataReader> ExecReaderAwaitable(this DbCommand command)
        {
            return command.ExecuteReaderAsync().ConfigureAwait(false);
        }

        private static DataSet ReadDataSet(IDataReader reader)
        {
            var ds = new DataSet();
            do
            {
                var dt = GenerateNextTable(ds, reader);
                FillDataTable(dt, reader);
            } while (reader.NextResult());

            return ds;
        }

        private static DataTable GenerateNextTable(DataSet ds, IDataRecord record)
        {
            var dt = ds.Tables.Add();

            for (var i = 0; i < record.FieldCount; i++)
            {
                dt.Columns.Add(record.GetName(i), record.GetFieldType(i));
            }

            return dt;
        }

        private static void FillDataTable(DataTable dt, IDataReader reader)
        {
            while (reader.Read())
            {
                var row = dt.NewRow();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader[i];
                }

                dt.Rows.Add(row);
            }
        }

        public static IEnumerable<string> GetRows(this DataTable dataTable)
        {
            return dataTable.Rows.Cast<DataRow>().Select(r => r[0].ToString());
        }

        public static T SafeGet<T>(this DataRow reader, string columnName, Func<object, T> converter, T defaultValue)
        {
            var raw = reader[columnName];
            return raw is DBNull ? defaultValue : converter(raw);
        }
    }
}
