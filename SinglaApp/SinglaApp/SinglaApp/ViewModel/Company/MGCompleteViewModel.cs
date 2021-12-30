using MvvmHelpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SinglaApp.Common;
using SinglaApp.Model.Company;
using Xamarin.Forms;
using Microsoft.AppCenter.Analytics;
using SinglaApp.Model;
using System.Linq;

namespace SinglaApp.ViewModel.Company
{

    public class MGCompleteViewModel
    {
        public SQLiteAsyncConnection database;
        public ObservableRangeCollection<MGroupViewModel> completelist { get; private set; }
            = new ObservableRangeCollection<MGroupViewModel>();
        public ICommand LoadDataCommand { get; private set; }
        public ICommand HeaderClickCommand { get; private set; }
        public MGCompleteViewModel()
        {
            try
            {
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                this.LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
                this.HeaderClickCommand = new Command<MGroupViewModel>((item) => ExecuteHeaderClickCommand(item));

            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MGCompleteViewModel--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }
        public async Task ExecuteLoadDataCommand()
        {
            try
            {
                string qry = "Select itemname,manufacturer From Itemmaster where manufacturer not like '#%' order by manufacturer;";
                var itemDetails = Task.Run(async () => await database.QueryAsync<ItemMaster>(qry)).Result;
                var itemDetail = itemDetails.Select(l => l.manufacturer).Distinct().ToList();
                foreach (string data in itemDetail.ToList())
                {
                    MGroupName item = new MGroupName();
                    item.manufacturer = data;
                    item.Product_List = (from pro in itemDetails
                                         where pro.manufacturer == data
                                         select new ItemName { itemname = pro.itemname }).Distinct().ToList();
                    var Custview = new MGroupViewModel(item);
                    completelist.Add(Custview);
                }
                AppSettings.MGroup_List = completelist;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MGCompleteViewModel--ExecuteLoadDataCommand--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void ExecuteHeaderClickCommand(MGroupViewModel item)
        {
            item.Expanded = !item.Expanded;
        }
    }
}
