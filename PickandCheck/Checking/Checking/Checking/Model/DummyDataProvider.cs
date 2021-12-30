using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Checking.Models
{
    static class DummyDataProvider
    {
        public static List<InvoiceItem> GetTeams()
        {
            var assembly = typeof(DummyDataProvider).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Checking.teams.json");
            string json = string.Empty;

            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<List<InvoiceItem>>(json);
        }
    }
}