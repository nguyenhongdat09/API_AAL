using API_Migrate.Database_Handle;
using API_Migrate.Model;
using Newtonsoft.Json; 
using System.Xml.Serialization;

namespace API_Migrate.API
{
    internal class getDX2Status : API_Method, IController
    {
        private DX2Status dx2status = new DX2Status();
        private DX2Status.Root root;
        public DX2Status.Root Root { get => root; set => root = value; }
        QuerySQLServer queryMan = new QuerySQLServer();

        public int Required_token_yn { get; set; }
        public string Query { get; set; }
        public bool Execute_SQL_After_API_yn { get; set; }
 
        public getDX2Status()
        {
            this.Controller = dx2status.Controller;
            this.Required_token_yn = 0;
            this.Query = dx2status.Query;
            this.Execute_SQL_After_API_yn = true;
            Logging.Controller = dx2status.Controller;
            queryMan.Controller = dx2status.Controller;
        }

        //get Condition To Sync
        private async Task<List<Dictionary<string, string>>> getdx2StatusBillToDict()
        {
            List<dynamic> table_filter = await queryMan.Query_GetFilter(dx2status.Store);
            var data = table_filter.Select(row => new
            {
                SoBill = row.so_bill,
                Stt_rec_bill = row.stt_rec_bill,
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
        public async Task request(string urlbase, string token)
        {
            try
            {
                string url = urlbase + this.dx2status.Path_api;
                List<Dictionary<string, string>> dicts = await this.getdx2StatusBillToDict();
                foreach (var dict in dicts)
                {
                    if (dict == null || !dict.ContainsKey("SoBill"))
                        continue;
                    string SoBill = dict["SoBill"];
                    string stt_rec_bill = dict.ContainsKey("Stt_rec_bill") ? dict["Stt_rec_bill"] : "";
                    var onlySoBillDict = new Dictionary<string, string>
                    {
                        { "SoBill", SoBill }
                    };

                    string response = await GetAsyncWithParams(url, onlySoBillDict);
                    if (string.IsNullOrWhiteSpace(response)) continue;
                    Receive(response, SoBill, stt_rec_bill);
                }
            }
            catch (Exception ex)
            {
                this.Logging.Description = ex.ToString();
            }
        }

        //Receive The result From sync
        public void Receive(string response, string SoBill, string stt_rec_bill)
        {
            this.Root = JsonConvert.DeserializeObject<DX2Status.Root>(response);
            var data_struct = this.Root;
            root = this.Root;
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer x = new XmlSerializer(data_struct.GetType());
                x.Serialize(writer, data_struct);
                string serializedXml = writer.ToString();
                string ID = DateTime.Now.ToString("ddMMyyyyHHmmss");
                QuerySQLServer query = new QuerySQLServer(this.Controller);
                queryMan.Query_PushToReceiveTableBillStatus(ID, serializedXml, SoBill, stt_rec_bill);
            }
            string dataAsString = JsonConvert.SerializeObject(data_struct);
            this.Logging.Description = dataAsString;
        }


        public void processToListView(ListView lstView)
        {
            if (root is null) return;
            //var tran = root.Data[0].Ky_Hieu.ToString();
            var tran = this.Controller;
            ListViewItem item2 = new ListViewItem($"{tran} is Done" + DateTime.Now.ToString("HH:mm:ss"));
            lstView.Items.Insert(0, item2);
        }

    }
}
