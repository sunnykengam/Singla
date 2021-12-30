using Newtonsoft.Json;
using SinglaApp.Model.Reports;

namespace SinglaApp.ViewModel
{
    public class OutstandingInvoicesViewmodel
    {
        private OutstandingInvoices _outstandinvoices;
        public OutstandingInvoicesViewmodel(OutstandingInvoices Outstandinvoices)
        {
            this._outstandinvoices = Outstandinvoices;
        }
        [JsonProperty(PropertyName = "invoicedate")]
        public string invoicedate { get { return _outstandinvoices.invoicedate; } }

        [JsonProperty(PropertyName = "invoicenum")]
        public string invoicenum { get { return _outstandinvoices.invoicenum; } }

        [JsonProperty(PropertyName = "noofitem")]
        public int noofitem { get { return _outstandinvoices.noofitem; } }

        [JsonProperty(PropertyName = "invtotalamount")]
        public decimal invtotalamount { get { return _outstandinvoices.invtotalamount; } }

        [JsonProperty(PropertyName = "invbalanceamount")]
        public decimal invbalanceamount { get { return _outstandinvoices.invbalanceamount; } }

        [JsonProperty(PropertyName = "totaldiscount")]
        public decimal totaldiscount { get { return _outstandinvoices.totaldiscount; } }

        [JsonProperty(PropertyName = "Days")]
        public int Days { get { return _outstandinvoices.Days; } }
    }
}
