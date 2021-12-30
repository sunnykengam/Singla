using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model
{
    [SQLite.Table("DeliveryStatus")]
    public class DeliveryStatus
    {
        [SQLite.Column("cust_code")]
        public int cust_code
        {
            get;
            set;
        }
        [SQLite.Column("cust_name")]
        public string cust_name
        {
            get;
            set;
        }
        [SQLite.Column("inv_num")]
        public string inv_num
        {
            get;
            set;
        }
        [SQLite.Column("inv_date")]
        public string inv_date
        {
            get;
            set;
        }
        [SQLite.Column("noofitems")]
        public int noofitems
        {
            get;
            set;
        }
        [SQLite.Column("inv_amount")]
        public int inv_amount
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
        [SQLite.Column("status")]
        public string status
        {
            get;
            set;

        }
        [SQLite.Column("DlvryStatus")]
        public string DlvryStatus
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

        [SQLite.Column("DlvryTime")]
        public string DlvryTime
        {
            get;
            set;

        }
        [SQLite.Column("DlvryImage")]
        public string DlvryImage
        {
            get;
            set;

        }
        [SQLite.Column("NoofBoxes")]
        public int NoofBoxes
        {
            get;
            set;

        }

    }
}
