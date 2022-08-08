using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegModule.Models;

namespace UserRegModule.DAtaOps
{
    public class SaveToDB
    {
        public static bool SaveNewUserDetails(NewUserDataModel nDet, MySqlConnection conn)
        {
            try
            {
                string nDP = string.Empty;
                string nRT = string.Empty;
                StringBuilder sb = new StringBuilder();
                foreach (NUDataProperty s in nDet.NUDataProps)
                {
                    sb.Append(s.ToString());
                    sb.Append(">");
                }
                nDP = sb.ToString().Substring(0, sb.ToString().Length - 1); //remove last : 
                sb.Clear();
                foreach (NURestriction n in nDet.NURestrictions)
                {
                    sb.Append(n.ToString());
                    sb.Append(">");
                }
                nRT = sb.ToString().Substring(0, sb.ToString().Length - 1); //remove last : 
                //Check do we have this user alredy?
                if (!SaveToDB.NewUserExists(nDet.AUName, conn))
                {
                    //Need to modify the existing user. 
                    int nID = SaveToDB.GetNewUserID(nDet.AUName, conn);
                    string mCmd = string.Format("UPDATE newuser SET nuAName = '{1}', nuDP = '{2}',nuRT ='{3}',nuFName ='{4}',nuPhone ='{5}',nuEmail ='{6}',nuID ='{7}',nupwd ='{8}' WHERE nID={0}", 
                                                    nID, nDet.AUName, nDP, nRT, nDet.UFName, nDet.UEPhone, nDet.UEMail, nDet.UId, nDet.UPwd);
                    MySqlCommand cmd = new MySqlCommand(mCmd, conn);
                    cmd.ExecuteNonQuery();
                    System.Threading.Thread.Sleep(100 * 1);//sleep for 2 ms just to ensure everything is OK..
                }
                else
                {
                    //Save
                    int nid = SaveToDB.GetMaxID("nID", "newuser", conn);
                    string icmd = string.Format("INSERT INTO newuser(nID,nuAName, nuDP,nuRT,nuFName,nuPhone,nuEmail,nuID,nupwd) VALUES" +
                                               "({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", nid, nDet.AUName, nDP, nRT,
                                               nDet.UFName, nDet.UEPhone, nDet.UEMail, nDet.UId, nDet.UPwd);
                    MySqlCommand cmd = new MySqlCommand(icmd, conn);
                    cmd.ExecuteNonQuery();
                    System.Threading.Thread.Sleep(100 * 1);//sleep for 2 ms just to ensure everything is OK..
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while saving New user det to DB. Reason is {0}", ex.Message));
                return false;
            }
            Console.WriteLine(string.Format("Saved New user det to DB."));
            return true;
        }
        public static bool SaveALDDetails(ADevDataModel aDet, MySqlConnection conn)
        {
            try
            {
                //add selected DS to a string
                StringBuilder sb = new StringBuilder();
                foreach (string s in aDet.SelDS)
                {
                    sb.Append(s);
                    sb.Append(":");
                }
                string selDS = sb.ToString().Substring(0, sb.ToString().Length - 1); //remove last : 
                //Save
                int aid = SaveToDB.GetMaxID("aID", "aldeveloper", conn);
                string icmd = string.Format("INSERT INTO aldeveloper(aID,adID,adName,adAddress,adURole,adFName,adPhone,adEmail,adpwd) VALUES" +
                                            "({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", aid, aDet.UId, aDet.UName, aDet.UAddress,
                                               selDS, aDet.UFName, aDet.UEPhone, aDet.UEMail, aDet.UPwd);
                MySqlCommand cmd = new MySqlCommand(icmd, conn);
                cmd.ExecuteNonQuery();
                System.Threading.Thread.Sleep(100 * 1);//sleep for 2 ms just to ensure everything is OK..
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while saving Algorithm develope det to DB. Reason is {0}", ex.Message));
                return false;
            }
            Console.WriteLine(string.Format("Saved Algorithm develope det to DB."));
            return true;
        }
        public static bool SaveSUDetails(SmartUserDataModel sDet, MySqlConnection conn)
        {
            try
            {
                int sid = SaveToDB.GetMaxID("sID", "smartuser", conn);
                string icmd = string.Format("INSERT INTO smartuser(sID,suID,suName,suAddress,suLoad,suFMethod,suFName,suPhone,suEmail,supwd) VALUES" +
                                             "({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", sid, sDet.UId, sDet.UName, sDet.UAddress,
                                               sDet.SLoad, sDet.SFOption, sDet.UFName, sDet.UEPhone, sDet.UEMail, sDet.UPwd);
                MySqlCommand cmd = new MySqlCommand(icmd, conn);
                cmd.ExecuteNonQuery();
                System.Threading.Thread.Sleep(100 * 1);//sleep for 2 ms just to ensure everything is OK..
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while saving Smart user det to DB. Reason is {0}", ex.Message));
                return false;
            }
            Console.WriteLine(string.Format("Saved Smart user det to DB."));
            return true;
        }
        public static bool SaveProsumerDet(ProsumerDataModel pDet, MySqlConnection conn)
        {
            try
            {
                int pid = SaveToDB.GetMaxID("pID", "prosumer", conn);
                string icmd = string.Format("INSERT INTO prosumer (pID,prID,prName,prAddress,prLoad,prPVSystem,prFMethod,prFName,prPhone,prEmail,prpwd)  VALUES " +
                                           "({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", pid, pDet.UId, pDet.UName, pDet.UAddress,
                                               pDet.SLoad, pDet.SPvPanel, pDet.SFOption, pDet.UFName, pDet.UEPhone, pDet.UEMail, pDet.UPwd);
                MySqlCommand cmd = new MySqlCommand(icmd, conn);
                cmd.ExecuteNonQuery();
                System.Threading.Thread.Sleep(100 * 1);//sleep for 2 ms just to ensure everything is OK..
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while saving PRosumer det to DB. Reason is {0}", ex.Message));
                return false;
            }
            Console.WriteLine(string.Format("Saved Prosumer det to DB."));
            return true;
        }
        public static bool SaveConsumerDet(ConsumerDataModel cDet, MySqlConnection conn)
        {
            try
            {
                int cid = SaveToDB.GetMaxID("cID", "consumer", conn);
                string icmd = string.Format("INSERT INTO consumer (cID,cuID,cuName,cuAddress,cDuration ,iInRE ,cuLoad,cuFMethod,cuFName,cuPhone ,cuEmail,cupwd)  VALUES " +
                                            "({0},'{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}')", cid, cDet.UId, cDet.UName, cDet.UAddress,
                                                cDet.CDuration, cDet.IntInRE, cDet.SLoad, cDet.SFOption, cDet.UFName, cDet.UEPhone, cDet.UEMail, cDet.UPwd);
                MySqlCommand cmd = new MySqlCommand(icmd, conn);
                cmd.ExecuteNonQuery();
                System.Threading.Thread.Sleep(100 * 1);//sleep for 2 ms just to ensure everything is OK..
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while saving Consumer det to DB. Reason is {0}", ex.Message));
                return false;
            }
            Console.WriteLine(string.Format("Saved Consumer det to DB."));
            return true;
        }
        public static int GetMaxID(string cName, string tableName, MySqlConnection conn)
        {
            int nID = 0;
            try
            {
                string sql = string.Format("select max({0}) from {1}", cName, tableName);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        nID = Int32.Parse(rdr[0].ToString());
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while getting MAX {0} from table{1}. Reason is {2}", cName, tableName, ex.Message));
                return -1;
            }
            return nID + 1;
        }
        public static UserStats GetUserStats(MySqlConnection conn)
        {
            UserStats us = new UserStats();
            try
            {
                string sql = string.Format("select count(*) from prosumer");
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        us.NOOfProsumers = Int32.Parse(rdr[0].ToString());
                    }
                }

                rdr.Close();
                sql = string.Empty;
                sql = string.Format("select count(*) from consumer");
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        us.NOOfConsumers = Int32.Parse(rdr[0].ToString());
                    }
                }

