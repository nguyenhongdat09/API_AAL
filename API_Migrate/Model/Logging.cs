using API_Migrate.Constant;
using System.Text;

namespace API_Migrate.Model
{
    public class Logging
    {
        private static Contanst contanst = new Contanst();
        private string description;
        private DateTime date;
        public string[] folder_logging;
        private string controller = string.Empty;
        public Logging()
        {
        }
        public string Description { get => description; 
            set { 
                description = value;
                date = DateTime.Now;
                if (this.controller == "*") {
                    foreach (string controller in folder_logging)
                    {
                        writeLog(controller);
                    }
                }
                else
                {
                    writeLog(this.controller);
                }
                
            } 
        }
        public DateTime Date { get => date;}
        public string Controller { get => controller; set => controller = value; }
        public void createFolderLogging()
        {
            string path = "";
            foreach (string folder in folder_logging)
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                path = Path.Combine(currentDirectory, @"Logs", folder);
                Directory.CreateDirectory(path);
            }
        }
        private static readonly object _logLock = new object();

        private void writeLog(string controller)
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine(currentDirectory, @"Logs", controller);
            string path = Path.Combine(folderPath, date);

            lock (_logLock)
            {
                try
                {
                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    // Tạo file nếu chưa tồn tại
                    if (!File.Exists(path))
                    {
                        using (File.Create(path)) { }
                    }

                    // Mở file để ghi, seek tới cuối
                    using (var file = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.Read)) // FileShare.Read tránh deadlock
                    using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                    {
                        writer.BaseStream.Seek(0, SeekOrigin.End);
                        string log_text = this.date.ToString("dd/MM/yyyy HH:mm:ss") + ": " + Environment.NewLine + this.description;
                        writer.WriteLine(log_text);
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi ra debug hoặc nơi khác nếu cần
                    Console.WriteLine($"Log write failed: {ex.Message}");
                }
            }
        }

    }
}
