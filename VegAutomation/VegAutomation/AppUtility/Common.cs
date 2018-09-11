using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VegAutomation.AppUtlity;
using System.IO;


namespace VegAutomation.AppUtlity
{
    public partial class Common
    {


        public static int FetchVoucherNum(string source, string crdr, string accName)
        {
            int r=-1;
            string q = "select MAX(isnull(voucherId,0)) from T_DayBook where [source] = '"+source +"' and crdr = '"+crdr+"'  and AccountId = "+
                        " (select Id from T_AccountDetails  where AccName ='"+accName +"')";
            DataSet d = DBAccess.GetData(q);
            if (d.Tables.Count > 0 && d.Tables[0].Rows.Count > 0)
            {
                if (d.Tables[0].Rows[0][0].ToString() == "")
                {
                    r = 0;
                }
                else
                {
                    r = Convert.ToInt16(d.Tables[0].Rows[0][0].ToString());
                }
            }
            return r;
        }

        public static int FetchVoucherNum(string source, string crdr)
        {
            int r = -1;
            string q = "select MAX(isnull(voucherId,0)) from T_DayBook where [source] = '" + source + "' and crdr = '" + crdr + "' ";
            DataSet d = DBAccess.GetData(q);
            if (d.Tables.Count > 0 && d.Tables[0].Rows.Count > 0)
            {
                r = Convert.ToInt16(d.Tables[0].Rows[0][0]);
            }
            return r;
        }


        //Generate Unique ID
        public static String GetNextUniqueId()
        {
            DateTime dt = DateTime.Now;
            string strMonth = ((dt.Month < 10) ? "0" : string.Empty) + dt.Month.ToString();
            string strDay = ((dt.Day < 10) ? "0" : string.Empty) + dt.Day.ToString();
            String RetVal = dt.Year.ToString() + strMonth + strDay + dt.Minute.ToString() + dt.Second.ToString();
            return RetVal;
            //Console.Write(RetVal);
        }

        public static void CallLogFunction(Exception ExMsg)
        {
            string message = "ERROR Message :" + ExMsg.Message + "\r\n";
            message = message + "ERROR Source :" + ExMsg.Source + "\r\n";
            message = message + "ERROR TargetSite :" + ExMsg.TargetSite + "\r\n";
            message = message + "ERROR Data :" + ExMsg.Data + "\r\n";
            message = message + "ERROR Data :" + ExMsg.StackTrace + "\r\n";
            WriteToLogFile(message);
        }
        
        //Code to create or Append Log File
        //public static void WriteToLogFile(string message, string fileName)
        public static void WriteToLogFile(string message )
        {
            String sDirPath = System.IO.Directory.GetCurrentDirectory();

            sDirPath = sDirPath + "\\LogFiles";
            string fileName = sDirPath + "\\LogFile.txt";
            try
            {
                //INAMUL: Change to get Current Directory

                // Refresh logs folder
                if (!Directory.Exists(sDirPath))
                {
                    Directory.CreateDirectory(sDirPath);
                }
                string newMsg = "\r\nDate: " + System.DateTime.Now.ToString();
                newMsg += "\r\n" + message;
                newMsg += "\r\n************************************************************************************";
                File.AppendAllText(fileName, newMsg);
                MaintainLogFiles(new FileInfo(fileName));
            }
            catch (Exception)
            {

            }
            finally
            {
            }


        }

        /// <summary>
        /// This method maintains VPS and Activity Log files.
        /// </summary>
        private static void MaintainLogFiles(FileInfo fileInfo)
        {
            DirectoryInfo directoryInfo = null;
            FileInfo[] files = null;
            try
            {
                //If File Size is greater than XX then create new log file
                if (fileInfo.Length > 52428800)
                //if (fileInfo.Length > 524)
                {
                    string parentDirectory = fileInfo.DirectoryName;
                    string directoryname = fileInfo.DirectoryName + "\\" + fileInfo.Name.Replace("-", "");
                    String sDirPath = System.IO.Directory.GetCurrentDirectory();
                    sDirPath = sDirPath + "\\LogFiles";

                    if (Directory.Exists(sDirPath))
                    {
                        //sDirPath
                        directoryInfo = new DirectoryInfo(sDirPath);
                        files = directoryInfo.GetFiles();
                        fileInfo.MoveTo(sDirPath + "\\" + fileInfo.Name + "-" + (files.Length + 1));
                    }

                }
            }
            catch (Exception exe)
            {

            }
            finally
            {
                files = null;
                directoryInfo = null;
            }
        }

        public static string GetCurrentAccountBalance(string accName,string accID, string dt)
        {
            string ret = "";
            string q = "";
            accID = accID.Trim(); accName = accName.Trim(); dt = dt.Trim();
            if (accName != "")
            {
                 q = @"select a.OpeningBal as amount,a.CrDr  from T_AccountDetails as a where AccName ='Cash account' UNION ALL
                        select d.Amount, d.crdr from dbo.T_DayBook as d
                        inner join T_AccountDetails as t on t.Id= d.AccountId and t.AccName = 'Cash account' ";
            }

            if (accID != "")
            {
                q = @"select a.OpeningBal as amount,a.CrDr  from T_AccountDetails as a where id ='" + accID + "' UNION ALL" +
                       " select d.Amount, d.crdr from dbo.T_DayBook as d where d.AccountId = '" + accID + "'";
            }

            if (dt != "")
            {
                q += " and CONVERT (date, CONVERT(varchar(10), transactiondate,101),101) < CONVERT (date, CONVERT(varchar(10), '" + dt + "',101),101) ";
            }
            
            DataSet ds = DBAccess.GetData(q);
            double  cr=0.0, dr=0.0;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (r["CrDr"].ToString().ToLower().Trim () == "cr")
                    {
                        cr = cr + Convert.ToDouble(r["Amount"].ToString());
                    }
                    else
                    {
                        dr = dr + Convert.ToDouble(r["Amount"].ToString());
                    }
                }

                if (cr > dr)
                {
                    ret = "CR: "+ (cr - dr).ToString();
                }
                else if (cr == dr)
                {
                    ret = "Nill: " + (cr - dr).ToString();
                }
                else
                {
                    ret = "DR: " + (dr - cr).ToString();
                }
            }
            return ret;
        }

        public static void saveAsAccount(string accountName)
        {
            Guid id = Guid.NewGuid();
            string accName = accountName.Replace(' ', '_'); 
            //accName = accName.Substring(8, txtName.Text.Trim().Length - 8);
            string sqlChk = "select AccName from T_AccountDetails where accName = '" + accName + "'";
            DataSet d = DBAccess.GetData(sqlChk);
            if (d.Tables[0].Rows.Count == 0)
            {
                string SQL = "INSERT INTO [dbo].[T_AccountDetails] ([Id],[AccName],[ShortCode],[Address] ,[Disstrict] ,[PhoneOff],[PhoneResi],[Mobile],[OpeningBal],[AccountGroup],[CrDr]) VALUES " +
                               " ('" + id + "' " +
                               ",'" + accName + "' " +
                               ",'" + accName + "'" +
                               ",' ' " +
                               ",' ' " +
                               ",' '" +
                               ",' '" +
                               ",' ' " +
                               ",'0' " +
                               ",'2','Cr')";

                DBAccess.ExecSQLScript(SQL);
            }

        }

    }
}
