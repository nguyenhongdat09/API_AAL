using API_Migrate.Constant;
using System.Xml.Linq;

namespace API_Migrate.Model
{
    public class HDA
    { 
        private static Contanst contanst = new Contanst();
        private string path_api;
        private string query;
        private string controller;
        private string store;
        public string Path_api { get => path_api; set => path_api = value; }
        public string Query { get => query;  }
        public string Controller { get => controller; set => controller = value; }
        public string Store { get => store; set => store = value; }

        public HDA()
        { 
            Path_api = "api/Egas/NXH/Transaction/GetEgasDataExport";
            this.controller = "HDA";
            this.Store = "api_SVTran";
            string path = contanst.Controller_path + "/" + this.controller + ".xml";
            this.query = contanst.ExtractSelectQuery(path);
        }
        

        public class Root
        {
            public string code { get; set; }
            public string message { get; set; }
            public Data data { get; set; }
            public Root() { }
        }

        public class Data
        {
            public int totalRecord { get; set; }
            public Syncmetadata[] syncMetaData { get; set; }
            public Datum[] data { get; set; }
        }

        public class Syncmetadata
        {
            public string pos { get; set; }
            public string maxSysDClient { get; set; }
        }

        public class Datum
        {
            public long transactionId { get; set; }
            public string transactionNo { get; set; }
            public string transactionDate { get; set; }
            public string transactionType { get; set; }
            public string stationCode { get; set; }
            public string stationName { get; set; }
            public string customerCode { get; set; }
            public string customerName { get; set; }
            public string warehouseCode { get; set; }
            public string warehouseName { get; set; }
            public string debitAcct { get; set; }
            public string creditAcct { get; set; }
            public string taxAcct { get; set; }
            public string paymentType { get; set; }
            public string invoiceStatus { get; set; }
            public string invoiceForm { get; set; }
            public string invoiceSerial { get; set; }
            public string invoiceNo { get; set; }
            public object invoiceCode { get; set; }
            public string invoiceDate { get; set; }
            public string currencyCode { get; set; }
            public int exchangeRate { get; set; }
            public string invoiceType { get; set; }
            public object buyerName { get; set; }
            public object buyerTaxCode { get; set; }
            public object buyerAddress { get; set; }
            public object buyerEmail { get; set; }
            public object createdStaff { get; set; }
            public int invoiceVATRate { get; set; }
            public float invoicePreTaxAmount { get; set; }
            public float invoiceTaxAmount { get; set; }
            public float invoiceTotalAmount { get; set; }
            public string createdUser { get; set; }
            public object invoiceRemarks { get; set; }
            public int sysDate { get; set; }
            public long i { get; set; }
            public Product[] products { get; set; }
        }

        public class Product
        {
            public int rowNumber { get; set; }
            public string productCode { get; set; }
            public string productName { get; set; }
            public string unitCode { get; set; }
            public string unitName { get; set; }
            public float preTaxUnitPrice { get; set; }
            public float unitPrice { get; set; }
            public float quantity { get; set; }
            public float preTaxAmount { get; set; }
            public float taxAmount { get; set; }
            public float totalAmount { get; set; }
            public float vatRate { get; set; }
            public object remarks { get; set; }
            public int rowNum { get; set; }
        }
         
    }
}
