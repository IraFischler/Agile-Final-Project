using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfService4
{
    [DataContract]
    public class Player
    {
        [DataMember]
        public int id { get; set; }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int TeamID { get; set; }


        [DataMember]
        public string Color { get; set; }

        public override bool Equals(System.Object obj) {
            if (obj == null)
                return false;
            else if (obj.GetType() != this.GetType())
                return false;
            else
                return this.id == ((Player)obj).id; 
                
        }

        public override int GetHashCode() {
            return this.id;
        }

    }
}