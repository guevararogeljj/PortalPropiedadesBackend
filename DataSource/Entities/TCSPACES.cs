using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCSPACES
    {
        public TCSPACES()
        {
            TPROPERTIESIDBATHROOMNavigation = new HashSet<TPROPERTIES>();
            TPROPERTIESIDBEDROOMNavigation = new HashSet<TPROPERTIES>();
            TPROPERTIESIDHALFBATHROOMNavigation = new HashSet<TPROPERTIES>();
            TPROPERTIESIDLEVELNavigation = new HashSet<TPROPERTIES>();
            TPROPERTIESIDPARKINGSPACENavigation = new HashSet<TPROPERTIES>();
        }

        public int ID { get; set; }
        public string DESCRIPTION { get; set; } = null!;
        public int? IDSTATUS { get; set; }
        public DateTime? CREATEDAT { get; set; }

        public virtual TCSTATUS? IDSTATUSNavigation { get; set; }
        public virtual ICollection<TPROPERTIES> TPROPERTIESIDBATHROOMNavigation { get; set; }
        public virtual ICollection<TPROPERTIES> TPROPERTIESIDBEDROOMNavigation { get; set; }
        public virtual ICollection<TPROPERTIES> TPROPERTIESIDHALFBATHROOMNavigation { get; set; }
        public virtual ICollection<TPROPERTIES> TPROPERTIESIDLEVELNavigation { get; set; }
        public virtual ICollection<TPROPERTIES> TPROPERTIESIDPARKINGSPACENavigation { get; set; }
    }
}
