using API_Migrate.Config;
using API_Migrate.Model;
using API_Migrate.API;
using API_Migrate.Database_Handle;
using System.Data; 

namespace API_Migrate
{
    public partial class FrmMain : Form
    {
        List<IController> controllers = new List<IController>
        {
          
            new getThanhPho(),
            new getQuanHuyen(),
            new setDX2(), 
            new getDX2Status()
        };
        Configuration config = new SetConfig().Config.Configuration;
        Token.Root token_root = new Token.Root();
        Logging logging = new Logging();
        QuerySQLServer queryMan = new QuerySQLServer();
        private void FrmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }
        private void customeWinform()
        {
            this.Resize += new System.EventHandler(this.FrmMain_Resize);
            lblTitle.Text = config.TitleTools.Value.ToString();
            lblTitle.Visible = true;
            lstID.Visible = false;
            lstToken.Visible = false;
            notifyIcon1.Text = config.TitleTools.Value.ToString();
            notifyIcon1.Visible = true;
        }
        public FrmMain()
        {
            InitializeComponent();
            customeWinform();

        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            var sencond = Int32.Parse(config.TokenTimeOut.Value);
            CreateLogFolder();
            Task token = PeriodicAsync(async () =>
            {
                await GetToken();
            }, TimeSpan.FromSeconds(sencond));

            var sencond_API = Int32.Parse(config.TimeInterval.Value);
            var sencond_SQL = Int32.Parse(config.TimeIntervalSQL.Value);
            Run_API(sencond_API, sencond_SQL);
        }

        private void CreateLogFolder()
        {
            logging.folder_logging = controllers.Select(c => c.Controller).ToArray();
            logging.createFolderLogging();
        }

        private async Task GetToken()
        {
            try
            {
                string urlbase = config.ApiURL.Value;
                string username = config.UserName.Value;
                string password = config.UserPass.Value;
                getToken getToken = new getToken();
                await getToken.request_login(urlbase, username, password);
                if (getToken.Root is null) return;
                token_root = getToken.Root;
                ListViewItem item = new ListViewItem(token_root.access_token.Substring(token_root.access_token.Length - 3));
                lstToken.Items.Insert(0, item);
                logging.Controller = "*";
                logging.Description = $"Token: {token_root.access_token}";
            }
            catch (Exception ex)
            {
                logging.Description = ex.ToString();
            }
        }

        public void Run_API(int sencond_API, int sencond_SQL)
        {

            /*
                 => Lay ra thanh pho 
                 => Co thanh pho xong lay ra quan huyen
                 
             */
            foreach (var controller in controllers)
            {
                Task callApi = PeriodicAsync(async () =>
                {
                    await API_Calling(controller);
                    
                    //Call Url xong, Day vao bang api_receive 
                    // Xong chay xuong ExecuteControllerAsync tim data trong api_receive day vao FAST
                }, TimeSpan.FromSeconds(sencond_API));
                
                //Nếu muốn luồng sql song song với luồng gọi API thì nhả ra trong case này thì cần tuần tự Vì ThanhPho là chạy thủ công gọi mới chạy
                //Tách ra 2 luồng song song callApi chạy rồi insertToTable chạy lập tức mà không cần đợi callApi xong
                Task insertToTable = PeriodicAsync(async () =>
                {
                    await ExecuteControllerAsync(controller);
                }, TimeSpan.FromSeconds(sencond_SQL));
               

            }

        }
        private async Task ExecuteControllerAsync(IController controller)
        {
            try
            {
                if (!controller.Execute_SQL_After_API_yn)
                    return;
                queryMan.Controller = controller.Controller;
                var result = await queryMan.Query_Controller(controller.Query);
                if (!result.Any()) return;
                string col = string.Join(" ; ", result.First().Keys);
                string rows = string.Join(": ", result.Select(row => string.Join(" ; ", row.Values)));
                controller.Logging.Description = "Message: " + rows;
            }
            catch (Exception ex)
            {
                controller.Logging.Description = ex.Message;
            }
        }
        private async Task API_Calling(IController controller)
        {
            try
            {
                var token = token_root.access_token;
                if (string.IsNullOrEmpty(token) && controller.Required_token_yn == 1) return;
                ListViewItem item = new ListViewItem($"{controller.Controller} is Running " + DateTime.Now.ToString("HH:mm:ss"));
                lstID.Items.Insert(0, item);
                string urlbase = config.ApiURL.Value;
                await controller.request(urlbase, token);
                controller.processToListView(lstID);
            }
            catch (Exception ex)
            {
                controller.Logging.Description = ex.ToString();
            }
        }
        public static async Task PeriodicAsync(Func<Task> action, TimeSpan interval, CancellationToken cancellationToken = default)
        {
            //cancellationToken Dùng để hủy tác vụ chủ động
            while (true)
            {
                Task delayTask = Task.Delay(interval, cancellationToken);
                await action();//Đợi đến khi action xong rồi mới delayTask
                await delayTask;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

    }
}