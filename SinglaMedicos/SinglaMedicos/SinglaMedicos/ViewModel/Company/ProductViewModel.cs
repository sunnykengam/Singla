using System;
using System.Collections.Generic;
using System.Text;
using SinglaMedicos.Model.Company;

namespace SinglaMedicos.ViewModel.Company
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
