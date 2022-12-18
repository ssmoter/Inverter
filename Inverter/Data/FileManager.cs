using Inverter.Helpers;
using System.Text;

namespace Inverter.Data
{
    public class FileManager
    {
        private const string FileName = @"Model";
        public string FilePathData;
        public string path { get; private set; }
        private const string configName = "config";

        public FileManager()
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += @"\Inverter";

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }



        public async Task<bool> CreateConfig(string configValue, MyEnums.configName configName)
        {
            try
            {
                string configPath = path + "\\config.txt";
                if (!System.IO.File.Exists(configPath))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(configPath))
                    {
                        StreamWriter writer = new(fs);
                        await writer.WriteLineAsync(configName + " " + configValue);
                        writer.Close();
                        writer.Dispose();
                    }
                }
                else
                {
                    string line = string.Empty;
                    using (StreamReader sr = new StreamReader(configPath))
                    {
                        line = sr.ReadToEnd();
                        var lines = line.Split('\r', '\n').ToList();
                        line = string.Empty;
                        for (int i = 0; i < lines.Count; i++)
                        {
                            if (!lines[i].Contains(configName.ToString()))
                            {
                                if (!string.IsNullOrWhiteSpace(lines[i]))
                                {
                                    line += lines[i];
                                    line += Environment.NewLine;
                                }
                            }
                        }


                        line += configName + " " + configValue;
                    }
                    using (System.IO.FileStream fs = System.IO.File.Create(configPath))
                    {
                        StreamWriter writer = new(fs);
                        await writer.WriteLineAsync(line);
                        writer.Close();
                        writer.Dispose();
                    }
                }
                return true;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Tworzenie pliku nie powiodło się" + Environment.NewLine + ex.Message);
            }
        }
        public string GetConfig(MyEnums.configName configName)
        {
            try
            {
                string configPath = path + "\\config.txt";
                if (!System.IO.File.Exists(configPath))
                {
                    return null;
                }
                using (StreamReader sr = new StreamReader(configPath))
                {
                    string line = sr.ReadToEnd();

                    var lines = line.Split('\r', '\n').ToList();

                    foreach (var item in lines)
                    {
                        if (item.Contains(configName.ToString()))
                        {
                            return item.Remove(0, configName.ToString().Length);
                        }
                    }
                }
                return null;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Wczytywanie konfiguracji nie powiodło się" + Environment.NewLine + ex.Message);
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
                throw new IOException("Error:Tworzenie pliku nie powiodło się" + Environment.NewLine + ex.Message);
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
                    line = sr.ReadToEnd();
                }
                return line;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Wczytywanie pliku nie powiodło się" + Environment.NewLine + ex.Message);
            }
        }

        public List<string> GetFilesName()
        {
            try
            {
                List<string> fileEntries = Directory.GetFiles(path).ToList();

                fileEntries.Remove(fileEntries.FirstOrDefault(x => x.Contains(FileName)));
                fileEntries.Remove(fileEntries.FirstOrDefault(x => x.Contains(FileName)));
                fileEntries.Remove(fileEntries.FirstOrDefault(x => x.Contains(configName)));

                return fileEntries;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Wczytywanie plików nie powiodło się" + Environment.NewLine + ex.Message);
            }
        }

        public bool DeleteFile(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                if (!System.IO.File.Exists(filePath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Usuwanie pliku nie powiodło się" + Environment.NewLine + ex.Message);
            }

        }

        public bool ExistFile(string name)
        {
            try
            {
                string filePath = path + @"\" + name + ".txt";
                if (!System.IO.File.Exists(filePath))
                {
                    return true;
                }
                return false;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Wystąpił błąd przy sprawdzaniu czy plik istnieje" + Environment.NewLine + ex.Message);
            }
        }
        public async Task<bool> SaveNewData(string name, string data)
        {
            try
            {
                string filePath = path + @"\" + name + ".txt";
                using (System.IO.FileStream fs = System.IO.File.Create(filePath))
                {
                    StreamWriter writer = new(fs);
                    await writer.WriteLineAsync(data);
                    writer.Close();
                    writer.Dispose();
                }
                if (System.IO.File.Exists(filePath))
                {
                    return true;
                }
                return false;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Nie udało zapisać się pliku" + Environment.NewLine + ex.Message);
            }
        }

        public async Task<string> LoadDataName(string name)
        {
            try
            {
                string filePath = path + @"\" + name + ".txt";
                if (!System.IO.File.Exists(filePath))
                {
                    return null;
                }
                string line;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    line = await sr.ReadToEndAsync();
                }
                return line;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Nie udało się wczytać pliku" + Environment.NewLine + ex.Message);
            }
        }
        public async Task<string> LoadDataPath(string path)
        {
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    return null;
                }
                string line;
                using (StreamReader sr = new StreamReader(path))
                {
                    line = await sr.ReadToEndAsync();
                }
                return line;
            }
            catch (IOException ex)
            {
                throw new IOException("Error:Nie udało się wczytać pliku" + Environment.NewLine + ex.Message);
            }
        }
    }
}
