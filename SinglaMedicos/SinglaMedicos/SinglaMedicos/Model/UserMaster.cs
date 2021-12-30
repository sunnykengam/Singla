using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model
{
    [SQLite.Table("UserMaster"), ConditionType("UserMaster", "PrimaryCondition")]
    public class UserMaster
    {
        [SQLite.Column("Id")]
        public int Id { get; set; }
      
        [SQLite.Column("Username")]
        public string Username { get; set; }
      
        [SQLite.Column("Password")]
        public string Password { get; set; }
       
        [SQLite.Column("ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    
        [SQLite.Column("Name")]
        public string Name { get; set; }
     
        [SQLite.Column("UserType")]
        public string UserType { get; set; }
      
        [SQLite.Column("status")]
        public string status { get; set; }
    
        [SQLite.Column("mobile")]
        public string mobile { get; set; }
     
        [SQLite.Column("area")]
        public string area { get; set; }
     
        [SQLite.Column("route")]
        public string route { get; set; }
      
        [SQLite.Column("ClosingBal")]
        public string ClosingBal { get; set; }

        [SQLite.Column("CreatedDt")]
        public string CreatedDt { get; set; }
      
        [SQLite.Column("Custcode")]
        public string Custcode { get; set; }
    }
}
