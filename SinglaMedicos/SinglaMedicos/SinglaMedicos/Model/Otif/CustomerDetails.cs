using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model
{
    [SQLite.Table("CustomerDetails")]
    public class CustomerDetails
    {
        [SQLite.Column("code")]
        public int code
        {
            get;
            set;
        }
        [SQLite.Column("altercode")]
        public int altercode
        {
            get;
            set;
        }
        [SQLite.Column("name")]
        public string name
        {
            get;
            set;
        }
        [SQLite.Column("address")]
        public string address
        {
            get;
            set;
        }
        [SQLite.Column("address1")]
        public string address1
        {
            get;
            set;
        }
        [SQLite.Column("address2")]
        public string address2
        {
            get;
            set;
        }
        [SQLite.Column("address3")]
        public string address3
        {
            get;
            set;
        }
        [SQLite.Column("address4")]
        public string address4
        {
            get;
            set;
        }
        [SQLite.Column("telephone")]
        public string telephone
        {
            get;
            set;
        }
        [SQLite.Column("mobile")]
        public string mobile
        {
            get;
            set;
        }
        [SQLite.Column("dlno")]
        public string dlno
        {
            get;
            set;
        }
        [SQLite.Column("InvID")]
        public string InvID
        {
            get;
            set;
        }
        [SQLite.Column("DlvryDate")]
        public string DlvryDate
        {
            get;
            set;
        }
        [SQLite.Column("Status")]
        public string Status
        {
            get;
            set;
        }
        [SQLite.Column("DlvryTime")]
        public string DlvryTime
        {
            get;
            set;
        }
        [SQLite.Column("CreatedDate")]
        public string CreatedDate
        {
            get;
            set;
        }
        [SQLite.Column("Password")]
        public string Password
        {
            get;
            set;
        }
    }
}
