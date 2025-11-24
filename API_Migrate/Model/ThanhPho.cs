using API_Migrate.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Migrate.Model
{
    public class ThanhPho
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

        public ThanhPho()
        {
            Path_api = "api/ThanhPho/GetAll";
            this.controller = "ThanhPho";
            this.Store = "api_ThanhPho"; 
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
            public int Id_Tp { get; set; }
            public string Ky_Hieu { get; set; }
            public string Ten_ThanhPho { get; set; }
        }

    }
}
