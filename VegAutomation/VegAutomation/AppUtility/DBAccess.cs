using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace VegAutomation.AppUtlity
{
    class DBAccess
    {
        const string DBConnStr = "ConnectionStr";

        /// <summary>
        /// Returns ADO connection string
        /// </summary>
        public static string GetConnStr()
        {
            return ConfigurationManager.AppSettings[DBConnStr].ToString();
        }

        public static void BulkInsertCheque( DataTable t)
        {
            string conString =GetConnStr ();
            //do bulk insert
            try
            {
                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();
                    using (SqlBulkCopy copy = new SqlBulkCopy(cn))
                    {
                        copy.ColumnMappings.Add("AccountId", "AccountId");
                        copy.ColumnMappings.Add("ChqNum", "ChqNum");
                        copy.ColumnMappings.Add("Available", "Available");
                        copy.ColumnMappings.Add("OtherDetails", "OtherDetails");

                        copy.DestinationTableName = "T_ChequeNumbers";
                        copy.WriteToServer(t);
                        copy.Close();
                    }
                    if (!(cn.State == ConnectionState.Closed))
                    {
                        cn.Close();
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        
        public static ListView GetSalesForExport(string strSQL, ListView myList)
        {
            // ListView myList = new ListView();
            SqlConnection myConnection;
            SqlDataReader dr = null;
            try
            {
                myConnection = new SqlConnection(DBAccess.GetConnStr());

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                if (string.IsNullOrEmpty(strSQL))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT T_SalesMain.Sno, T_SalesMain.SalesID,  T_SalesMain.CreateDate, T_Buyer.BuyerName, T_Buyer.BuyerNickName, T_SalesMain.BuyerID,  T_SalesMain.UpdateDate, T_SalesMain.UserID, T_SalesMain.IsCash, T_SalesMain.IsExport FROM         T_SalesMain INNER JOIN   T_Buyer ON T_SalesMain.BuyerID = T_Buyer.BuyerID WHERE     (T_SalesMain.SalesID IN  (SELECT     SalesID FROM          T_SalesDetails WHERE      (IsExport = 1) AND (NetValue IS NULL) OR (IsExport = 1) AND (GrossValue IS NULL))) AND (T_SalesMain.IsExport = 1)";
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = strSQL;
                }



                myConnection.Open();
                //DataSet ds = FillDataset("GetReceitsForWatak", null, null);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem lItem = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i <= dr.FieldCount - 1; i++)
                    {
                        lItem.SubItems.Add(dr[i].ToString());
                    }
                    myList.Items.Add(lItem);
                    int rCount = myList.Items.Count;
                    if (rCount % 2 == 1)
                    {
                        //lItem.BackColor = Color.WhiteSmoke;
                        lItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(234)))), ((int)(((byte)(189)))));
                    }
                    else
                    {
                        lItem.BackColor = System.Drawing.Color.White;
                    }
                }
                myConnection.Close();
                cmd.Dispose();
                dr.Close();
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
            }
            return myList;
        }
        
        public static ListView GetReceitsForWatak(string strSQL, ListView myList)
        {
           // ListView myList = new ListView();
            SqlConnection myConnection;
            SqlDataReader dr = null;
            try
            {
                myConnection = new SqlConnection(DBAccess.GetConnStr());

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                if (string.IsNullOrEmpty(strSQL))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetReceitsForWatak";
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = strSQL;
                }


                
                myConnection.Open();
                //DataSet ds = FillDataset("GetReceitsForWatak", null, null);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem lItem = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i <= dr.FieldCount - 1; i++)
                    {
                        lItem.SubItems.Add(dr[i].ToString());
                    }
                    myList.Items.Add(lItem);
                    int rCount = myList.Items.Count;
                    if (rCount % 2 == 1)
                    {
                        //lItem.BackColor = Color.WhiteSmoke;
                        lItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(234)))), ((int)(((byte)(189)))));
                    }
                    else
                    {
                        lItem.BackColor = System.Drawing.Color.White;
                    }
                }
                myConnection.Close();
                cmd.Dispose();
                dr.Close();
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
            }
            return myList;
        }
        
        /// <summary>
        /// Returns filled dataset from stored procedure name and its parameters
        /// </summary>
        public static DataSet FillDataset(string SProc, SqlParameter[] Params, object[] Values)
        {
            return FillDataset(SProc, Params, Values, DBAccess.GetConnStr());
        }

        /// <summary>
        /// Returns filled dataset from stored procedure name and its parameters
        /// </summary>
        public static DataSet FillDataset(string SProc, SqlParameter[] Params, object[] Values, string ConStr)
        {
            SqlConnection myConnection = new SqlConnection(ConStr);
            SqlDataAdapter myAdapter = new SqlDataAdapter();

            myAdapter.SelectCommand = new SqlCommand(SProc, myConnection);
            myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            // assign all parameters with its values
            for (int i = 0; i < Params.Length; i++)
            {
                myAdapter.SelectCommand.Parameters.Add(Params[i]).Value = Values[i];
            }

            DataSet myDataSet = new DataSet();

            myAdapter.Fill(myDataSet);

            return myDataSet;
        }
        
        public static int GetNextAvailableSalesNumber()
        {
            int RetVal = 0;
            SqlConnection myConnection;
            SqlCommand myCmd;

            try
            {
                myConnection = new SqlConnection(DBAccess.GetConnStr());
                myCmd = new SqlCommand("SELECT MAX(Sno) from T_SalesMain", myConnection);
                myCmd.CommandType = CommandType.Text;

                myConnection.Open();
                object tmpValue = myCmd.ExecuteScalar();
                if (tmpValue == System.DBNull.Value)
                {
                    RetVal = 1;
                }
                else
                {
                    RetVal = (int)tmpValue;
                    RetVal++;
                }
                myConnection.Close();
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
                //myConnection.Close();

            }

            return RetVal;
        }
        public static int GetNextAvailableReceitNumber()
        {
            int RetVal = 0;
            SqlConnection myConnection;
            SqlCommand myCmd;

            try
            {
                myConnection = new SqlConnection(DBAccess.GetConnStr());
                myCmd = new SqlCommand("SELECT MAX(ReceitNoInt) from T_Receit", myConnection);
                myCmd.CommandType = CommandType.Text;

                myConnection.Open();
                object tmpValue = myCmd.ExecuteScalar();
                if (tmpValue == System.DBNull.Value)
                {
                    RetVal = 1;
                }
                else
                {
                    RetVal = (int)tmpValue;
                    RetVal++;
                }
                myConnection.Close();
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
                //myConnection.Close();

            }

            return RetVal;
        }
        /// <summary>
        /// Executes stored procedure with its parameters
        /// </summary>
        public static void ExecSQL(string SProc, SqlParameter[] Params, object[] Values)
        {
            SqlConnection myConnection;
            SqlCommand myCmd;

            try
            {
                myConnection = new SqlConnection(DBAccess.GetConnStr());
                myCmd = new SqlCommand(SProc, myConnection);
                myCmd.CommandType = CommandType.StoredProcedure;

                // assign all parameters with its values
                for (int i = 0; i < Params.Length; i++)
                {
                    myCmd.Parameters.Add(Params[i]).Value = Values[i];
                }
                myConnection.Open();
                myCmd.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
                //myConnection.Close();

            }
        }

        private static SqlConnection myConnection;
        private static void OpenConnection()
        {
            if (myConnection == null)
            {
                myConnection = new SqlConnection(DBAccess.GetConnStr());
                myConnection.Open();
            }
        }
        private static void CloseConnection()
        {
            if(myConnection!=null && myConnection.State == ConnectionState.Open)
            myConnection.Close();
        }

        public static string GetIdentityAfterInsert(string sqlScript)
        {
            //string sqlScript1 = ""INSERT into T_Users (UserTypeId,UserName) values (2,'333');select scope_identity()"";
            SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
            SqlCommand myCmd = new SqlCommand(sqlScript, myConnection);
            myCmd.CommandType = CommandType.Text;
            myConnection.Open();
            string RetVal = string.Empty;
            try
            {
                object obj = myCmd.ExecuteScalar();
                RetVal = obj.ToString();
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
                myConnection.Close();
            }
            return RetVal;
        }

        public static int ExecSQLScript(string sqlScript)
        {
            SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
            SqlCommand myCmd = new SqlCommand(sqlScript,myConnection);
            myCmd.CommandType = CommandType.Text;
            myConnection.Open();
            int RetVal = -1;
            try
            {
                RetVal = myCmd.ExecuteNonQuery();
            }
            catch (Exception exe) 
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally {
                myConnection.Close();
            }           
            return RetVal;
        }

        public static DataSet GetData(string sqlScript)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlDataAdapter myAdap = new SqlDataAdapter(sqlScript, myConnection);
                myAdap.Fill(ds);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
            }
            return ds;
        }
        public static string GetValueForKey(string sqlScript)
        {
            string RetVal = string.Empty;
            try
            {
                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlCommand myCmd = new SqlCommand(sqlScript, myConnection);
                myConnection.Open();
                object value = myCmd.ExecuteScalar();
                myConnection.Close();
                if (value != null)
                    RetVal = value.ToString();
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
            }
            return RetVal;
        }

        public static DataSet GetUnits()
        {
            String sqlScript = "select unitid, unitname from T_Units where isActive=1;";
            DataSet ds = new DataSet();
            try
            {
                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlDataAdapter myAdap = new SqlDataAdapter(sqlScript, myConnection);
                myAdap.Fill(ds);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
            }
            return ds;
        }
                //INAMUL: Get ITEM-Product NAMES
        public static DataSet GetItemNames()
        {
            String sqlScript = "select ItemTypeId, ItemTypeName from T_ItemTypes where isActive=1 Order by ItemTypeName ;";
            DataSet ds = new DataSet();
            try
            {

                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlDataAdapter myAdap = new SqlDataAdapter(sqlScript, myConnection);
                myAdap.Fill(ds);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
            }
                return ds;
        }

        public static DataSet GetBuyerID()
        {
            String sqlScript = "select TOP 1 BuyerID from T_Buyer ORDER BY BuyerID DESC";
            DataSet ds = new DataSet();
            try
            {

                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlDataAdapter myAdap = new SqlDataAdapter(sqlScript, myConnection);

                myAdap.Fill(ds);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
                //return ds;
            }
            return ds;
        }

        public static DataSet GetUserUniqueID(string UniqueUserID)
        {
            String sqlScript = "Select UserId FROM T_Users WHERE UniqueID ='" + UniqueUserID + "'";
            DataSet ds = new DataSet();
            try
            {

                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlDataAdapter myAdap = new SqlDataAdapter(sqlScript, myConnection);

                myAdap.Fill(ds);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);

            }
            finally
            {
            }
            return ds;
        }
        //INAMUl: User Verification 
        public static DataSet GetValidUser(string sUserName, string sPassword)
        {
            //Allow only 'System' users to log in..
            String sqlScript = "Select UserId FROM T_Users WHERE UserName ='" + sUserName + "' AND UserPassword ='" + sPassword + "' AND UserTypeId=4";
            DataSet ds = new DataSet();
            try
            {

                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlDataAdapter myAdap = new SqlDataAdapter(sqlScript, myConnection);

                myAdap.Fill(ds);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);

            }
            finally
            {
            }
            return ds;
        }

       //INAMUL: get Grower details
        public static DataSet GetGrowerNames(int userTypeID)
        {
            String sqlScript = "SELECT UserId,(UserName + ' > ' +ISNULL(FullName, 'N') +' > '+ISNULL(UserNickName, 'N')+' > '+ ISNULL(UserAddress,'N')) AS Names FROM T_Users WHERE UserTypeID =" + userTypeID  ;
            DataSet ds = new DataSet();
            try
            {
                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlDataAdapter myAdap = new SqlDataAdapter(sqlScript, myConnection);

                myAdap.Fill(ds);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
            }
            return ds;

        }
        //INAMUL: get Grower details
        public static DataSet GetGrowerMappingID(int userTypeID)
        {
            String sqlScript = "SELECT GrowerId FROM T_GrowerKhataMappings WHERE KhataId =" + userTypeID;
            DataSet ds = new DataSet();
            try
            {

                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlDataAdapter myAdap = new SqlDataAdapter(sqlScript, myConnection);

                myAdap.Fill(ds);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
            }


            return ds;

        }

        //public DataTable GetGrowerNamesASdt(int UserTypeID)
        //{
        //    DataSet dsGrowerNames = null;
        //    DataTable dtGrowerNames = new DataTable();
        //    // Set the stored procedure name to be executed
        //    string storedProcedureName = "GetKhataDetailaOnReceipt";

        //    try
        //    {
        //        // execute the stored procedure and return a dataset
        //        SqlParameter[] sqlParams = new SqlParameter[1];
        //        sqlParams[0] = new SqlParameter("@ReceitNo", receiptNo);
        //        dsKhataDetails = SqlHelper.ExecuteDataset(GetConnStr(), CommandType.StoredProcedure, storedProcedureName, sqlParams);
        //        if (dsKhataDetails != null && dsKhataDetails.Tables.Count > 0)
        //        {
        //            dtKhataDetails = dsKhataDetails.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return dtKhataDetails;
        //}

        //INAMUL: get user Type details
        public static DataSet GetUserType()
        {
            String sqlScript = "SELECT UserTypeId,UserTypeName FROM T_UserTypes Order by UserTypeName";
            DataSet ds = new DataSet();
            try
            {

                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlDataAdapter myAdap = new SqlDataAdapter(sqlScript, myConnection);
                myAdap.Fill(ds);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            finally
            {
            }
            return ds;

        }

        public static DataSet FillDataSetFromSQLScript(string sqlScript)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection myConnection = new SqlConnection(DBAccess.GetConnStr());
                SqlDataAdapter myAdap = new SqlDataAdapter(sqlScript, myConnection);

                myAdap.Fill(ds);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            return ds;
        }
        public static DataSet GetActiveUnits()
        {
            DataSet ds = new DataSet();

            return ds;
        }
    }
}

