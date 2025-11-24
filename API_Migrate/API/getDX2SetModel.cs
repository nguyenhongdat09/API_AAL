using API_Migrate.Database_Handle;
using API_Migrate.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace API_Migrate.API
{
    internal class getDX2SetModel : API_Method, IController
    {
        private DX2SetModel dx2SetModel = new DX2SetModel();
        private DX2SetModel.Root root;
        public DX2SetModel.Root Root { get => root; set => root = value; }
        QuerySQLServer queryMan = new QuerySQLServer();
        public string Query { get; set; }
        public int Required_token_yn { get; set; }
        public bool Execute_SQL_After_API_yn { get; set; }
        public getDX2SetModel()
        {
            this.Controller = dx2SetModel.Controller;
            this.Required_token_yn = 0;
            this.Query = dx2SetModel.Query;
            this.Execute_SQL_After_API_yn = true;
            Logging.Controller = dx2SetModel.Controller;
            queryMan.Controller = dx2SetModel.Controller;
        }

        private async Task<List<Dictionary<string, string>>> getdx2StatusBillToDict()
        {
            List<dynamic> table_filter = await queryMan.Query_GetFilter(dx2SetModel.Store);
            var data = table_filter.Select(row => new
            {
                SoBill = row.so_ct,
                Stt_rec_bill = row.stt_rec,
                Ticket = row.ticket
            }).ToList();

            if (data.Count == 0) return new List<Dictionary<string, string>>();

            var result = new List<Dictionary<string, string>>();

            foreach (var item in data)
            {
                var dict = new Dictionary<string, string>();
                foreach (var prop in item.GetType().GetProperties())
                {
                    var value = prop.GetValue(item);
                    dict[prop.Name] = value?.ToString() ?? "";
                }
                result.Add(dict);
            }

            return result;
        }

        public void Receive(string response, string SoBill, string stt_rec_bill, string ticket)
        {
            this.Root = JsonConvert.DeserializeObject<DX2SetModel.Root>(response);
            var data_struct = this.Root;
            root = this.Root;
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer x = new XmlSerializer(data_struct.GetType());
                x.Serialize(writer, data_struct);
                string serializedXml = writer.ToString();
                string ID = DateTime.Now.ToString("ddMMyyyyHHmmss");
                QuerySQLServer query = new QuerySQLServer(this.Controller);
                queryMan.Query_PushToReceiveTableBillStatus(ID, serializedXml, SoBill, stt_rec_bill, ticket);
            }
            string dataAsString = JsonConvert.SerializeObject(data_struct);
            this.Logging.Description = dataAsString;
        }
        public async Task request(string urlbase, string token)
        {
            try
            {
                string url = urlbase + this.dx2SetModel.Path_api;
                List<Dictionary<string, string>> dicts = await this.getdx2StatusBillToDict();
                foreach (var dict in dicts)
                {
                    if (dict == null || !dict.ContainsKey("SoBill"))
                        continue;
                    string SoBill = dict["SoBill"];
                    string stt_rec_bill = dict.ContainsKey("Stt_rec_bill") ? dict["Stt_rec_bill"] : "";
                    string ticket = dict.ContainsKey("Ticket") ? dict["Ticket"] : "";
                    var onlySoBillDict = new Dictionary<string, string>
                    {
                        { "SoBill", SoBill }
                    };

                    string response = await GetAsyncWithParams(url, onlySoBillDict);
                    if (string.IsNullOrWhiteSpace(response)) continue;
                    Receive(response, SoBill, stt_rec_bill, ticket);
                }
            }
            catch (Exception ex)
            {
                this.Logging.Description = ex.ToString();
            }
        }

        public void processToListView(ListView lstView)
        {
            if (root is null) return;
            //var tran = root.Data[0].Ten_Quan.ToString();
            var tran = this.Controller;
            ListViewItem item2 = new ListViewItem($"{tran} is Done" + DateTime.Now.ToString("HH:mm:ss"));
            lstView.Items.Insert(0, item2);
        }

    }
}
