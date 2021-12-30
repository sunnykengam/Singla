using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model.Company
{
    public class MGroupName
    {
        public string manufacturer { get; set; }
        public List<ItemName> Product_List { get; set; } = new List<ItemName>();
    }
}
