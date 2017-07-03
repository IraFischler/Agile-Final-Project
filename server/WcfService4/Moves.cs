using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfService4
{
    [DataContract]
    public class Moves
    {
        [DataMember]
        public int MoveId { get; set; }

        [DataMember]
        public Game GameOfMove { get; set; }

        [DataMember]
        public Player PlayerOfMove { get; set; }

        [DataMember]
        public string MoveString { get; set; }
    }
}