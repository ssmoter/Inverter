using System.Text;

namespace Inverter.Data
{
    public class FileManager
    {
        private string _filePathInverter;
        private string _filePathData;


        public FileManager()
        {

        }


        private void DeleteFile()
        {
            if (System.IO.File.Exists(_filePathInverter))
            {
                System.IO.File.Delete(_filePathInverter);
            }
        }

        public async void NewFile(string inverterModel)
        {
            if (System.IO.File.Exists(_filePathInverter))
            {
                System.IO.File.Delete(_filePathInverter);
            }
            try
            {
                using (System.IO.FileStream fs = System.IO.File.Create(_filePathInverter))
                {
                    StreamWriter writer = new(fs);
                    await writer.WriteLineAsync(inverterModel);
                    writer.Close();
                    writer.Dispose();
                }
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Creating file failed\n" + ex.Message);
            }
        }

        public async Task<string> OpenFile(string OpenFilePath)
        {
            if (!System.IO.File.Exists(_filePathData))
            {
                throw new IOException("Nie znaleziono pliku");
            }
            try
            {
                string line;
                using (StreamReader sr = new StreamReader(_filePathData, Encoding.UTF8))
                {
                    line = await sr.ReadToEndAsync();
                }
                  DeleteFile();
                return line;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:File Opening falied\n" + ex.Message);
            }
        }
    }
}
