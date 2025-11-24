using API_Migrate.Database_Handle;
using API_Migrate.Model;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace API_Migrate.API
{
    internal class getThanhPho : API_Method, IController
    {
        private ThanhPho thanh_pho = new ThanhPho();
        private ThanhPho.Root root; 
        public ThanhPho.Root Root { get => root; set => root = value; }
        QuerySQLServer queryMan = new QuerySQLServer(); 
        public string Query { get; set; }
        public int Required_token_yn { get; set; }
        public bool Execute_SQL_After_API_yn { get; set; }
        public getThanhPho()
        {
            this.Controller = thanh_pho.Controller;
            this.Required_token_yn = 0;
            this.Query = thanh_pho.Query;
            this.Execute_SQL_After_API_yn = true;
            Logging.Controller = thanh_pho.Controller;
            queryMan.Controller = thanh_pho.Controller;
        }

        private async Task<bool> checkSync()
        {
            List<dynamic> table_filter = await queryMan.Query_GetFilter(thanh_pho.Store);
            var data = table_filter.Select(row => new
            {
                Controller = row.controller,
            }).ToList();
            return data.Count == 0;
        }

        public void Receive(string response)
        {
            this.Root = JsonConvert.DeserializeObject<ThanhPho.Root>(response);
            root = this.Root;
            var data_struct = this.Root;

            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer x = new XmlSerializer(data_struct.GetType());
                x.Serialize(writer, data_struct);
                string serializedXml = writer.ToString();
                string ID = DateTime.Now.ToString("ddMMyyyyHHmmss");
                queryMan.Query_PushToReceiveTable(ID, serializedXml);
            }
            string dataAsString = JsonConvert.SerializeObject(data_struct);
            this.Logging.Description = dataAsString;
        }


        public async Task request(string urlbase, string token)
        {
            try
            {
                //=> Check Store xem co yeu cau dong bo khong
                if (await this.checkSync())
                    return;

                string url = urlbase + this.thanh_pho.Path_api;
                string response = await GetAsyncWithNoParams(url);
                if (response == "") return;
                Receive(response); // 👈 Gọi hàm mới
            }
            catch (Exception ex)
            {
                this.Logging.Description = ex.ToString();
            }
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
