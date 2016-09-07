using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSCP;

namespace RealtimeSpreadMonitor.FormManipulation
{
    class FTPGetGMI
    {

        internal List<String> getFTPFiles()
        {
            List<String> filesToLoadFromFCM = new List<String>();

            DateTime currentDate = DateTime.Now.Date;
            DateTime positionFileDate;

            if (currentDate.DayOfWeek == DayOfWeek.Monday)
            {
                positionFileDate = currentDate.AddDays(-3);
            }
            else if (currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                positionFileDate = currentDate.AddDays(-2);
            }
            else
            {
                positionFileDate = currentDate.AddDays(-1);
            }

            String fcmFtpFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                    TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY,
                    TradingSystemConstants.REALTIME_CONFIGURATION);

            if (File.Exists(fcmFtpFile))
            {
                FileStream fileStream = new FileStream(fcmFtpFile,
                       FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                StreamReader streamReader = new StreamReader(fileStream);

                while (!(streamReader.EndOfStream))
                {
                    //string line = streamReader.ReadLine();

                    String lineWithTokenAndValue = streamReader.ReadLine();

                    int locationOfEqual = lineWithTokenAndValue.IndexOf("=");
                    String token = lineWithTokenAndValue.Substring(0, locationOfEqual);
                    String line = lineWithTokenAndValue.Substring(locationOfEqual + 1);

                    if (token.CompareTo(TradingSystemConstants.FCM_FTP) == 0
                        && line.Length > 0)
                    {

                        List<string> stringList = readSeparatedLine(line, ',');

                        string ftpFileDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                            TradingSystemConstants.FCM_DATA_FOLDER, stringList[1]);

                        if (!Directory.Exists(ftpFileDir))
                        {
                            Directory.CreateDirectory(ftpFileDir);
                        }

                        runwscp(stringList[0], ftpFileDir,
                            stringList[2], currentDate, positionFileDate,
                            filesToLoadFromFCM);

                    }
                }




                streamReader.Close();
                fileStream.Close();
            }

            return filesToLoadFromFCM;
        }

        private List<String> readSeparatedLine(String separatedLine, char separator)
        {
            List<String> listOfStrings = new List<string>();

#if DEBUG
            try
#endif
            {
                String[] separatedArray = separatedLine.Split(separator);

                for (int arrayCnt = 0; arrayCnt < separatedArray.Length; arrayCnt++)
                {
                    String x = separatedArray[arrayCnt].Replace("\"", "");

                    listOfStrings.Add(x); //separatedArray[arrayCnt].Trim());
                }

            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

            return listOfStrings;
        }

        private SessionOptions createSession(string fcm)
        {
            SessionOptions sessionOptions = new SessionOptions();

            switch (fcm)
            {
                case "wedbush":
                {
                    sessionOptions.Protocol = Protocol.Sftp;
                    sessionOptions.HostName = "ftp.wedbushfutures.com";
                    sessionOptions.UserName = "njoice";
                    sessionOptions.Password = "dfg789%";
                    sessionOptions.SshHostKeyFingerprint =
                        "ssh-rsa 2048 da:5f:17:98:72:51:d7:b3:59:d2:47:32:83:9c:a2:b9";
                }
                break;

                case "adm":
                {
                    sessionOptions.Protocol = Protocol.Sftp;
                    sessionOptions.HostName = "SFTP.admis.com";
                    //sessionOptions.HostName = "sftp01.admis.com";
                    sessionOptions.UserName = "trendftp";
                    sessionOptions.Password = "Jugh34d";
                    //sessionOptions.SshHostKeyFingerprint =
                    //    "ssh-rsa 1024 f2:7d:42:65:16:f5:9b:da:30:f9:9f:b3:d8:80:00:3e";
                    sessionOptions.SshHostKeyFingerprint =
                        "ssh-rsa 1024 f2:7d:42:65:16:f5:9b:da:30:f9:9f:b3:d8:80:00:3e";
                }
                break;

            }

            return sessionOptions;
        }

        private string getPrelimFile(string fcm, DateTime currentDate)
        {
            StringBuilder prelimFile = new StringBuilder();

            switch (fcm)
            {
                case "adm":
                    {
                        prelimFile.Append(currentDate.ToString("yyyy-MM-dd",
                            DateTimeFormatInfo.InvariantInfo));
                        prelimFile.Append(".aaprlmcsv.csv");
                    }
                    break;

                case "wedbush":
                    {
                        prelimFile.Append("prltrades2_");
                        prelimFile.Append(currentDate.ToString("yyyyMMdd",
                            DateTimeFormatInfo.InvariantInfo));
                        prelimFile.Append(".csv");
                    }
                    break;

            }

            return prelimFile.ToString();
        }

        private string getPosFile(string fcm, DateTime positionFileDate)
        {
            StringBuilder poslimFile = new StringBuilder();

            switch (fcm)
            {
                case "adm":
                    {
                        poslimFile.Append(positionFileDate.ToString("yyyy-MM-dd",
                            DateTimeFormatInfo.InvariantInfo));
                        poslimFile.Append(".adatpos.csv");
                    }
                    break;

                case "wedbush":
                    {
                        poslimFile.Append("pos");
                        poslimFile.Append(positionFileDate.ToString("yyyyMMdd",
                            DateTimeFormatInfo.InvariantInfo));
                        poslimFile.Append(".csv");
                    }
                    break;

            }

            return poslimFile.ToString();
        }

        internal void runwscp(string ftpFolderName, string ftpLocalFileDir,
            string fcm, DateTime currentDate, DateTime positionFileDate,
            List<string> filesToLoadFromFCM)
        {
            try
            {
                SessionOptions sessionOptions =
                    createSession(fcm);

                using (Session session = new Session())
                {
                    

                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    string posFile = getPosFile(fcm, positionFileDate);

                    transferOptions.FileMask = posFile;

                    TransferOperationResult transferResult;
                    transferResult = session.GetFiles(@ftpFolderName, ftpLocalFileDir, false, transferOptions);

                    string downloadedFile =
                        System.IO.Path.Combine(ftpLocalFileDir, posFile);

                    if (File.Exists(downloadedFile))
                    {
                        filesToLoadFromFCM.Add(downloadedFile);
                    }

                    string prelimFile = getPrelimFile(fcm, currentDate);

                    transferOptions.FileMask = prelimFile;

                    //TransferOperationResult transferResult;
                    transferResult = session.GetFiles(@ftpFolderName, ftpLocalFileDir, false, transferOptions);

                    downloadedFile =
                        System.IO.Path.Combine(ftpLocalFileDir, prelimFile);

                    if (File.Exists(downloadedFile))
                    {
                        filesToLoadFromFCM.Add(downloadedFile);
                    }

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    //foreach (TransferEventArgs transfer in transferResult.Transfers)
                    //{
                    //    Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                    //}
                }

                //return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                //return 1;
            }

        }
    }
}
