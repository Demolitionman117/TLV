using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLine_Visualiser
{
    class Neo4jEasyClient
    {


        private GraphClient client;

        


        public Neo4jEasyClient()
        { }




        public void Connect(Uri uri)
        {
            //Creates a new GraphClient, and connect to the specified uri 



            try
            {
                this.client = new GraphClient(uri);
                client.Connect();

                //{For debug purp. }
               // MainWindow.main.StatusMsg = ("Connected to Database...");

            }
            catch (Exception e)
            {
                //Temporary handled Exception
              //  MainWindow.main.ConsoleOutPut = ("Can not connect to database, Something is Wrong" + e.Message);

                MainWindow.main.StatusMsg = ("Something is wrong... no database connection");
            }


        }

        //Creates A node, Unuqe.
        //The node is created with propeties Inherited from the ObjectData. 
        public void CreateNode(string Label, string objecttype, object ObjectInstance, string UniqueProp)
        {
            //Equivlant with .Create( "user:User{user}").WithParam("user",user");
            // client.Cypher.Create("(n:" + Label + ('{' + objecttype + '}') + ")").WithParam(objecttype, ObjectInstance).ExecuteWithoutResults(); 
            //---The Above is the old one----- Creates with no unique value ------ // 

            
            client.Cypher
                .Merge("(n:" + Label + ('{' + UniqueProp  + ':' + '{' + objecttype + '}' + '.' + UniqueProp + '}') + ")")
                .OnCreate()
                .Set("n ={" + objecttype + '}')
                .WithParam(objecttype, ObjectInstance)
                .ExecuteWithoutResults();
            
        }

        //Delete node for string values
        public void DeleteNode(string Label, string Propertie, string Value)
        {

            client.Cypher.Match("(n:" + Label + ')')
                         .Where("n." + Propertie + "=" + '"' + Value + '"')
                         .Delete("n")
                         .ExecuteWithoutResults();


        }

        //Delete node for int values.
        public void DeleteNode(string Label, string Propertie, int Value)
        {

            client.Cypher.Match("(n:" + Label + ')')
                         .Where("n." + Propertie + "=" + Value)
                         .Delete("n")
                         .ExecuteWithoutResults();


        }


        //For propeties with string values. 
        public IEnumerable<T> FindNodeByPropValue<T>(string Label, string Propertie, string Value)
        {

            // client.Cypher.Match("(n:"+Label+('{'+Propertie+':'+Value+'}')).Return(u => u.As<AsObject>());



            IEnumerable<T> ReturnList = client.Cypher.Match("(n:" + Label + ')')
                                .Where("n." + Propertie + "=" + '"' + Value + '"')
                                .Return(n => n.As<T>())
                                .Results;



            return ReturnList;
        }

        //For propeties with int values. 
        public IEnumerable<T> FindNodeByPropValue<T>(string Label, string Propertie, int Value)
        {

            // client.Cypher.Match("(n:"+Label+('{'+Propertie+':'+Value+'}')).Return(u => u.As<AsObject>());



            IEnumerable<T> ReturnList = client.Cypher.Match("(n:" + Label + ')')
                                .Where("n." + Propertie + "=" + Value)
                                .Return(n => n.As<T>())
                                .Results;



            return ReturnList;
        }


        //Finds Nodes by specifik prop, edits or creates a new prop with new int value.
        public void FindAndUpdateNode(string Label, string FindbyPopertie, string FindbyValue, string newPopertie, int newValue)
        {

            client.Cypher.Match("(n:" + Label + ')')
                                .Where("n." + FindbyPopertie + "=" + '"' + FindbyValue + '"')
                                .Set("n." + newPopertie + "=" + newValue)
                                .ExecuteWithoutResults();

        }

        //Finds Nodes by specifik prop, edits or creates a new prop with new String value.
        public void FindAndUpdateNode(string Label, string FindbyPopertie, string FindbyValue, string newPopertie, string newValue)
        {

            client.Cypher.Match("(n:" + Label + ')')
                                .Where("n." + FindbyPopertie + "=" + '"' + FindbyValue + '"')
                                .Set("n." + newPopertie + "=" + '"' + newValue + '"')
                                .ExecuteWithoutResults();

        }


        //    ###################################              {RELATIONSHIP SECTION}                         #################################

        //Find 2 nodes and create a Directed relationshipt between them N1 -> N2, Takes String values to find
        public void CreateRelationShipDirectedN1TON2(string Label1, string FindbyPopertieNode1, string FindbyValueNode1, string Label2, string FindbyPopertieNode2, string FindbyValueNode2, string RelationShip)
        {


            client.Cypher.Match("(node1:" + Label1 + ')' + ',' + "(node2:" + Label2 + ')')
                                .Where("node1." + FindbyPopertieNode1 + "=" + '"' + FindbyValueNode1 + '"')
                                .AndWhere("node2." + FindbyPopertieNode2 + "=" + '"' + FindbyValueNode2 + '"')
                                .CreateUnique("node1" + '-' + '[' + ':' + RelationShip + ']' + '-' + '>' + "node2")
                                .ExecuteWithoutResults();

        }


        //Find 2 nodes and create Directed relationshipt between them N1 -> N2, Takes Integer values to find
        public void CreateRelationShipDriectedN1TON2(string Label1, string FindbyPopertieNode1, int FindbyValueNode1, string Label2, string FindbyPopertieNode2, int FindbyValueNode2, string RelationShip)
        {


            client.Cypher.Match("(node1:" + Label1 + ')' + ',' + "(node2:" + Label2 + ')')
                                .Where("node1." + FindbyPopertieNode1 + "=" + FindbyValueNode1)
                                .AndWhere("node2." + FindbyPopertieNode2 + "=" + FindbyValueNode2)
                                .CreateUnique("node1" + '-' + '[' + ':' + RelationShip + ']' + '-' + '>' + "node2")
                                .ExecuteWithoutResults();

        }



        //Find 2 nodes and create UnDirected relationshipt between them N1 -- N2, Takes String values to find
        public void CreateRelationShipUnDirectedN1TON2(string Label1, string FindbyPopertieNode1, string FindbyValueNode1, string Label2, string FindbyPopertieNode2, string FindbyValueNode2, string RelationShip)
        {


            client.Cypher.Match("(node1:" + Label1 + ')' + ',' + "(node2:" + Label2 + ')')
                                .Where("node1." + FindbyPopertieNode1 + "=" + '"' + FindbyValueNode1 + '"')
                                .AndWhere("node2." + FindbyPopertieNode2 + "=" + '"' + FindbyValueNode2 + '"')
                                .CreateUnique("node1" + '-' + '[' + ':' + RelationShip + ']' + '-' + '>' + "node2" + ',' +
                                        "node2" + '-' + '[' + ':' + RelationShip + ']' + '-' + '>' + "node1")
                                .ExecuteWithoutResults();

        }


        //Find 2 nodes and create UnDirected relationshipt between them N1 -- N2, Takes Integer values to find
        public void CreateRelationShipUnDriectedN1TON2(string Label1, string FindbyPopertieNode1, int FindbyValueNode1, string Label2, string FindbyPopertieNode2, int FindbyValueNode2, string RelationShip)
        {


            client.Cypher.Match("(node1:" + Label1 + ')' + ',' + "(node2:" + Label2 + ')')
                                .Where("node1." + FindbyPopertieNode1 + "=" + FindbyValueNode1)
                                .AndWhere("node2." + FindbyPopertieNode2 + "=" + FindbyValueNode2)
                                .CreateUnique("node1" + '-' + '[' + ':' + RelationShip + ']' + '-' + '>' + "node2" + ',' +
                                         "node2" + '-' + '[' + ':' + RelationShip + ']' + '-' + '>' + "node1")
                                .ExecuteWithoutResults();

        }



        //Deletes directed relation between 2 nodes, (node1:Label) -[R:Relation]->(node2:Label)
        public void DeleteAnyDirectedRelationShip(string Label1, string FindbyPopertieNode1, string FindbyValueNode1, string Label2, string FindbyPopertieNode2, string FindbyValueNode2, string RelationShip)
        {
            client.Cypher.Match("(node1:" + Label1 + ')' + '-' + '[' + 'R' + ':' + RelationShip + ']' + '-' + '>' + "(node2:" + Label2 + ')')
                                .Where("node1." + FindbyPopertieNode1 + "=" + '"' + FindbyValueNode1 + '"')
                                .AndWhere("node2." + FindbyPopertieNode2 + "=" + '"' + FindbyValueNode2 + '"')
                                .Delete("R")
                                .ExecuteWithoutResults();

        }


        //Deletes any relation between 2 nodes, (node1:Label) -[R:Relation]-(node2:Label)
        public void DeleteAnyRelationShip(string Label1, string FindbyPopertieNode1, string FindbyValueNode1, string Label2, string FindbyPopertieNode2, string FindbyValueNode2, string RelationShip)
        {
            client.Cypher.Match("(node1:" + Label1 + ')' + '-' + '[' + 'R' + ':' + RelationShip + ']' + '-' + "(node2:" + Label2 + ')')
                                .Where("node1." + FindbyPopertieNode1 + "=" + '"' + FindbyValueNode1 + '"')
                                .AndWhere("node2." + FindbyPopertieNode2 + "=" + '"' + FindbyValueNode2 + '"')
                                .Delete("R")
                                .ExecuteWithoutResults();

        }






        ////Returns 2 nodes and relationsships, {use foreach loop to go through returned result, var.n1.prop , var.R.TypeKey, var.n2.prop
        //Results contains, var.R.StartNodeReference.Id, var.R.TypeKey, var.R.EndNodeReference.Id
        public dynamic RetriveRealtionShipN1TON2<T>(string Label1, string FindbyPopertieNode1, string FindbyValueNode1, string Label2, string FindbyPopertieNode2, string FindbyValueNode2, string RelationShip)
        {

            //{Future improv. Choose Direction} 


            var results = client.Cypher.Match("(node1:" + Label1 + ')' + '-' + '[' + "f" + ':' + RelationShip + ']' + '-' + "(node2:" + Label2 + ')')
                                 .Where("node1." + FindbyPopertieNode1 + "=" + '"' + FindbyValueNode1 + '"')
                                 .AndWhere("node2." + FindbyPopertieNode2 + "=" + '"' + FindbyValueNode2 + '"')
                                 .Return((node1, node2, f) => new
                                  {
                                      n1 = node1.As<T>(),
                                      n2 = node2.As<T>(),
                                      R = f.As<RelationshipInstance<object>>()
                                  }).Results;

            if (results.Count() != 0)
            {



                return results;
            }
            else
            {

                return null;  //Returns null if no relationship is found.
            }
        }


        //Returns a list of nodes and relationsships, {use foreach loop to go through returned result, var.n1.prop , var.R.TypeKey, var.n2.prop
        //Results contains, var.R.StartNodeReference.Id, var.R.TypeKey, var.R.EndNodeReference.Id
        public dynamic RetriveRealtionShipAllNodes<T>(string Label1, string Label2, string RelationShip, bool Directed)
        {


            string DirectionSymbol = "";
            if (Directed)
            {
                DirectionSymbol = ">";

            }

            var results = client.Cypher.Match("(node1:" + Label1 + ')' + '-' + '[' + "f" + ':' + RelationShip + ']' + '-' + DirectionSymbol + "(node2:" + Label2 + ')')
                                  .Return((node1, node2, f) => new
                                   {
                                       n1 = node1.As<T>(),
                                       n2 = node2.As<T>(),
                                       R = f.As<RelationshipInstance<dynamic>>(),
                                   }).Results.ToList();

            return results;

        }




    }

}
