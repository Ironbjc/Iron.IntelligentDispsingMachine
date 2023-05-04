using Iron.IntelligentDispsingMachine.Entities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.DataAccess
{
    public class LocalDataAccess
    {
        #region 基础方法
        private string ConnName;
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataAdapter sda;
        private SqlDataReader sdr;
        private DataSet ds;
        private SqlTransaction trans;
        public LocalDataAccess(string ConnectString)
        {
            ConnName = ConnectString;
            conn = new SqlConnection(ConnName);
        }
        private void Dispose()
        {
            if (trans != null)
            {
                trans.Dispose();
                trans = null;
            }
            if (sda != null)
            {
                sda.Dispose();
                sda = null;
            }
            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }
            if (cmd != null)
            {
                cmd.Dispose();
                cmd = null;
            }

        }
        public void Open()
        {
            try
            {
                if (conn == null || conn.ConnectionString == string.Empty)
                {
                    conn = new SqlConnection(ConnName);
                }
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"数据库打开失败失败:{ex.Message}");


            }
        }
        public DataTable GetDtSql(string sql)
        {
            try
            {
                this.Open();
                sda = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                // GlobalValue.IronLog.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Dispose();
            }
        }

        public SqlDataReader GetSingleResult(string sql)
        {
            try
            {
                this.Open();
                cmd = new SqlCommand(sql, conn);
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                //GlobalValue.IronLog.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private int DoExceute(string sql)
        {
            try
            {
                this.Open();
                cmd = new SqlCommand(sql, conn);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //GlobalValue.IronLog.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Dispose();
            }
        }

        private  void DoExceuteProcedure(string Procedure, SqlParameter[] param)
        {
            this.Open();
            SqlCommand cmd = new SqlCommand(Procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {           
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(param);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string errorInfo = "DoExceuteProcedure(string Procedure,SqlParameter[] param)方法时发生错误" + ex.Message;               
                throw new Exception(errorInfo);
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region 

        public List<do_list_out> GetPreByNo(string MachineID,string Pre)
        {
            try
            {
                List<do_list_out> list = new List<do_list_out>();
                //string sql = "select * from do_list_out where ID in (select min(ID) from do_list_out group by PresNo) and  OutFlag='0' and  substring(medpos,1,2)='01' order by CreateTime asc";
                string sql = $"select * from do_list_out where PresNo='{Pre}' and OutFlag='0'";
                DataTable dt = this.GetDtSql(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (var item in dt.AsEnumerable())
                    {
                        list.Add(new do_list_out()
                        {
                            MedOnlyCode = item["MedOnlyCode"]?.ToString(),
                            MedName = item["MedName"].ToString(),
                            MedUnit = item["MedUnit"].ToString(),
                            MedPack = item["MedPack"].ToString(),
                            MedFactory = item["MedFactory"].ToString(),
                            MedOutAMT = float.Parse(item["MedOutAMT"].ToString()),
                            MedPos = item["MedPos"].ToString(),
                            PresNo = item["PresNo"].ToString(),
                            PName = item["PName"].ToString(),
                            MedValidTime = DateTime.Parse(item["MedValidTime"].ToString())
                        });
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<do_list_out> GetAllPreList(string MachineID)
        {
            try
            {
                List<do_list_out> list = new List<do_list_out>();
                //string sql = "select * from do_list_out where ID in (select min(ID) from do_list_out group by PresNo) and  OutFlag='0' and  substring(medpos,1,2)='01' order by CreateTime asc";
                string sql = $"select * from do_list_out where ID in (select min(ID) from do_list_out group by PresNo) and  OutFlag='0' and  substring(medpos,1,2)='{MachineID}' order by CreateTime asc ";
                DataTable dt = this.GetDtSql(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (var item in dt.AsEnumerable())
                    {
                        list.Add(new do_list_out()
                        {
                            MedOnlyCode = item["MedOnlyCode"]?.ToString(),
                            MedName = item["MedName"].ToString(),
                            MedUnit = item["MedUnit"].ToString(),
                            MedPack = item["MedPack"].ToString(),
                            MedFactory = item["MedFactory"].ToString(),
                            MedOutAMT = float.Parse(item["MedOutAMT"].ToString()),
                            MedPos = item["MedPos"].ToString(),
                            PresNo = item["PresNo"].ToString(),
                            PName = item["PName"].ToString(),
                            MedValidTime = DateTime.Parse(item["MedValidTime"].ToString())
                        });
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 查询所有需要出药的处方
        /// </summary>
        /// <param name="MachineID">设备ID</param>
        /// <returns></returns>
        public List<do_list_out> GetAllPreDrugList(string MachineID)
        {
            try
            {
                //select top 1 PresNO from do_list_out  where OutFlag='0' and  substring(medpos,1,2)='" + MachineID + "'" + "order by CreateTime asc
                List<do_list_out> list = new List<do_list_out>();
                string sql = $"select*from do_list_out where PresNo=(select top 1 PresNO from do_list_out  where OutFlag='0' and  substring(medpos,1,2)='{MachineID}' order by CreateTime asc) and OutFlag='0' and  substring(medpos,1,2)='{MachineID}' and (substring(medpos,3,2)='01' or substring(medpos,3,2)='02' or substring(medpos,3,2)='03' or substring(medpos,3,2)='04') order by MedOutTime";
                DataTable dt = this.GetDtSql(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (var item in dt.AsEnumerable())
                    {
                        list.Add(new do_list_out()
                        {
                            MedOnlyCode = item["MedOnlyCode"]?.ToString(),
                            MedName = item["MedName"].ToString(),
                            MedUnit = item["MedUnit"].ToString(),
                            MedPack = item["MedPack"].ToString(),
                            MedFactory = item["MedFactory"].ToString(),
                            MedOutAMT = float.Parse(item["MedOutAMT"].ToString()),
                            MedPos = item["MedPos"].ToString(),
                            PresNo = item["PresNo"].ToString(),
                            PName = item["PName"].ToString(),
                            MedValidTime = DateTime.Parse(item["MedValidTime"].ToString())
                        });
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据处方条形码来查询需要出药的处方
        /// </summary>
        /// <param name="MachineID">设备ID</param>
        /// <param name="BarCode">处方条形码</param>
        /// <returns></returns>
        public List<do_list_out> GetAllPreDrugListByBarCode(string MachineID, string BarCode)
        {
            try
            {
                List<do_list_out> list = new List<do_list_out>();
                string sql = "select*from do_list_out where PresNO like '%" + BarCode + "%' and OutFlag='0' and  substring(medpos,1,2)='" + MachineID + "' and (substring(medpos,3,2)='01' or substring(medpos,3,2)='02' or substring(medpos,3,2)='03' or substring(medpos,3,2)='04') order by MedOutTime";
                DataTable dt = this.GetDtSql(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (var item in dt.AsEnumerable())
                    {
                        list.Add(new do_list_out()
                        {
                            MedOnlyCode = item["MedOnlyCode"]?.ToString(),
                            MedName = item["MedName"].ToString(),
                            MedUnit = item["MedUnit"].ToString(),
                            MedPack = item["MedPack"].ToString(),
                            MedFactory = item["MedFactory"].ToString(),
                            MedOutAMT = float.Parse(item["MedOutAMT"].ToString()),
                            MedPos = item["MedPos"].ToString(),
                            PresNo = item["PresNo"].ToString(),
                            PName = item["PName"].ToString(),
                            MedValidTime = DateTime.Parse(item["MedValidTime"].ToString())
                        });
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<DrugDetail> GetDrugDetailByPYCode(string MedPYCode)
        {
            try
            {
                List<DrugDetail> DrugList = new List<DrugDetail>();
                string sql;
                if (string.IsNullOrEmpty(MedPYCode))
                {
                    sql = "SELECT top 20 MI.MedOnlyCode,MI.MedName,MI.MedUnit,MI.MedPack,MI.MedFactory,MedBarCode,MedMonitorCode FROM MedInfoTableHis MI left join Drugs_Ex_Info DI ON MI.MedOnlyCode=DI.MedOnlyCode  WHERE MedPYCode like'" + MedPYCode + "%'";
                }
                else
                    sql = "SELECT top 20 MI.MedOnlyCode,MI.MedName,MI.MedUnit,MI.MedPack,MI.MedFactory,MedBarCode,MedMonitorCode FROM MedInfoTableHis MI left join Drugs_Ex_Info DI ON MI.MedOnlyCode=DI.MedOnlyCode  WHERE MedPYCode like'%" + MedPYCode + "%'";
                DataTable dt = this.GetDtSql(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (var item in dt.AsEnumerable())
                    {
                        DrugList.Add(new DrugDetail()
                        {
                            MedOnlyCode = item["MedOnlyCode"]?.ToString(),
                            MedName = item["MedName"].ToString(),
                            MedUnit = item["MedUnit"].ToString(),
                            MedPack = item["MedPack"].ToString(),
                            MedFactory = item["MedFactory"].ToString(),
                            MedBarCode = string.IsNullOrEmpty(item["MedBarCode"].ToString()) ? string.Empty : item["MedBarCode"].ToString(),
                            MedMonitorCode = string.IsNullOrEmpty(item["MedMonitorCode"].ToString()) ? string.Empty : item["MedMonitorCode"].ToString()

                        });
                    }
                    return DrugList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        /// <summary>
        /// 根据药品条形码来显示
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public  List<DrugDetail> GetDrugInfosByBarCode(string BarCode)
        {
            try
            {
                List<DrugDetail> DrugList = new List<DrugDetail>();
                string sql = $"SELECT top 20 MI.MedOnlyCode,MI.MedName,MI.MedUnit,MI.MedPack,MI.MedFactory,MedBarCode,MedMonitorCode FROM MedInfoTableHis MI left join Drugs_Ex_Info DI ON MI.MedOnlyCode=DI.MedOnlyCode where DI.MedBarCode='{BarCode}'";
                DataTable dt = GetDtSql(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (var item in dt.AsEnumerable())
                    {
                        DrugList.Add(new DrugDetail()
                        {
                            MedOnlyCode = item["MedOnlyCode"]?.ToString(),
                            MedName = item["MedName"].ToString(),
                            MedUnit = item["MedUnit"].ToString(),
                            MedPack = item["MedPack"].ToString(),
                            MedFactory = item["MedFactory"].ToString(),
                            MedBarCode = string.IsNullOrEmpty(item["MedBarCode"].ToString()) ? string.Empty : item["MedBarCode"].ToString(),
                            MedMonitorCode = string.IsNullOrEmpty(item["MedMonitorCode"].ToString()) ? string.Empty : item["MedMonitorCode"].ToString()

                        });
                    }
                    return DrugList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新处方信息 OutFlag变成1
        /// </summary>
        /// <param name="outflag"></param>
        /// <param name="PresNo"></param>
        /// <param name="MedPos"></param>
        /// <param name="MachineID"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateDoListOut(string outflag, string PresNo, string MedPos, string MachineID)
        {
            try
            {
                string sql = "update do_list_out set OutFlag='" + outflag + "' where PresNO='" + PresNo + "' and MedPos='" + MedPos + "' and substring(medpos,1,2)='" + MachineID + "'";
                this.DoExceute(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 出药更新调配机药品存储数量 
        /// </summary>
        /// <param name="MedOutAMT"></param>
        /// <param name="medonlycode"></param>
        /// <param name="medpos"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateStoreTableAMT(double MedOutAMT, string medonlycode, string medpos)
        {
            try
            {
                string sql = "update storetable set mednowamt=mednowamt-'" + MedOutAMT + "' where medonlycode='" + medonlycode + "' and medpos='" + medpos + "'";
                this.DoExceute(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 添加药品
        /// </summary>
        /// <param name="MedNowAMT"></param>
        /// <param name="medonlycode"></param>
        /// <param name="medname"></param>
        /// <param name="medpos"></param>
        /// <param name="MachineID"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateStoreTableAMTAdd(int MedNowAMT, string medonlycode, string medname, string medpos, string MachineID)
        {
            try
            {                                        //update StoreTable set MedNowAMT={0}+(select MedNowAMT from StoreTable where  MedOnlyCode = '{1}' and MedName = '{2}' and MedPos = '{3}' and substring(medpos,1,2)= '{4}') where MedOnlyCode = '{5}' and MedName = '{6}' and MedPos = '{7}' and substring(medpos,1,2)= '{8}'", MedNowAMT, medonlycode, medname, medpos, MachineID, medonlycode, medname, medpos, MachineID
                string sql = string.Format("update StoreTable set MedNowAMT={0}+(select MedNowAMT from StoreTable where  MedOnlyCode='{1}' and MedName='{2}' and MedPos='{3}' and substring(medpos,1,2)='{4}') where MedOnlyCode='{5}' and MedName='{6}' and MedPos='{7}' and substring(medpos,1,2)='{8}'", MedNowAMT, medonlycode, medname, medpos, MachineID, medonlycode, medname, medpos, MachineID);
                this.DoExceute(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 盘点，直接更新当前数量
        /// </summary>
        public void UpdateStoreTableCheck(string medPos, int? Count)
        {
            try
            {
                string sql = string.Format("update StoreTable set MedNowAMT={0} where MedPos={1}", Count, medPos);
                this.DoExceute(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据条形码查询在调配机中的药品
        /// </summary>
        /// <returns></returns>
        public List<StoreTable> GetMedInStoreByBarCode(string BarCode)
        {
            try
            {
                string sql = $"select*from StoreTable st  left join Drugs_Ex_Info DI ON st.MedOnlyCode=DI.MedOnlyCode where MedBarCode='{BarCode}'";
                List<StoreTable> list = new List<StoreTable>();
                DataTable dt = this.GetDtSql(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (var item in dt.AsEnumerable())
                    {
                        list.Add(new StoreTable()
                        {
                            MedOnlyCode = item["MedOnlyCode"]?.ToString(),
                            MedName = item["MedName"].ToString(),
                            MedUnit = item["MedUnit"].ToString(),
                            MedPack = item["MedPack"].ToString(),
                            MedFactory = item["MedFactory"].ToString(),
                            MedNowAMT = int.Parse(item["MedNowAMT"].ToString()),
                            MedPos = item["MedPos"].ToString(),
                            MedPYCode = item["MedPYCode"].ToString(),
                            MedValidTime = string.IsNullOrEmpty(item["MedValidTime"].ToString()) ? null : Convert.ToDateTime(item["MedValidTime"])
                        });
                    }

                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据监管码查询在调配机中的药品
        /// </summary>
        /// <param name="MonitorCode"></param>
        /// <returns></returns>
        public List<StoreTable> GetMedInStoreByMonitorCode(string MedMonitorCode)
        {
            try
            {
                string sql = $"select*from StoreTable st  left join Drugs_Ex_Info DI ON st.MedOnlyCode=DI.MedOnlyCode where MedMonitorCode='{MedMonitorCode}'";
                List<StoreTable> list = new List<StoreTable>();
                DataTable dt = this.GetDtSql(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (var item in dt.AsEnumerable())
                    {
                        list.Add(new StoreTable()
                        {
                            MedOnlyCode = item["MedOnlyCode"]?.ToString(),
                            MedName = item["MedName"].ToString(),
                            MedUnit = item["MedUnit"].ToString(),
                            MedPack = item["MedPack"].ToString(),
                            MedFactory = item["MedFactory"].ToString(),
                            MedNowAMT = int.Parse(item["MedNowAMT"].ToString()),
                            MedPos = item["MedPos"].ToString(),
                            MedPYCode = item["MedPYCode"].ToString(),
                            MedValidTime = DateTime.Parse(item["MedValidTime"].ToString())
                        });
                    }

                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据拼音码查询
        /// </summary>
        /// <param name="MedPYCode"></param>
        /// <param name="MachineID"></param>
        /// <returns></returns>
        public List<StoreTable> GetMedInStoreByPYCode(string MedPYCode, string MachineID)
        {
            try
            {
                string sql = "";
                if (MedPYCode == "")
                {
                    sql = "select * from StoreTable st where MedPYCode like '" + MedPYCode + "%' and MedPos<>0 and substring(medpos,1,2)='" + MachineID + "' order by MedPos asc";
                }
                else
                {
                    sql = "select * from StoreTable st  where MedPYCode like '%" + MedPYCode + "%' and MedPos<>0 and substring(medpos,1,2)='" + MachineID + "' order by MedPos asc";
                }
                List<StoreTable> list = new List<StoreTable>();
                StoreTable table = null;
                using (DataTable dt = GetDtSql(sql))
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            table = new StoreTable();
                            table.MedOnlyCode = Convert.ToString(dr["MedOnlyCode"]);
                            table.MedName = string.IsNullOrEmpty(dr["MedName"].ToString()) == true ? "" : Convert.ToString(dr["MedName"]);
                            table.MedUnit = string.IsNullOrEmpty(dr["MedUnit"].ToString()) == true ? "" : Convert.ToString(dr["MedUnit"]);
                            table.MedPack = string.IsNullOrEmpty(dr["MedPack"].ToString()) == true ? "" : Convert.ToString(dr["MedPack"]);
                            table.MedNowAMT = string.IsNullOrEmpty(dr["MedNowAMT"].ToString()) == true ? 0 : Convert.ToInt32(dr["MedNowAMT"]);
                            //table.PosSafeAmt = string.IsNullOrEmpty(dr["PosSafeAmt"].ToString()) == true ? 0 : Convert.ToInt32(dr["PosSafeAmt"]);
                            //table.PosMaxAmt = string.IsNullOrEmpty(dr["PosMaxAmt"].ToString()) == true ? 0 : Convert.ToInt32(dr["PosMaxAmt"]);
                            table.MedPos = string.IsNullOrEmpty(dr["MedPos"].ToString()) == true ? "000000" : Convert.ToString(dr["MedPos"]);
                            table.MedPYCode = string.IsNullOrEmpty(dr["MedPYCode"].ToString()) == true ? "" : Convert.ToString(dr["MedPYCode"]);
                            // table.MedBatchNO = string.IsNullOrEmpty(dr["MedBatchNO"].ToString()) == true ? "" : dr["MedBatchNO"].ToString();
                            table.MedFactory = string.IsNullOrEmpty(dr["MedFactory"].ToString()) == true ? "" : dr["MedFactory"].ToString();
                            //    table.MachineID = Convert.ToString(dr["MachineID"]);
                            //table.MedConvercof = string.IsNullOrEmpty(dr["MedConvercof"].ToString()) == true ? 1 : Convert.ToInt32(dr["MedConvercof"]);
                            if (!string.IsNullOrEmpty(dr["MedValidTime"].ToString()))
                            {
                                table.MedValidTime = Convert.ToDateTime(dr["MedValidTime"]);
                            }
                            list.Add(table);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据位置找到该药品的信息
        /// </summary>
        /// <param name="MedPos"></param>
        /// <param name="MachineId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<StoreTable> GetMedInStoreByMedPos(string MedPos, string MachineId)
        {
            try
            {
                string sql = $"select*from StoreTable where MedPos='{MedPos}'";
                List<StoreTable> list = new List<StoreTable>();
                DataTable dt = this.GetDtSql(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (var item in dt.AsEnumerable())
                    {
                        list.Add(new StoreTable()
                        {
                            MedOnlyCode = item["MedOnlyCode"]?.ToString(),
                            MedName = item["MedName"].ToString(),
                            MedUnit = item["MedUnit"].ToString(),
                            MedPack = item["MedPack"].ToString(),
                            MedFactory = item["MedFactory"].ToString(),
                            MedNowAMT = int.Parse(item["MedNowAMT"].ToString()),
                            MedPos = item["MedPos"].ToString(),
                            MedPYCode = item["MedPYCode"].ToString(),
                            MedValidTime = GetDateTime(item)
                        });
                    }

                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 更新条形码和监管码
        /// </summary>
        /// <param name="MedOnlyCode"></param>
        /// <param name="MedName"></param>
        /// <param name="MedBarCode"></param>
        /// <param name="MedMonitorCode"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateDrugCode(string MedOnlyCode,string MedName,string? MedBarCode,string ?MedMonitorCode)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@MedOnlyCode",MedOnlyCode),
                new SqlParameter("@MedName",MedName),
                new SqlParameter("@MedBarCode",MedBarCode),
                                   
                new SqlParameter("@MedMonitorCode",MedMonitorCode)
                };
                this.DoExceuteProcedure("P_TPJ_UpdateDrugBarCode", parameters);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除条形码和监管码
        /// </summary>
        /// <param name="BarCode"></param>
        /// <param name="MonitorCode"></param>
        /// <exception cref="Exception"></exception>
        public  void DeleteBarCode(string BarCode, string MonitorCode)
        {
            try
            {
              
                string sql;
                if (!string.IsNullOrEmpty(BarCode))
                {
                    sql = "update Drugs_Ex_Info set medbarcode=null,medMonitorCode=null where medbarcode='" + BarCode + "' ";
                }
                else
                {
                    sql = "update Drugs_Ex_Info set medbarcode=null,medMonitorCode=null where  MedMonitorCode=like '%" + MonitorCode + "%'";
                }
                DoExceute(sql);       
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DateTime? GetDateTime(DataRow item)
        {
            if (item["MedValidTime"] != null && !string.IsNullOrEmpty(item["MedValidTime"].ToString()))
            {
                return DateTime.Parse(item["MedValidTime"].ToString());
            }
            return null;
        }
        #endregion 
    }
}
