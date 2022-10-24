using System.Text;

namespace Inverter.Data
{
    public class FileManager
    {
        public string FileName = @"Model";
        public string FilePathData;
        string path;


        public FileManager()
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += @"\Inverter";

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }
        public async Task<bool> NewFile(string inverterModel)
        {
            try
            {
                FilePathData = path + @"\" + FileName + @".cir";

                using (System.IO.FileStream fs = System.IO.File.Create(FilePathData))
                {
                    StreamWriter writer = new(fs);
                    await writer.WriteLineAsync(inverterModel);
                    writer.Close();
                    writer.Dispose();
                }
                return true;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Creating file failed\n" + ex.Message);
            }
        }

        public async Task<string> OpenFile()
        {
            if (!System.IO.File.Exists(path + @"\" + FileName + @".out"))
            {
                throw new IOException("Nie znaleziono pliku");
            }
            try
            {
                string line;
                using (StreamReader sr = new StreamReader(path + @"\" + FileName + @".out", Encoding.UTF8))
                {
                    line =  sr.ReadToEnd();
                }
              //  System.IO.File.Delete(path + @"\" + FileName + @".out");

                return line;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:File Opening falied\n" + ex.Message);
            }
        }
    }
}
