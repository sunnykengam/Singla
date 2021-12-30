using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OTIF.Model
{
    public class Customers
    { 
     

        private string _custname;
        private string _password;
        public string _custcode;
        private int _code;
        public string _address;
        public string _address1;
        public string _address2;
        public string _address3;
        public string _address4;
        public string _telephone;
        public string _mobile;
        public string _dlnum;
        public string _dlnum1;
        public string _route;
        public string _status;

        [JsonProperty(PropertyName = "custname")]
        public string custname
        {
            get => _custname;
            set
            {
                _custname = value;
               
            }
        }
       

        [JsonProperty(PropertyName = "custcode")]
        public string custcode
        {
            get => _custcode;
            set
            {
                _custcode = value;
               
            }
        }

        [JsonProperty(PropertyName = "password")]
        public string password
        {
            get => _password;
            set
            {
                _password = value;
              
            }
        }

        [JsonProperty(PropertyName = "address")]
        public string address
        {
            get => _address;
            set
            {
                _address = value;
             
            }
        }


        [JsonProperty(PropertyName = "address1")]
        public string address1
        {
            get => _address1;
            set
            {
                _address1 = value;
               
            }
        }

        [JsonProperty(PropertyName = "address2")]
        public string address2
        {
            get => _address2;
            set
            {
                _address2 = value;
              
            }
        }

        [JsonProperty(PropertyName = "address3")]
        public string address3
        {
            get => _address3;
            set
            {
                _address3 = value;
               
            }
        }

        [JsonProperty(PropertyName = "address4")]
        public string address4
        {
            get => _address4;
            set
            {
                _address4 = value;
               
            }
        }



        [JsonProperty(PropertyName = "telephone")]
        public string telephone
        {
            get => _telephone;
            set
            {
                _telephone = value;
              
            }
        }


        [JsonProperty(PropertyName = "mobile")]
        public string mobile
        {
            get => _mobile;
            set
            {
                _mobile = value;
                
            }
        }

        [JsonProperty(PropertyName = "dlnum")]
        public string dlnum
        {
            get => _dlnum;
            set
            {
                _dlnum = value;
              
            }
        }

        [JsonProperty(PropertyName = "dlnum1")]
        public string dlnum1
        {
            get => _dlnum1;
            set
            {
                _dlnum1 = value;
              
            }
        }

        [JsonProperty(PropertyName = "route")]
        public string route
        {
            get => _route;
            set
            {
                _route = value;
             
            }
        }


        [JsonProperty(PropertyName = "status")]
        public string status
        {
            get => _status;
            set
            {
                _status = value;
               
            }
        }
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
       
        public string Version { get; set; }
        [JsonProperty(PropertyName = "createdAt")]
        public string createdAt { get; set; }
        [JsonProperty(PropertyName = "  updatedAt")]
        public string updatedAt { get; set; }
        [JsonProperty(PropertyName = "  partnercode")]
        public int partnercode { get; set; }
        [JsonProperty(PropertyName = "code")]
        public int code { get => _code; set => _code = value; }
    }
}