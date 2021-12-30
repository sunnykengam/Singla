using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model
{
    [SQLite.Table("Partner"), ConditionType("Partner", "PrimaryCondition")]
    public class Partner
    {
        [SQLite.Column("id")]
        public int id { get; set; }

        [SQLite.Column("Code")]
        public int Code { get; set; }

        [SQLite.Column("Name")]
        public string Name { get; set; }

        [SQLite.Column("Branch")]
        public string Branch { get; set; }

        [SQLite.Column("ShortName")]
        public string ShortName { get; set; }

        public string guid { get; set; }

        public string totalamount { get; set; }

        public int NoOfItems { get; set; }

        public string partnername { get; set; }

    }
}
