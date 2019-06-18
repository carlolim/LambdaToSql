using Dapper;
using LambdaToSql;
using LambdaToSql.SproutRecruitApi.Core.Extensions;
using LambdaToSql.Tests.Entities;
using NUnit.Framework;
using System;
using System.Linq.Expressions;

namespace Tests
{
    public class SelectTests
    {
        private readonly ExpressionTranslator expressionTranslator;
        public SelectTests()
        {
            expressionTranslator = new ExpressionTranslator();
        }

        [Test]
        public void WithStringParameters_ReturnsSqlString_ShouldReturnTrue()
        {
            var expectedResult = "SELECT * FROM [User] WHERE ((Username = @Username) AND (Password = @Password))";

            var whereClause = WhereClause<User>(m => m.Username == "carlo" && m.Password == "1234");
            var entityDetails = EntityDetails<User>();

            var result = $"SELECT * FROM [{ entityDetails.TableName }] WHERE { whereClause.SqlQueryParameterized }";

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void WithDateParameter_ReturnsSqlString_ShouldReturnTrue()
        {
            var birthday = new DateTime(1992, 10, 09, 11, 30, 0);
            var expectedQuery = "SELECT * FROM [User] WHERE (BirthDay = @BirthDay)";

            var whereClause = WhereClause<User>(m => m.BirthDay == birthday);
            var entityDetails = EntityDetails<User>();

            var actualQuery = $@"SELECT * FROM [{entityDetails.TableName}] WHERE { whereClause.SqlQueryParameterized}";
            
            Assert.AreEqual(expectedQuery, actualQuery);
            
        }

        [Test]
        public void WithDateParameter_ReturnsDynamicParameter_ShouldReturnTrue()
        {
            var birthday = new DateTime(1992, 10, 09, 11, 30, 0);
            var expectedParameter = new DynamicParameters();
            expectedParameter.Add("@Birthday", birthday);

            var whereClause = WhereClause<User>(m => m.BirthDay == birthday);

            var actualParameter = whereClause.Parameters;

            Assert.AreEqual(actualParameter.ParameterNames.AsList()[0], "BirthDay");
            Assert.That(actualParameter.Get<DateTime>("BirthDay"), Is.EqualTo(birthday));
        }

        private EntityDetails EntityDetails<T>() where T : new()
        {
            var entity = new T();
            return entity.GetEntityDetails();
        }
        

        private QueryResult WhereClause<TEntity>(Expression<Func<TEntity, bool>> pred) where TEntity : class
        {
            return expressionTranslator.TranslateToQueryResult(pred);
        }
    }
}