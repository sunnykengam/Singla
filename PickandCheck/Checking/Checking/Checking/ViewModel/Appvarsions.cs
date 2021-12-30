using System;
using System.Collections.Generic;

namespace Checking.ViewModel
{
    public class ConditionTypeAttribute : Attribute
    {
        public ConditionTypeAttribute(string coloumnname, string condition)
        {
            theCondition.Add(condition, coloumnname);
        }
        protected Dictionary<string, string> theCondition = new Dictionary<string, string>();
    }
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
