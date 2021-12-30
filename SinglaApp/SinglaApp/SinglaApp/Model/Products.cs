using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model
{
    public class Products
    {
        [SQLite.Column("Id")]
        public string Id { get; set; }

        [SQLite.Column("Name")]
        public string Name { get; set; }

        [SQLite.Column("Code")]
        public string Code { get; set; }

        [SQLite.Column("Qty")]
        public string Qty { get; set; }

        [SQLite.Column("Scheme")]
        public string Scheme { get; set; }

        [SQLite.Column("Size")]
        public string Size { get; set; }

        [SQLite.Column("HalfScheme ")]
        public string HalfScheme { get; set; }
    }
}
