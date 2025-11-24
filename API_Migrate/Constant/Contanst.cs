using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace API_Migrate.Constant
{
    public class Contanst
    {
        private string controller_path;
        private string name_StoreCheckSync;
        private string table_ReceiveData; 

        public string Controller_path { get => controller_path; set => controller_path = value; }
        public string Name_StoreCheckSync { get => name_StoreCheckSync; set => name_StoreCheckSync = value; }
        public string Table_ReceiveData { get => table_ReceiveData; set => table_ReceiveData = value; }

        public Contanst() {
            this.controller_path = @"./Controller";
            this.name_StoreCheckSync = "api_CheckSyncAAL";
            this.Table_ReceiveData = "api_receiveAAL"; 
        }
        public  string ExtractSelectQuery(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            XNamespace ns = "urn:schemas-fast-com:fast-api";
            string? selectQuery = doc.Descendants(ns + "text")
                                 .FirstOrDefault() // Get the first <text> element
                                 ?.Value            // Get its value (CDATA content)
                                 .Trim();
            return selectQuery;
        }
    }
}
