using Newtonsoft.Json;
using System.Threading.Tasks;
using API_Migrate.Model;
using API_Migrate.Config;

namespace API_Migrate.API
{
    internal class getToken : API_Method
    {
        private Token.Root root;
        private Token token;
        public getToken() {
            this.token = new Token();
            this.root =  new Token.Root();
            this.Controller = "*";
        }

        public Token.Root? Root
        {
            get => root;
            set => root = value;
        }
        public Token Token { get => token; set => token = value; }

        public async Task request_login(string urlbase, string user, string pass)
        {
            var data = new Dictionary<string, string>
            {
                { "username", user },
                { "password", pass },
                { "grant_type", "password" }
            };

            string url = urlbase + this.token.Path_api;

            string response = await PostAsyncFormUrlEncodedContent(url, data);

            if (!string.IsNullOrEmpty(this.Config.Token.Value))
            {
                this.Root.access_token = this.Config.Token.Value;
                return;
            }

            this.Root = JsonConvert.DeserializeObject<Token.Root>(response);
        }

    }
}
