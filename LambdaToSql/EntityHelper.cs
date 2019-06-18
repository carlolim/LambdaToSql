using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LambdaToSql
{

    public static class EntityHelper
    {
        public static EntityDetails GetEntityDetails<T>(this T target)
        {

            var result = new EntityDetails();
            var targetType = target.GetType();
            var props = (from p in targetType.GetProperties()
                         let attrs = p.GetCustomAttributes(typeof(KeyAttribute), true)
                         where attrs.Length != 0
                         select p).FirstOrDefault();

            result.PrimaryKeyName = props.Name;
            result.TableName = targetType.GetCustomAttributesData().FirstOrDefault().ConstructorArguments[0].Value.ToString();

            return result;

        }


        public static string GenerateSqlStatement<T>(this T target, EntitySQLGenerationType type, string tableName, string primaryKeyName)
        {
            var result = "";
            Type targetType = target.GetType();
            PropertyInfo[] properties = targetType.GetProperties();

            var databaseParameters = "";
            var parameters = "";

            if (type == EntitySQLGenerationType.InsertStatement)
            {
                databaseParameters = string.Join(",", properties.Where(m => m.Name != primaryKeyName).Select(m => $"@{m.Name}").ToList());
                parameters = databaseParameters.Replace("@", "");
                result = $"INSERT INTO {tableName} ({parameters})VALUES({databaseParameters})" +
                            " SELECT CAST(SCOPE_IDENTITY() AS INT)";
            }
            else
            {
                databaseParameters = string.Join(",", properties.Where(m => m.Name != primaryKeyName).Select(m => $"{m.Name} = @{m.Name}").ToList());
                result = $"UPDATE {tableName} Set {databaseParameters} where {primaryKeyName} = @{primaryKeyName}";
            }


            return result;
        }

        public static string GenerateInsertSqlStatement<T>(this T target, string tableName, string[] except)
        {
            var result = "";
            Type targetType = target.GetType();
            PropertyInfo[] properties = targetType.GetProperties();

            var databaseParameters = "";
            var parameters = "";

            databaseParameters = string.Join(",", properties.Where(m => !except.Contains(m.Name)).Select(m => $"@{m.Name}").ToList());
            parameters = databaseParameters.Replace("@", "");
            result = $"INSERT INTO {tableName} ({parameters})VALUES({databaseParameters})" +
                        " SELECT CAST(SCOPE_IDENTITY() AS INT)";

            return result;
        }

        public static string GenerateInsertSqlStatement<T>(this T target, string tableName)
        {
            var result = "";
            Type targetType = target.GetType();
            PropertyInfo[] properties = targetType.GetProperties();

            var databaseParameters = "";
            var parameters = "";

            databaseParameters = string.Join(",", properties.Select(m => $"@{m.Name}").ToList());
            parameters = databaseParameters.Replace("@", "");
            result = $"INSERT INTO {tableName} ({parameters})VALUES({databaseParameters})" +
                        " SELECT CAST(SCOPE_IDENTITY() AS INT)";

            return result;
        }

        public static string[] ToArrayOfNames<T>(this T target)
        {
            Type targetType = target.GetType();
            PropertyInfo[] properties = targetType.GetProperties();
            var result = properties.Select(m => m.Name).ToArray();
            return result;
        }


    }
}
