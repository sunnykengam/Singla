using System;
using System.Collections.Generic;
using System.Text;
using SinglaApp.Model.Company;

namespace SinglaApp.ViewModel.Company
{
    public class ProductViewModel
    {
        private ItemName _Product;
        public ProductViewModel(ItemName Product)
        {
            this._Product = Product;
        }
        public string itemname { get { return _Product.itemname; } }
    }
}
