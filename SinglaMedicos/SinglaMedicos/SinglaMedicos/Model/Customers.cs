using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model
{
    public class ConditionTypeAttribute : Attribute
    {
        public ConditionTypeAttribute(string coloumnname, string condition)
        {
            theCondition.Add(condition, coloumnname);
        }
        protected Dictionary<string, string> theCondition = new Dictionary<string, string>();
    }
    [SQLite.Table("Customers"), ConditionType("Customers", "PrimaryCondition")]
    public class Customers
    {
        [SQLite.Column("Custcode")]
        public string Custcode { get; set; }
        
        [SQLite.Column("Name")]
        public string Name { get; set; }
        
        [SQLite.Column("status")]
        public string status { get; set; }
       
        [SQLite.Column("address")]
        public string address { get; set; }
        
        [SQLite.Column("address1")]
        public string address1 { get; set; }
        
        [SQLite.Column("address2")]
        public string address2 { get; set; }
        
        [SQLite.Column("City")]
        public string City { get; set; }
        
        [SQLite.Column("PIN")]
        public string PIN { get; set; }
        
        [SQLite.Column("telephone")]
        public string telephone { get; set; }
        
        [SQLite.Column("mobile")]
        public string mobile { get; set; }
        
        [SQLite.Column("EMail")]
        public string EMail { get; set; }
        
        [SQLite.Column("sman")]
        public string sman { get; set; }
        
        [SQLite.Column("area")]
        public string area { get; set; }
        
        [SQLite.Column("route")]
        public string route { get; set; }
        
        [SQLite.Column("DLNO")]
        public string DLNO { get; set; }
        
        [SQLite.Column("GSTNO")]
        public string GSTNO { get; set; }
        
        [SQLite.Column("CrLimit")]
        public string CrLimit { get; set; }
        
        [SQLite.Column("Tag")]
        public string Tag { get; set; }
       
        [SQLite.Column("ClosingBal")]
        public string ClosingBal { get; set; }
        
        [SQLite.Column("CreatedDt")]
        public string CreatedDt { get; set; }
    }
}
