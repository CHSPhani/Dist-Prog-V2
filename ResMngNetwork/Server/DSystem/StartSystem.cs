using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DSystem
{
    public class SystemStartEventArgs : EventArgs
    {
        public ServerStates StatusMessage { get; set; }

        public List<DSNode> ActiveNodes { get; set; }
        public SystemStartEventArgs(ServerStates msg, List<DSNode> aNodes)
        {
            StatusMessage = msg;
            ActiveNodes = aNodes;
        }
    }

    public class InitSetupEventArgs:EventArgs
    {
        public string InitArgs { get; set; }

        public InitSetupEventArgs()
        {

        }
    }

    public delegate void SystemStartedEventHandler(object sender, SystemStartEventArgs e);

    public delegate void InitialSetupEventHandler(object sender, InitSetupEventArgs e);

    public enum ServerStates
    {
        SystemNotStarted,
        SystemStarted,
        VotingStarted,
        VotingFinished,
        SystemShutDown
    }

    public class StartSystem
    {
        public string workingDir;

        public string dataDir;

        public int noOfUsers;

        public int noOfAUSers;

        public bool initSetupDone;

        public ServerStates sStatus;

        public event SystemStartedEventHandler SsEventHandler;
        public event InitialSetupEventHandler IsuEventHandler;

        List<DSNode> dsNodeList;

        public StartSystem()
        {
            workingDir = dataDir = string.Empty;
            noOfUsers = noOfAUSers = 0;
            initSetupDone = false;
            sStatus = ServerStates.SystemNotStarted;
            dsNodeList = new List<DSNode>();
        }

        public StartSystem(string wDir, string dDir, int noUsers, int noAUser, bool initSetup) : this()
        {
            workingDir = wDir;
            dataDir = dDir;
            noOfUsers = noUsers;
            noOfAUSers = noAUser;
            initSetupDone = initSetup;
        }

        public void SystemStart()
        {
            if (sStatus == ServerStates.SystemStarted)
                return;
            if (initSetupDone)
            {
                //initial Setup not done. REvoke what you did..
                if (System.IO.Directory.Exists(workingDir))
                {
                    string[] dirs = Directory.GetDirectories(workingDir,"*", SearchOption.TopDirectoryOnly);
                    foreach(string s in dirs)
                    {
                        DirectoryInfo di = new DirectoryInfo(s);
                        int counter = 0;
                        Int32.TryParse(di.Name, out counter);
                        if (counter != 0)
                        {
                            DSNode clSystem = new DSNode();
                            CreateClientSystems(counter, ref clSystem);
                            if (clSystem != null)
                            {
                                string pathString = string.Format("{0}\\{1}", workingDir, counter);
                                string dataPathString = string.Format("{0}\\{1}", pathString, "InitialDB.dat");
                                clSystem.WorkingDir = pathString;
                                clSystem.DataDir = dataPathString;
                                clSystem.DeSerializeInitData(dataPathString);
                                clSystem.Status = "Initialized";
                                dsNodeList.Add(clSystem);
                            }
                        }
                        else
                        {
                            DSNode clSystem = new DSNode();
                            clSystem.UserName = "AUser";
                            clSystem.UserRole = MemberRole.NonVotingMember;
                            if (clSystem != null)
                            {
                                string pathString = string.Format("{0}\\{1}", workingDir, di.Name);
                                string dataPathString = string.Format("{0}\\{1}", pathString, "InitialDB.dat");
                                clSystem.WorkingDir = pathString;
                                clSystem.DataDir = dataPathString;
                                clSystem.DeSerializeInitData(dataPathString);
                                clSystem.Status = "Initialized";
                                dsNodeList.Add(clSystem);
                            }
                        }
                    }
                }
            }
            else
            {
                //initial Setup not done. Let us do that
                //1. Create Folders in working dir for all users with numbers.
                int counter = 1;
                while (counter <= noOfUsers)
                {
                    DSNode clSystem = new DSNode();
                    CreateClientSystems(counter, ref clSystem);
                    if (clSystem != null)
                    {
                        string pathString = string.Format("{0}\\{1}", workingDir, counter);
                        string dataPathString = string.Format("{0}\\{1}", pathString, "InitialDB.dat");
                        if (!System.IO.Directory.Exists(pathString))
                        {
                            System.IO.Directory.CreateDirectory(pathString);
                            System.IO.File.Copy(dataDir, dataPathString, true);
                        }
                        clSystem.WorkingDir = pathString;
                        clSystem.DataDir = dataPathString;
                        clSystem.DeSerializeInitData(dataPathString);
                    }
                    clSystem.Status = "Initialized";
                    counter++;
                    dsNodeList.Add(clSystem);
                }
                counter = 1; //reset for anonymoususers
                while (counter <= noOfAUSers)
                {
                    DSNode clSystem = new DSNode();
                    clSystem.UserName = "AUser";
                    clSystem.UserRole = MemberRole.NonVotingMember;
                    string folderName = string.Format("AU{0}", counter);
                    if (clSystem != null)
                    {
                        string pathString = string.Format("{0}\\{1}", workingDir, folderName);
                        string dataPathString = string.Format("{0}\\{1}", pathString, "InitialDB.dat");
                        if (!System.IO.Directory.Exists(pathString))
                        {
                            System.IO.Directory.CreateDirectory(pathString);
                            System.IO.File.Copy(dataDir, dataPathString, true);
                        }
                        clSystem.WorkingDir = pathString;
                        clSystem.DataDir = dataPathString;
                        clSystem.DeSerializeInitData(dataPathString);
                    }
                    clSystem.Status = "Initialized";
                    counter++;
                    dsNodeList.Add(clSystem);
                }

                initSetupDone = true;
                IsuEventHandler?.Invoke(this, new InitSetupEventArgs() { InitArgs = "true" });
            }
            //2.Establish Channels for each DSNode
            NetworkFunctions.CreateIncidents(ref dsNodeList);
           
            //3. Create Visual Forms for each DSNode
            foreach (DSNode clSystem in dsNodeList)
            {
                if (clSystem.UserName.Equals("AUser")) //Dont want to show screen for anonymous users
                    continue;
                NodeWindow clWinow = new NodeWindow(clSystem);
                clWinow.Show();
            }
            sStatus = ServerStates.SystemStarted;
            SsEventHandler?.Invoke(this, new SystemStartEventArgs(ServerStates.SystemStarted, dsNodeList));
        }
        
        /// <summary>
        /// This is hardcoded for initial round as this is a PoC, This is OK
        /// LAter we can write a user registration module and get user info from there. 
        /// </summary>
        void CreateClientSystems(int userNo, ref DSNode cSystem)
        {
            switch (userNo)
            {
                case 1:
                    {
                        cSystem.UserName = "User1";
                        cSystem.UserRole = MemberRole.ExecutiveMember;
                        break;
                    }
                case 2:
                    {
                        cSystem.UserName = "User2";
                        cSystem.UserRole = MemberRole.ExecutiveMember;
                        break;
                    }
                case 3:
                    {
                        cSystem.UserName = "User3";
                        cSystem.UserRole = MemberRole.ExecutiveMember;
                        break;
                    }
                case 4:
                    {
                        cSystem.UserName = "User4";
                        cSystem.UserRole = MemberRole.NonVotingMember;
                        break;
                    }
                case 5:
                    {
                        cSystem.UserName = "User5";
                        cSystem.UserRole = MemberRole.ExecutiveMember;
                        break;
                    }
                default:
                    {
                        cSystem = null;
                        break;
                    }
            }
        }      
    }
    /// <summary>
    /// Three Roles Execution Environment, Participate in Vote, Contribute Memory
    ///                     EE  PV CM    
    /// Supervizor          Y   Y   Y
    /// ExecutiveMember     Y   Y   Y
    /// NonExecutiveMeMber  N   Y   Y
    /// NonVotingMember     N   N   Y
    /// MuteMember          Y   N   Y
    /// </summary>
    public enum MemberRole
    {
        Supervizor, //Supervisors the system
        ExecutiveMember, //Member where the execution engine participate in voting and memory contributed
        NonExecutiveMember, //No execution environment but memory contributed and participate in voting
        NonVotingMember, //No execution environment and no participate in voting but memory contributed 
        MuteMember, //No participate in voting, but contributes execution environment and memory.
        None // DEfault value
    }

    /// <summary>
    /// This enum tells what the cause of proposal
    /// </summary>
    public enum ProposalCause
    {
        NewPackage,
        NewClass,
        NewProperty,
        NewOntology,
        NewOClass,
        NewDataProperty,
        NewDomainToDP,
        NewRstr,
        NewConversion,
        ModifyDataProperty,
        NewObjectProperty,
        ModifyObjectProperty,
        UploadInd,
        None
    }

    /// <summary>
    /// This enum tells what is the type of proposal
    /// </summary>
    public enum ProposalType
    {
        Voting,
        Transition,
        None
    }

    public enum MessageType
    {
        outwards,
        inwards
    }
}
