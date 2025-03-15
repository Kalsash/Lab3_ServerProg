using CsvHelper;
using Lab3_ServerProg.Pages;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Lab3_ServerProg
{
    public static class CsvHelperService
    {
        private const string CsvFilePath = "contacts.csv";

        public static async Task SaveRecordAsync(ContactRecord record)
        {
            var records = new List<ContactRecord> { record };
            using (var writer = new StreamWriter(CsvFilePath, true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(records);
            }
        }
    }
}
