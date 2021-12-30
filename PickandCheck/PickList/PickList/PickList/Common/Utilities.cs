using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PickList.Common
{
    public class Utilities
    {
        public static SQLiteAsyncConnection database;
        public static SQLiteAsyncConnection GetDbPath()
        {
            var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("PicklistDB.Sqlite");
            database = new SQLiteAsyncConnection(path);
            return database;
        }
    }
}
