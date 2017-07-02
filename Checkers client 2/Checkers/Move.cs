using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Checkers.ServiceReference1;

namespace Checkers
{
    [DataContract]
    public class Move
    {
        [DataMember]
        public int MoveId { get; set; }

        [DataMember]
        public Game GameOfMove { get; set; }

        [DataMember]
        public Player PlayerOfMove { get; set; }

        [DataMember]
        public string MoveString { get; set; }//Format = "xPos,yPos@xPos,yPos"

        //TODO: fix method, may need to be a static method
        public void IncertMoveToDB() { }
    }
}
