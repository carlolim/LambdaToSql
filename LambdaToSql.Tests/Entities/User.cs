using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaToSql.Tests.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool IsOnline { get; set; }
        public int Age { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
