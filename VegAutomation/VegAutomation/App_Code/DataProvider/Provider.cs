using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace VegAutomation.App_Code.DataProvider
{
    public class Provider
    {
        const string DBConnStr = "ConnectionStr";
        private static SqlConnection myConnection;
        /// <summary>
        /// Returns ADO connection string
        /// </summary>
        public static string GetConnStr()
        {
            return ConfigurationManager.AppSettings[DBConnStr].ToString();
            
        }

        private static void OpenConnection()
        {
            if (myConnection == null)
            {
                myConnection = new SqlConnection(GetConnStr());
                myConnection.Open();
            }
        }
        private static void CloseConnection()
        {
            if (myConnection != null && myConnection.State == ConnectionState.Open)
                myConnection.Close();
        }

        /// <summary>
        /// This stored procedure will return all MainKhataName
        /// </summary>
        /// <returns>dataset</returns>
        public DataSet GetAllMainKhataName()
        {
            DataSet dsMainKhataName = null;
            // Set the stored procedure name to be executed
            string storedProcedureName = "GetAllMainKhataName";

            try
            {
                // execute the stored procedure and return a dataset
                dsMainKhataName = SqlHelper.ExecuteDataset(GetConnStr(), CommandType.StoredProcedure, storedProcedureName);
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            return dsMainKhataName;
        }
        //Inamul: Function to return User Details as datatable
        public DataTable GetUserDetails()
        {
            DataSet dsUserDetails = null;
            DataTable dtSUserDetails = new DataTable();
            // Set the stored procedure name to be executed
            string storedProcedureName = "GetUserDetails";

            try
            {
                dsUserDetails = SqlHelper.ExecuteDataset(GetConnStr(), CommandType.StoredProcedure, storedProcedureName);
                if (dsUserDetails != null && dsUserDetails.Tables.Count > 0)
                {
                    dtSUserDetails = dsUserDetails.Tables[0];
                }
                
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);

            }
            finally
            {

            }
            return dtSUserDetails;
        }

        //Inamul: Function to return Grower Name as datatable
        ////public DataTable GetGrowerNames()
        ////{
        ////    DataSet dsUserDetails = null;
        ////    DataTable dtSUserDetails = new DataTable();
        ////    // Set the stored procedure name to be executed
        ////    string storedProcedureName = "GetGrowerNames";

        ////    try
        ////    {
        ////        dsUserDetails = SqlHelper.ExecuteDataset(GetConnStr(), CommandType.StoredProcedure, storedProcedureName);
        ////        if (dsUserDetails != null && dsUserDetails.Tables.Count > 0)
        ////        {
        ////            dtSUserDetails = dsUserDetails.Tables[0];
        ////        }

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {

        ////    }
        ////    return dtSUserDetails;
        ////}

        //INAMUL : Get Invoice Details for buyer for Current Date
        public DataTable GetSalesInvoiceForPrinting(string strSaleID)
        {
            DataSet dsSalesInvoicePrint = null;
            DataTable dtSalesInvoicePrint = new DataTable();
            // Set the stored procedure name to be executed
            string storedProcedureName = "GetSalesInvoiceForPrinting";

            try
            {
                // execute the stored procedure and return a dataset
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@SalesID", strSaleID);
                dsSalesInvoicePrint = SqlHelper.ExecuteDataset(GetConnStr(), CommandType.StoredProcedure, storedProcedureName, sqlParams);
                if (dsSalesInvoicePrint != null && dsSalesInvoicePrint.Tables.Count > 0)
                {
                    dtSalesInvoicePrint = dsSalesInvoicePrint.Tables[0];
                }
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            return dtSalesInvoicePrint;
        }

        /// <summary>
        /// This stored procedure will return all the details of a particular khata on the basis of recipt number
        /// </summary>
        /// <returns>dataset</returns>
        public DataTable GetKhataDetails(string receiptNo)
        {
            DataSet dsKhataDetails = null;
            DataTable dtKhataDetails = new DataTable();
            // Set the stored procedure name to be executed
            string storedProcedureName = "GetKhataDetailaOnReceipt";

            try
            {
                // execute the stored procedure and return a dataset
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@ReceitNo", receiptNo);
                dsKhataDetails = SqlHelper.ExecuteDataset(GetConnStr(), CommandType.StoredProcedure, storedProcedureName, sqlParams);
                if (dsKhataDetails != null && dsKhataDetails.Tables.Count > 0)
                {
                    dtKhataDetails = dsKhataDetails.Tables[0];
                }
            }
            catch (Exception exe)
            {
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            return dtKhataDetails;
        }

        /// <summary>
        /// This stored procedure will return all the details of a particular khata on the basis of recipt number
        /// </summary>
        /// <returns>dataset</returns>
        public bool SaveSalesDetails(string receit, float Quantity, int ReceitDetailId, float ReceitQuantity, int BuyerID)
        {

            bool result = false;
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[5];
                sqlParams[0] = new SqlParameter("@ReceitNo", receit);
                sqlParams[1] = new SqlParameter("@Quantity", Quantity);
                sqlParams[2] = new SqlParameter("@ReceitDetailId", ReceitDetailId);
                sqlParams[3] = new SqlParameter("@ReceitQuantity", ReceitQuantity);
                sqlParams[4] = new SqlParameter("@BuyerID", BuyerID);
                SqlHelper.ExecuteNonQuery(GetConnStr(), CommandType.StoredProcedure, "AddNewSales", sqlParams);
                result = true;
            }
            catch (Exception exe)
            {
                result = false;
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            return result;
        }

        public bool BackupDatabase(string FilePath)
        {

            bool result = false;
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@varDiskPath", FilePath);
                SqlHelper.ExecuteNonQuery(GetConnStr(), CommandType.StoredProcedure, "BackUpDatabase", sqlParams);
                result = true;
            }
            catch (Exception exe)
            {
                result = false;
                VegAutomation.AppUtlity.Common.CallLogFunction(exe);
            }
            return result;
        }


    }
}