                rdr.Close();
                sql = string.Empty;
                sql = string.Format("select count(*) from smartuser");
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        us.NOSU = Int32.Parse(rdr[0].ToString());
                    }
                }

                rdr.Close();
                sql = string.Empty;
                sql = string.Format("select count(*) from aldeveloper");
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        us.NOAD = Int32.Parse(rdr[0].ToString());
                    }
                }


                rdr.Close();
                sql = string.Empty;
                sql = string.Format("select count(*) from newuser");
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        us.NONU = Int32.Parse(rdr[0].ToString());
                    }
                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                return null;
            }
            return us;
        }
        public static List<string> GetUserRoles(string uId, MySqlConnection conn)
        {
            List<string> uRoles = new List<string>();
            int count = 0;
            try
            {
                string sql = string.Format("select count(*) from prosumer where prID='{0}'", uId);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        count = Int32.Parse(rdr[0].ToString());
                    }
                }
                if (count > 0)
                    uRoles.Add("prosumer");

                rdr.Close();
                sql = string.Empty;
                count = 0;
                sql = string.Format("select count(*) from consumer where cuID='{0}'", uId);
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        count = Int32.Parse(rdr[0].ToString());
                    }
                }
                if (count > 0)
                    uRoles.Add("consumer");

                rdr.Close();
                sql = string.Empty;
                count = 0;
                sql = string.Format("select count(*) from smartuser where suID='{0}'", uId);
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        count = Int32.Parse(rdr[0].ToString());
                    }
                }
                if (count > 0)
                    uRoles.Add("smartuser");

                rdr.Close();
                sql = string.Empty;
                count = 0;
                sql = string.Format("select count(*) from aldeveloper where adID='{0}'", uId);
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        count = Int32.Parse(rdr[0].ToString());
                    }
                }
                if (count > 0)
                    uRoles.Add("aldeveloper");

                rdr.Close();
                sql = string.Empty;
                count = 0;
                sql = string.Format("select count(*) from newuser where nuID='{0}'", uId);
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        count = Int32.Parse(rdr[0].ToString());
                    }
                }
                if (count > 0)
                    uRoles.Add("newuser");
                rdr.Close();
            }
            catch (Exception ex)
            {
                return null;
            }
            return uRoles;
        }
        public static bool NewUserExists(string auName, MySqlConnection conn)
        {
            int count = 0;
            try
            {
                string sql = string.Format("SELECT count(*) FROM newuser WHERE nuAName ='{0}'", auName);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        count = Int32.Parse(rdr[0].ToString());
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while checking new user already exists. Reason is {0}", ex.Message));
                return false;
            }
            return (count == 0);
        }
        public static int GetNewUserID(string auName, MySqlConnection conn)
        {
            int id = 0;
            try
            {
                string sql = string.Format("SELECT nID FROM newuser WHERE nuAName ='{0}'", auName);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        id = Int32.Parse(rdr[0].ToString());
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while checking new user already exists. Reason is {0}", ex.Message));
                return 0;
            }
            return id;
        }
        public static string GetADURole(string adName, MySqlConnection conn)
        {
            string uRole = string.Empty;
            try
            {
                string sql = string.Format("SELECT adURole FROM aldeveloper WHERE adID ='{0}'", adName);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        uRole = rdr[0].ToString();
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while checking new user already exists. Reason is {0}", ex.Message));
                return string.Empty;
            }
            return uRole;
        }
        public static NewUserDataModel GetExistingUSer(string auName, MySqlConnection conn)
        {
            NewUserDataModel ndM = new NewUserDataModel();
            try
            {
                string sql = string.Format("SELECT * FROM newuser WHERE nuAName ='{0}'", auName);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr1 = cmd.ExecuteReader();
                if (rdr1.Read())
                {
                    ndM.NID = Int32.Parse(rdr1[0].ToString());
                    ndM.AUName = rdr1[1].ToString();
                    //Process DP
                    List<string> dps = rdr1[2].ToString().Split('>').ToList<string>();
                    foreach (string s in dps)
                    {
                        NUDataProperty nup = new NUDataProperty();
                        string[] ddps = s.Split('&');
                        nup.NUDPName = ddps[0];
                        nup.SNudpType = ddps[1];
                        nup.NUDPExpr = ddps[2];
                        ndM.NUDataProps.Add(nup);
                    }
                    //Process RT
                    List<string> rts = rdr1[3].ToString().Split('>').ToList<string>();
                    foreach (string s in rts)
                    {
                        NURestriction ntr = new NURestriction();
                        string[] ddrt = s.Split('&');
                        ntr.NURName = ddrt[0];
                        ntr.TargetOName = ddrt[1];
                        ntr.SRName = ddrt[2];
                        ndM.NURestrictions.Add(ntr);
                    }
                    ndM.UFName = rdr1[4].ToString();
                    ndM.UEPhone = rdr1[5].ToString();
                    ndM.UEMail = rdr1[6].ToString();
                    ndM.UId = rdr1[7].ToString();
                    ndM.UPwd = rdr1[8].ToString();
                }
                rdr1.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(string.Format("Error while getting new user already exists. Reason is {0}", ex.Message));
                return null;
            }
            return ndM;
        }
        public static bool CheckPassword(string uName, string pwd, string tName, MySqlConnection conn)
        {
            string apwd = string.Empty;
            try
            {
                string sql = string.Empty;
                if (tName == "prosumer")
                    sql = string.Format("select prpwd from {0} where prID = '{1}'", tName, uName);
                else if (tName == "consumer")
                    sql = string.Format("select cupwd from {0} where cuID = '{1}'", tName, uName);
                else if (tName == "smartuser")
                    sql = string.Format("select supwd from {0} where suID = '{1}'", tName, uName);
                else if (tName == "aldeveloper")
                    sql = string.Format("select adpwd from {0} where adID = '{1}'", tName, uName);
                else if (tName == "newuser")
                    sql = string.Format("select nupwd from {0} where nuID = '{1}'", tName, uName);

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        apwd = rdr[0].ToString();
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while getting password from table{0}. Reason is {1}", tName, ex.Message));
                return false;
            }
            return (apwd == pwd);
        }

        public static int GetADId(string adName, MySqlConnection conn)
        {
            int aid = -1;
            try
            {
                string sql = string.Format("SELECT aID FROM aldeveloper WHERE adID ='{0}'", adName);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        aid = Int32.Parse(rdr[0].ToString());
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while getting aID from aldeveloper table. Reason is {0}", ex.Message));
                return -1;
            }
            return aid;
        }

        public static int GetADSId(string adsName, MySqlConnection conn)
        {
            int aid = -1;
            try
            {
                string sql = string.Format("SELECT adSID FROM aldevconfig1 WHERE adcSInstName ='{0}'", adsName);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        aid = Int32.Parse(rdr[0].ToString());
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while getting aID from aldeveloper table. Reason is {0}", ex.Message));
                return -1;
            }
            return aid;
        }

        public static int GetADPId(string adsName, MySqlConnection conn)
        {
            int aid = -1;
            try
            {
                string sql = string.Format("SELECT adSDS FROM aldevconfig2 WHERE adcDSName ='{0}'", adsName);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr[0].ToString()))
                    {
                        aid = Int32.Parse(rdr[0].ToString());
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while getting aID from aldeveloper table. Reason is {0}", ex.Message));
                return -1;
            }
            return aid;
        }

        public static bool SaveADC1Details(int adsid, int aid, string instName, MySqlConnection conn)
        {
            try
            {
                string icmd = string.Format("INSERT INTO aldevconfig1(adSID, aID, adcSInstName) VALUES" +
                                           "({0},{1},'{2}')", adsid, aid, instName);
                MySqlCommand cmd = new MySqlCommand(icmd, conn);
                cmd.ExecuteNonQuery();
                System.Threading.Thread.Sleep(100 * 1);//sleep for 2 ms just to ensure everything is OK..

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while saving aldevconfig1 to DB. Reason is {0}", ex.Message));
                return false;
            }
            Console.WriteLine(string.Format("Saved New user det to DB."));
            return true;
        }

        public static bool SaveADC2Details(int adsid, int adsds, string dsName, MySqlConnection conn)
        {
            try
            {
                string icmd = string.Format("INSERT INTO aldevconfig2(adSID, adSDS, adcDSName) VALUES" +
                                           "({0},{1},'{2}')", adsid, adsds, dsName);
                MySqlCommand cmd = new MySqlCommand(icmd, conn);
                cmd.ExecuteNonQuery();
                System.Threading.Thread.Sleep(100 * 1);//sleep for 2 ms just to ensure everything is OK..

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while saving aldevconfig2 to DB. Reason is {0}", ex.Message));
                return false;
            }
            Console.WriteLine(string.Format("Saved New user det to DB."));
            return true;
        }

        public static bool SaveADC3Details(int adsds, int adsdp, string dpName, MySqlConnection conn)
        {
            try
            {
                string icmd = string.Format("INSERT INTO aldevconfig3(adSDS, adSDP, adcDPName) VALUES" +
                                           "({0},{1},'{2}')", adsds, adsdp, dpName);
                MySqlCommand cmd = new MySqlCommand(icmd, conn);
                cmd.ExecuteNonQuery();
                System.Threading.Thread.Sleep(100 * 1);//sleep for 2 ms just to ensure everything is OK..

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error while saving aldevconfig3 to DB. Reason is {0}", ex.Message));
                return false;
            }
            Console.WriteLine(string.Format("Saved New user det to DB."));
            return true;
        }

    }
}
