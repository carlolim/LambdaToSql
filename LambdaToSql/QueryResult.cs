using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaToSql
{
    public class QueryResult
    {
        public string SqlQuery { get; set; }
        public DynamicParameters Parameters { get; set; }
        public string SqlQueryParameterized { get; set; }
    }
}
