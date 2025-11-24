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
    internal class getQuanHuyen : API_Method, IController
    {
        private QuanHuyen quan_huyen = new QuanHuyen();
        private QuanHuyen.Root root; 
        public QuanHuyen.Root Root { get => root; set => root = value; }
        QuerySQLServer queryMan = new QuerySQLServer();
        public string Query { get; set; }
        public int Required_token_yn { get; set; }
        public bool Execute_SQL_After_API_yn { get; set; }
        public getQuanHuyen()
        {
            this.Controller = quan_huyen.Controller;
            this.Required_token_yn = 0;
            this.Query = quan_huyen.Query;
            this.Execute_SQL_After_API_yn = true;
            Logging.Controller = quan_huyen.Controller;
            queryMan.Controller = quan_huyen.Controller;
        }

        private async Task<Dictionary<string, string>> getThanPhoToDict()
        {
            List<dynamic> table_filter = await queryMan.Query_GetFilter(quan_huyen.Store);
            var data = table_filter.Select(row => new
            {
                ThanhPhoId = row.id_tp,
            }).ToList();

            if (data.Count == 0) return null;
            var firstItem = data.First();
            var dict = new Dictionary<string, string>();
            foreach (var prop in firstItem.GetType().GetProperties())
            {
                var value = prop.GetValue(firstItem);
                dict[prop.Name] = value?.ToString() ?? "";
            }
            return dict;
        }
        public void Receive(string response, string thanhPhoId)
        {
            this.Root = JsonConvert.DeserializeObject<QuanHuyen.Root>(response);
            var data_struct = this.Root;
            root = this.Root;
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer x = new XmlSerializer(data_struct.GetType());
                x.Serialize(writer, data_struct);
                string serializedXml = writer.ToString();
                string ID = DateTime.Now.ToString("ddMMyyyyHHmmss");
                QuerySQLServer query = new QuerySQLServer(this.Controller);
                queryMan.Query_PushToReceiveTable(ID, serializedXml, thanhPhoId);
            }
            string dataAsString = JsonConvert.SerializeObject(data_struct);
            this.Logging.Description = dataAsString;
        }
        public async Task request(string urlbase, string token)
        {
            try
            {
                string url = urlbase + this.quan_huyen.Path_api; 
                Dictionary<String, String> dict = await this.getThanPhoToDict();
                string thanhPhoId = "";
                if (dict != null && dict.ContainsKey("ThanhPhoId"))
                {
                    thanhPhoId = dict["ThanhPhoId"];
                }
                else
                {
                    return;
                }
                string response = "";
                response = await GetAsyncWithParams(url, dict); 
                if (response == "") return;
                Receive(response, thanhPhoId);
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
