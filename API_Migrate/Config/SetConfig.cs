using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.Emit;
using System.Net;

namespace API_Migrate.Config
{
    internal class SetConfig
    {
        private ConfigStruct config;
        public SetConfig() { 
            this.config = _setConfig();
        }

        public ConfigStruct Config { get => config; set => config = value; }
        private ConfigStruct _setConfig()
        {
            ConfigStruct _config;
            string path_config = @".\Configure\Config.json";
            string json = File.ReadAllText(path_config);
            _config = JsonConvert.DeserializeObject<ConfigStruct>(json)!;
            return _config;
        }
    }
}
