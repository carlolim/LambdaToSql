using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaToSql.Tests.Entities
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string Fullname { get; set; }
    }
}
