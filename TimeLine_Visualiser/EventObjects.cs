using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLine_Visualiser
{
    class EventObjects
    {
        //Object Class to hold events data after parsing for transform to database

       public DateTime Time { set; get; }

       public string Source { set; get; }

       public string SystemName { set; get; }

       public string UserName { set; get; }

       public string Description { set; get; }

       public EventObjects()
        {

        }
    }
}
