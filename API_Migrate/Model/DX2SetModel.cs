using API_Migrate.Constant;
using System.Xml.Linq;

namespace API_Migrate.Model
{
    public class DX2SetModel
    {
        private static Contanst contanst = new Contanst();
        private string path_api;
        private string query;
        private string controller;
        private string store;
        public string Path_api { get => path_api; set => path_api = value; }
        public string Query { get => query; }
        public string Controller { get => controller; set => controller = value; }
        public string Store { get => store; set => store = value; }

        public DX2SetModel()
        {
            Path_api = "api/Bill/Insert";
            this.controller = "DX2Set";
            this.Store = "api_SITran";
            string path = contanst.Controller_path + "/" + this.controller + ".xml";
        }


        public class Root
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public Datum[] Data { get; set; }
        }

        public class Datum
        {
            public int Id { get; set; }
            public string So_Bill { get; set; }
            public string Ma_Kh_Gui { get; set; }
            public string Ten_Kh_Gui { get; set; }
            public string Dia_Chi_Gui { get; set; }
            public string Ma_Kh_Nhan { get; set; }
            public string Ten_Kh_Nhan { get; set; }
            public string Dia_Chi_Nhan { get; set; }
            public string Dien_Giai { get; set; }
            public float So_Kien { get; set; }
            public float Tinh_Cuoc { get; set; }
            public float Cuoc_Phi { get; set; }
            public float Tong_Tien { get; set; }
            public float VAT { get; set; }
            public float Thuc_Thu { get; set; }
            public int Hinh_Thuc_Tt { get; set; }
        }


    }
}
