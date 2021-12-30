using System;
using System.Collections.Generic;
using System.Text;
using SinglaMedicos.Model;

namespace SinglaMedicos.ViewModel
{
    [SQLite.Table("Appvarsions"), ConditionType("Appvarsions", "PrimaryCondition")]
    public class Appvarsions
    {
        [SQLite.Column("Name")]
        public string Name
        {
            get;
            set;
        }
        [SQLite.Column("Value")]
        public string Value
        {
            get;
            set;
        }
        [SQLite.Column("sno"), ConditionType("sno", "PrimaryCondition")]
        public int sno { get; set; }

    }
}
