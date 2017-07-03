using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfService4
{
    [DataContract]
    public class Team
    {
        [DataMember]
        private int id;
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [DataMember]
        public string TeamName { get; set; }

        [DataMember]
        public List<Player> PlayersInTeam { get; set; }

        public void AddPlayerToTeam(Player p) {
            if(p != null)
                this.PlayersInTeam.Add(p);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
                return false;
            else if (obj.GetType() != this.GetType())
                return false;
            else
                return this.id == ((Team)obj).id;

        }

        public override int GetHashCode()
        {
            return this.id;
        }





    }
}