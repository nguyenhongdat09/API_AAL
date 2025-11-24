using API_Migrate.Config;
using API_Migrate.Constant;
using API_Migrate.Model;
using Dapper; 
using System.Data;
using System.Data.SqlClient; 
namespace API_Migrate.Database_Handle
{
    internal class QuerySQLServer
    {
        private Configuration config = new SetConfig().Config.Configuration;
        private static Contanst contanst = new Contanst();
        private string controller;
        Logging logging = new Logging();
        public string Controller { get => controller; set => controller = value; }

        public QuerySQLServer() { 
            
        }
        public QuerySQLServer(string controller)
        {
            this.controller = controller;
        } 
        public void Query_PushToReceiveTable(string ID, string serializedXml, string id_tp = "")
        { 
            logging.Controller = this.Controller;
           
            using (IDbConnection conn = new SqlConnection(config.AppConnectionString.Value))
            {
                string query = $"INSERT INTO {contanst.Table_ReceiveData} (ID, controller, data_api, ghi_chu) VALUES (@ID, @Controller, @XmlData, @ThanhPhoID)";
                conn.Execute(query, new
                {
                    ID,
                    Controller = this.controller,
                    XmlData = serializedXml,
                    ThanhPhoID = id_tp
                });
            }
        }

        public void Query_PushToReceiveTableBillStatus(string ID, string serializedXml, string SoBill, string stt_rec_bill, string ticket = "")
        {
            logging.Controller = this.Controller;

            using (IDbConnection conn = new SqlConnection(config.AppConnectionString.Value))
            {
                string query = $"INSERT INTO {contanst.Table_ReceiveData} (ID, controller, data_api, ghi_chu) VALUES (@ID, @Controller, @XmlData, @ghi_chu)";
                string Ghi_chu = $"{SoBill}#{stt_rec_bill}" + ticket == "" ? $"{ticket}" : "";
                conn.Execute(query, new
                {
                    ID,
                    Controller = this.controller,
                    XmlData = serializedXml,
                    ghi_chu = Ghi_chu
                });
            }
        }

        public async Task<IEnumerable<IDictionary<string, object>>> Query_Controller(string query)
        {
            if (query == "")
                return null;
            logging.Controller = this.Controller;
            using (IDbConnection conn = new SqlConnection(config.AppConnectionString.Value))
            {
                query = query
                    .Replace("@@controller", $"'{this.controller}'")
                    .Replace("@@user_id", $"'{config.UserDebug.Value.ToString()}'");
                var result =  await conn.QueryAsync(query);
                    return result.Select(row => (IDictionary<string, object>)row);
            }
        }
        //Chạy store đẩy điều kiện lọc vào các controller
        public async Task Query_CheckSync()
        {
            logging.Controller = this.Controller;

            using (IDbConnection conn = new SqlConnection(config.AppConnectionString.Value))
            {
                string query = "exec "+ contanst.Name_StoreCheckSync + "";
                // Thực thi stored procedure với QueryMultipleAsync
                var reader = await conn.QueryMultipleAsync(
                    query,
                    param: null,  
                    commandType: CommandType.Text
                ); 
                
            }
        }
        //Chạy store lấy ra điều kiện lọc từ các controller
        public async Task<List<dynamic>> Query_GetFilter(string store)
        {
            
            logging.Controller = this.Controller;
            using (IDbConnection conn = new SqlConnection(config.AppConnectionString.Value))
            {
                string query = "exec " + store + "";
                // Thực thi stored procedure với QueryMultipleAsync
                var reader = await conn.QueryMultipleAsync(
                    query,
                    param: null,
                    commandType: CommandType.Text
                );
                var result = (await reader.ReadAsync<dynamic>()).ToList();
                return result; 
            } 
        }
 

    }
}
