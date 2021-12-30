using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model
{
    [SQLite.Table("routeMaster"), ConditionType("routeMaster", "PrimaryCondition")]
    public class routeMaster
    {
        [SQLite.Column("id")]
        public string id
        {
            get;
            set;
        }
        [SQLite.Column("createdAt")]
        public string createdAt
        {
            get;
            set;
        }
        [SQLite.Column("updatedAt")]
        public string updatedAt
        {
            get;
            set;
        }
        [SQLite.Column("version")]
        public string version
        {
            get;
            set;
        }
        [SQLite.Column("deleted")]
        public string deleted
        {
            get;
            set;
        }
        [SQLite.Column("RouteId")]
        public string RouteId
        {
            get;
            set;
        }
        [SQLite.Column("RouteName")]
        public string RouteName
        {
            get;
            set;
        }
        [SQLite.Column("Partnercode")]
        public int Partnercode
        {
            get;
            set;
        }
    }
}
