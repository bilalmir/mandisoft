using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System .Data;
using System.Data.SqlClient ;
using VegAutomation.AppUtlity;

namespace VegAutomation.AppUtlity
{
    public partial class  Common
    {
        public static DataSet GetOperationType()
        {
            string q = "select * from T_Operationtype order by orderby";
            DataSet d = new DataSet();
            d=    DBAccess.GetData(q);
            return d;
        }

    }
}
