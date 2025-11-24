using API_Migrate.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Migrate.Model
{
    public class DX2Status
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

        public DX2Status()
        {
            Path_api = "api/Bill/GetBillStatus";
            this.controller = "DX2Status";
            this.Store = "api_DX2Status";
            string path = contanst.Controller_path + "/" + this.controller + ".xml";
            this.query = contanst.ExtractSelectQuery(path);
        }


        public class Root
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public Datum[] Data { get; set; }
        }

        public class Datum
        {
            public string Ngay_Tao { get; set; }
            public string Gio_Tao { get; set; }
            public string Ma_Trang_Thai { get; set; }
            public string Ten_Trang_Thai { get; set; }
            public string Vi_Tri { get; set; }
            public string Ghi_Chu { get; set; }
        }


    }
}
