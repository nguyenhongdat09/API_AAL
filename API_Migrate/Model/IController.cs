namespace API_Migrate.Model
{
    public interface IController
    {
         Task request(string urlbase, string token);
         void processToListView(ListView item);
         string Controller { get; set; }
         int Required_token_yn { get; set; }
         string Query { get; set; }
            
         bool Execute_SQL_After_API_yn { get; set; } //Có execute SQL khi chạy API hay không

         Logging Logging { get; set; }
    }

}
