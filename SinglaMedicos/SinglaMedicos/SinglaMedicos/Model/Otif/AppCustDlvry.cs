using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model
{
    [SQLite.Table("AppCustDlvry"), ConditionType("AppCustDlvry", "PrimaryCondition")]
    public class AppCustDlvry
    {
        [SQLite.Column("Custcode")]
        public int Custcode
        {
            get;
            set;
        }
        [SQLite.Column("Sno"), SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Sno
        {
            get;
            set;
        }
        [SQLite.Column("CustAltcode")]
        public string CustAltcode
        {
            get;
            set;
        }
        [SQLite.Column("Custname")]
        public string Custname
        {
            get;
            set;
        }
        [SQLite.Column("NoofBoxes")]
        public string NoofBoxes
        {
            get;
            set;
        }
        [SQLite.Column("Noofpackes")]
        public string Noofpackes
        {
            get;
            set;
        }
        [SQLite.Column("Photo")]
        public string Photo
        {
            get;
            set;
        }
        [SQLite.Column("InvoiceImage")]
        public byte[] InvoiceImage
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
        [SQLite.Column("InvoiceDeltails")]
        public string InvoiceDeltails
        {
            get;
            set;
        }
        [SQLite.Column("InvoiceDate")]
        public string InvoiceDate
        {
            get;
            set;
        }

        [SQLite.Column("CreatedDate")]
        public DateTime CreatedDate
        {
            get;
            set;
        }
        [SQLite.Column("DlvryCode")]
        public string DlvryCode
        {
            get;
            set;
        }
        [SQLite.Column("patnercode")]
        public int patnercode
        {
            get;
            set;
        }

    }
}
