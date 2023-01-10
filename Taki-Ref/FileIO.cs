using System.IO;
using System.Text;

namespace Taki_Ref
{
    class FileIO
    {
        public static void Create(string filename)
        {
            if (!File.Exists(filename))
            {
                var myFile = File.Create(filename);
                myFile.Close();
            }
        }

        public static void CreateFile(Taki taki)
        {
            string jsAcount = Newtonsoft.Json.JsonConvert.SerializeObject(taki);
            string path = "Data//" + taki.name + ".data";
            if (!File.Exists(path))
            {
                var myFile = File.Create(path);
                myFile.Close();
                WriteFilePath(path, jsAcount);
            }
            else
            {
                File.Delete(path);
                var myFile = File.Create(path);
                myFile.Close();
                WriteFilePath(path, jsAcount);
            }
        }

        public static string ReadFile(string filename)
        {
            string text;
            var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            fileStream.Close();
            return text;
        }

        public static void WriteFile(string filename, string text)
        {
            File.WriteAllText("Data//" + filename, string.Empty);
            var fileStream = new FileStream("Data//" + filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using (var sw = new StreamWriter(fileStream, Encoding.UTF8))
            {
                sw.WriteLine(text);
            }
            fileStream.Close();
        }

        public static void WriteFilePath(string path, string text)
        {
            File.WriteAllText(path, string.Empty);
            var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using (var sw = new StreamWriter(fileStream, Encoding.UTF8))
            {
                sw.WriteLine(text);
            }
            fileStream.Close();
        }
    }
}
