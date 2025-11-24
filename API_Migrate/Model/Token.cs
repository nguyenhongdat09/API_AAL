using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Migrate.Model
{
    public class Token
    {
        private string path_api;
        public string Path_api { get => path_api; set => path_api = value; }
        public Token()
        {
            Path_api = "token";
        }

        public class Data
        {
            public string token { get; set; } = string.Empty;
        }

        public class Root
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }
 
    }

}
