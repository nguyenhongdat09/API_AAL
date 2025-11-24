using API_Migrate.Database_Handle;
using API_Migrate.Model;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;
using System.Xml.Serialization;

namespace API_Migrate.API
{
    internal class setDX2 : API_Method, IController
    {
        private DX2SetModel dx2SetModel = new DX2SetModel();
        private DX2SetModel.Root root;
        public DX2SetModel DX2SetModel { get => dx2SetModel; set => dx2SetModel = value; }
        public DX2SetModel.Root Root { get => root; set => root = value; }
        QuerySQLServer queryMan = new QuerySQLServer();
        public int Required_token_yn { get; set; }
        public string Query { get; set; }
        public bool Execute_SQL_After_API_yn { get; set; }
       
        
        public setDX2()
        {
            this.Controller = dx2SetModel.Controller;
            this.Required_token_yn = 1;
            this.Query = dx2SetModel.Query;
            this.Execute_SQL_After_API_yn = false;
            Logging.Controller = dx2SetModel.Controller;
            queryMan.Controller = dx2SetModel.Controller;
        }

        private void Receive(string response)
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
                query.Query_PushToReceiveTable(ID, serializedXml);
            }
            string dataAsString = JsonConvert.SerializeObject(data_struct);
            this.Logging.Description = dataAsString;
        }

        public async Task request(string urlbase, string token)
         {
            if (string.IsNullOrEmpty(token))
            {
                Logging.Description = "Token is null or empty";
                return;
            }
            List<dynamic> table_filter = await queryMan.Query_GetFilter(dx2SetModel.Store);
            var data = table_filter.Select(row => new
            {
                SoBill = row.SoBill,
                NgayNhap = row.NgayNhap,
                NgayGui = row.NgayGui,
                MaKhNhan = row.MaKhNhan,
                TenKhNhan = row.TenKhNhan,
                DiaChiNhan = row.DiaChiNhan,
                DiDongNhan = row.DiDongNhan,
                DienThoaiNhan = row.DienThoaiNhan,
                TinhThanhNhan = row.TinhThanhNhan,
                QuanHuyenNhan = row.QuanHuyenNhan,
                DienGiai = row.DienGiai,
                SoKien = row.SoKien,
                TinhCuoc = row.TinhCuoc,
                TongTien = row.TongTien,
                TienCOD = row.TienCOD,
                PhiCOD = row.PhiCOD,
                ThucTe = row.ThucTe
            }).FirstOrDefault();// 👈 Chỉ lấy 1 object, không gửi cả danh sách khi nào gửi danh sách thì dùng Tolist())
            if (data == null)
                return;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = System.Text.Json.JsonSerializer.Serialize(data, options);
            string url = urlbase + this.dx2SetModel.Path_api;
            string response = await PostAsync(url, data, token);
            this.Logging.Description = jsonString;
            if (response == "") return;
            Receive(response);
        }
        

        public void processToListView( ListView lstView)
        {
            if (root is null) return;
            var tran = this.Controller;
            ListViewItem item2 = new ListViewItem($"{tran} is Done " + DateTime.Now.ToString("HH:mm:ss"));
            lstView.Items.Insert(0, item2);
        }

    }
}
