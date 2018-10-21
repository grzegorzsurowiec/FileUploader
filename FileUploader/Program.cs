using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileUploader
{
    class Program
    {
        private const string ftpServerName = "_PUT_NAME_HERE_"";
        private const string ftpUserName = "_PUT_USERNAME_HERE_";
        private const string ftpUserPassword = "_PUT_PASSWORD_HERE_";

        static void Main(string[] args)
        {
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            string filename;
            Console.WriteLine("FileUploader by Grzegorz Surowiec 2012");

            foreach(string plik in new List<string>() { "phone", "mobile", "email" } )
            {
                filename = string.Concat(currentDir, plik, ".csv");

                if (File.Exists(filename))
                {
                    Console.WriteLine("Send file " + filename);
                    FileUpload(filename);
                }
                else Console.WriteLine("File not found: " + filename);
            }
        }

        static void FileUpload(string fileName)
        {
            FileInfo fileInf = new FileInfo(fileName);
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerName + "/excel/" + fileInf.Name));
            reqFTP.Credentials = new NetworkCredential(ftpUserName, ftpUserPassword);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;

            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                strm.Close();
                fs.Close();
            }

            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
