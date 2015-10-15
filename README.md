# TimeLine Visualizer
A tool to structure five-field timeline data for store in a graph database, which allows faster analysis with the help of already established graph-algorithms.

This is the whole project as Microsoft Visual Studio project. 

NOTE!! that the tool requiers the graph database Neo4j 

The five-field timeline format is described in Windows Forensic Analysis Toolkit, Third Edition(chapter 7).  
The timeline data can be generated with the help of free tools avilable on(chapter 7): 

https://code.google.com/p/winforensicaanalysis/downloads/detail?name=wfa3e.zip&can=2&q= 

The tools include a file with detalied descriptions of every field. 

#Five-field format
The five-field structure is as follow,

Time|Source|System|User|Description 

An example of real raw timeline data,

1087576057|EVT|REG-OIPK81M2WC8|N/A|LoadPerf/1000;Info;RSVP,QoS RSVP
1087576059|EVT|REG-OIPK81M2WC8|N/A|LoadPerf/1000;Info;PSched,PSched
1087576069|EVT|REG-OIPK81M2WC8|N/A|LoadPerf/1000;Info;RemoteAccess,Routing and Remote Access
1087577850|EVT|REG-OIPK81M2WC8|N/A|LoadPerf/1000;Info;TermService,Terminal Services
1087577859|EVT|REG-OIPK81M2WC8|N/A|LoadPerf/1000;Info;ContentIndex,ContentIndex
1087577859|EVT|REG-OIPK81M2WC8|N/A|LoadPerf/1000;Info;ContentFilter,ContentFilter
1087577859|EVT|REG-OIPK81M2WC8|N/A|LoadPerf/1000;Info;ISAPISearch,ISAPISearch
1087577866|EVT|REG-OIPK81M2WC8|N/A|LoadPerf/1000;Info;WmiApRpl,WmiApRpl
1087577867|EVT|REG-OIPK81M2WC8|N/A|LoadPerf/1001;Info;WmiApRpl,WmiApRpl
1087577867|EVT|REG-OIPK81M2WC8|N/A|LoadPerf/1000;Info;WmiApRpl,WmiApRpl

-------------------------------------------------------------------------

#Structure in database
The timeline data is structured as follows, 
Every event is saved as a node with the five-field data as properties. 
An unique node is created of every time field. 
Every event that have a time propertie, that matches the unique time nodes "time", is then bounded to that node by a relationship with the LastWrite as type.
Se the figure below --> 

![alt tag](https://github.com/Demolitionman117/TLV/blob/master/DbStructure.png)

-------------------------------------------------------------------------------

#Analysis Algorithms 
The analysis is very easy to do with the following algorithms (cypher) typed directly in neo4j webinterface: 

structural similarity, the algorithms below shows events that share the same change time.

//shows only similar times between EVT(Event Logg) and PREF(prefetch)
MATCH (event1:EVT)-[:LastWrite]->(evtime:EventTime)<-[:LastWrite]-(event2:PREF)
WITH event1.Description AS EVT, event2.Description AS PREF, evtime.Time AS Links
RETURN  EVT,PREF,Links AS Time ORDER BY Time DESC

//shows only similar times between EVT(Event Logg) and REG(Registry)
MATCH (event1:EVT)-[:LastWrite]->(evtime:EventTime)<-[:LastWrite]-(event2:REG)
WITH event1.Description AS EVT, event2.Description AS REG, evtime.Time AS Links
RETURN  EVT,REG,Links AS Time ORDER BY Time DESC

//shows only similar times between PREF(prefetch) and REG(Registry)
MATCH (event1:PREF)-[:LastWrite]->(evtime:EventTime)<-[:LastWrite]-(event2:REG)
WITH event1.Description AS PREF, event2.Description AS REG, evtime.Time AS Links
RETURN  PREF,REG,Links AS Time ORDER BY Time DESC

The Algorithms can be modified to compare more than 2 types.

-------------------------------------------------------------------------------

#Analysis Visual 
The analysis could be visual, this can be achieved by typing the following command directly in neo4j webinterface, 

MATCH (n:EventTime) RETURN n LIMIT 25 //The LIMIT could be changed after need. 

After entering the above command, the database is going to answer with visual time nodes, by double clicking on any node of interest. The events that are bounded to the node, are gonna popup as nodes with relations. Se the figure below --> 

![alt tag](https://github.com/Demolitionman117/TLV/blob/master/klickedevents.png)

------------------------------------------------------------------------------------

The latest compiled release version of the tool can be found under, https://github.com/Demolitionman117/TLV/tree/master/TimeLine_Visualiser/bin/Release 

--------------------------------------------------------------------------------------

Feel free to use and change the tool. Read https://github.com/Demolitionman117/TLV/blob/master/LICENSE 
