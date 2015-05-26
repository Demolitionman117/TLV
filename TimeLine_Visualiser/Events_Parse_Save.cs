using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLine_Visualiser
{
    class Events_Parse_Save
    {


       

        Uri DatabaseCredintals;
        public static bool Aborted;

       public Events_Parse_Save()
        {
            Aborted = false;

        }



        public void parse(string filename)
        {
            MainWindow.main.StatusMsg = "Parsing File... ";
            StreamReader reader = File.OpenText(@filename);


            string line;

            while ((line = reader.ReadLine()) != null && Aborted == false)
            {
                //Create new class for each line of data. 
                EventObjects EObject = new EventObjects();

                TimeNode EObjectTime = new TimeNode(); 

                string[] Filedata;

                line = reader.ReadLine();
                Filedata = line.Split('|');

                  
                    EObjectTime.Time = UnixTimeStampToDateTime(Filedata[0]).ToString();  // Save to the Time Object for node creation.
                    CreateTimeNode(EObjectTime); //create node in the database. 

                    EObject.Time = UnixTimeStampToDateTime(Filedata[0]);//Save to ojbect data anyway for controll.
                    

                    EObject.Source = Filedata[1];  
                    
                    EObject.SystemName = Filedata[2];
                    

                    EObject.UserName = Filedata[3];

                     

                    EObject.Description = RemoveSpecialCharacters(Filedata[4]); //Clean and save the Description. {The Database is angry with special chars} 
                   


                    //Create the event node. 
                    CreateDataNodes(EObject);

                 //Create relations between event and time. 
                CreatTypeAndTimeRealtions(EObject, EObjectTime);

            }

        }



        //To convert Unix Timestamp to dateTime
        DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
           double parsedunixTimeStamp = Double.Parse(unixTimeStamp);
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(parsedunixTimeStamp).ToLocalTime();

            return dtDateTime;
        }


        public void CreateCredUri(string DatabaseUserName, string DatabasePassword, string DatabaseUrl)
        {
            Uri DataBaseCreds = new Uri("http://" + DatabaseUserName + ':' + DatabasePassword + '@' + DatabaseUrl + "/db/data");
            DatabaseCredintals = DataBaseCreds;

            Console.WriteLine(DataBaseCreds);
        }


        //Creates an uniq node with the datasoruce. 
        void CreateTimeNode(TimeNode timeNode)
        {
            MainWindow.main.StatusMsg = "Creating Time-Node... ";

            Neo4jEasyClient dbclient = new Neo4jEasyClient();

            dbclient.Connect(DatabaseCredintals);

            dbclient.CreateNode("EventTime","time",timeNode, "Time");

        }


        void CreateDataNodes(EventObjects EventObject)
        {
            MainWindow.main.StatusMsg = "Creating Event-Node... ";

            Neo4jEasyClient dbclient = new Neo4jEasyClient();

            dbclient.Connect(DatabaseCredintals);

            dbclient.CreateNode(EventObject.Source,EventObject.Source,EventObject,"Description");

        }

        //Create data Relations, Description and time.
        void CreatTypeAndTimeRealtions(EventObjects EventObject, TimeNode timeNode)
        {
            MainWindow.main.StatusMsg = "Creating Relationships Event -> Time... ";
      

            Neo4jEasyClient dbclient = new Neo4jEasyClient();

            dbclient.Connect(DatabaseCredintals);

            dbclient.CreateRelationShipDirectedN1TON2(EventObject.Source,"Description",EventObject.Description,"EventTime","Time",timeNode.Time,"LastWrite");


        }

        // To clean the Description data.
        string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c==' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

    }
}
