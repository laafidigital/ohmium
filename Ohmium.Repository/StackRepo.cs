using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ohmium.Repository
{

    public class StackRepo
    {
        //SqlConnection conn = new SqlConnection("Server=20.197.59.213;Database=Ohmiumdb;user= RameshenArka;password=Admin@8231");
  public DataTable GetDeviceStackDataNew(DateTime sdate, DateTime edate, string seqName, string stackmfgid, int sec)
        {
            DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection("Server=mtssqlprod.database.windows.net;Database=Ohmiumdb;user=sasanka;password=Kororahul@23;TrustServerCertificate = True;"))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DeviceStackDataByStackID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 700;
                        cmd.Parameters.Add("@seqName", SqlDbType.NVarChar).Value = seqName;
                        cmd.Parameters.Add("@sdate", SqlDbType.DateTime).Value = sdate;
                        cmd.Parameters.Add("@edate", SqlDbType.DateTime).Value = edate;
                        cmd.Parameters.Add("@stackids", SqlDbType.NVarChar).Value = stackmfgid;
                        cmd.Parameters.Add("@sec", SqlDbType.Int).Value = sec;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
                return dt;
            }


            public DataTable GetDeviceStackData(DateTime sdate, DateTime edate, string seqName, string deviceID, string stackList, int sec)
        {
            sdate = sdate.ToUniversalTime();
            edate= edate.ToUniversalTime();
            DataTable dt = new DataTable();
            if(sdate.Month >= 01 && sdate.Year >= 2024 && sdate.Day>=4)
            {
                using (SqlConnection con = new SqlConnection("Server=mtssqlprod.database.windows.net;Database=Ohmiumdb;user=sasanka;password=Kororahul@23;TrustServerCertificate = True;"))
                {
                    using (SqlCommand cmd = new SqlCommand("OneDayData", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 700;
                        cmd.Parameters.Add("@seqName", SqlDbType.NVarChar).Value = seqName;
                        cmd.Parameters.Add("@sdate", SqlDbType.DateTime).Value = sdate;
                        cmd.Parameters.Add("@edate", SqlDbType.DateTime).Value = edate;
                        //cmd.Parameters.Add("@deviceid", SqlDbType.NVarChar).Value = deviceID;
                        cmd.Parameters.Add("@stkid", SqlDbType.NVarChar).Value = stackList;
                        cmd.Parameters.Add("@sec", SqlDbType.Int).Value = sec;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
 
            else if(edate.Month >=01 && edate.Year>=2024 && edate.Day<=3)
            {
                using (SqlConnection con = new SqlConnection("Server=mtssqlprod.database.windows.net;Database=Ohmiumdb;user=sasanka;password=Kororahul@23;TrustServerCertificate = True;"))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DeviceStackDataNewTable", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 700;
                        cmd.Parameters.Add("@seqName", SqlDbType.NVarChar).Value = seqName;
                        cmd.Parameters.Add("@sdate", SqlDbType.DateTime).Value = sdate;
                        cmd.Parameters.Add("@edate", SqlDbType.DateTime).Value = edate;
                        cmd.Parameters.Add("@deviceid", SqlDbType.NVarChar).Value = deviceID;
                        cmd.Parameters.Add("@stackids", SqlDbType.NVarChar).Value = stackList;
                        cmd.Parameters.Add("@sec", SqlDbType.Int).Value = sec;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
