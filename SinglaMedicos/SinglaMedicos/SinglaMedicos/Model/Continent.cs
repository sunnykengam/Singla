using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model
{
    public class Continent
{
    public string Name { get; set; }
    public string Stock { get; set; }
        public List<ItemMaster>Items { get; set; } = new List<ItemMaster>();
}
}
