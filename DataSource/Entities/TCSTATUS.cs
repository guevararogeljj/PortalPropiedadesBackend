using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCSTATUS
    {
        public TCSTATUS()
        {
            SYSTEMUSERS = new HashSet<SYSTEMUSERS>();
            TCDOCUMENTTYPE = new HashSet<TCDOCUMENTTYPE>();
            TCGENDER = new HashSet<TCGENDER>();
            TCPROCEDURALSTAGE = new HashSet<TCPROCEDURALSTAGE>();
            TCSPACES = new HashSet<TCSPACES>();
            TCSTAGE = new HashSet<TCSTAGE>();
            TCTYPE = new HashSet<TCTYPE>();
            TFILES = new HashSet<TFILES>();
            TPROPERTIES = new HashSet<TPROPERTIES>();
            TRUSERPROPERTIES = new HashSet<TRUSERPROPERTIES>();
        }

        public int ID { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime? CREATED_AT { get; set; }

        public virtual ICollection<SYSTEMUSERS> SYSTEMUSERS { get; set; }
        public virtual ICollection<TCDOCUMENTTYPE> TCDOCUMENTTYPE { get; set; }
        public virtual ICollection<TCGENDER> TCGENDER { get; set; }
        public virtual ICollection<TCPROCEDURALSTAGE> TCPROCEDURALSTAGE { get; set; }
        public virtual ICollection<TCSPACES> TCSPACES { get; set; }
        public virtual ICollection<TCSTAGE> TCSTAGE { get; set; }
        public virtual ICollection<TCTYPE> TCTYPE { get; set; }
        public virtual ICollection<TFILES> TFILES { get; set; }
        public virtual ICollection<TPROPERTIES> TPROPERTIES { get; set; }
        public virtual ICollection<TRUSERPROPERTIES> TRUSERPROPERTIES { get; set; }
    }
}
