using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfService4
{
    [DataContract]
    public class Game
    {
        [DataMember]
        private int id;
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        [DataMember]
        public string StartTime { get; set; }

        [DataMember]
        public string WinnerName { get; set; }

        [DataMember]
        public int WinnerId { get; set; }

        [DataMember]
        public Team Team1 { get; set; }

        [DataMember]
        public Team Team2 { get; set; }

        [DataMember]
        public Player Player1 { get; set; }

        [DataMember]
        public Player Player2 { get; set; }



        public bool IsTeamGame() {
            return !((Team1 == null)&&(Team2 == null));
        }

    }
}