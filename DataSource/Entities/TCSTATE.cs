using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCSTATE
    {
        public TCSTATE()
        {
            TCCITY = new HashSet<TCCITY>();
        }

        public int ID { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? IDSTATUS { get; set; }
        public DateTime? CREATED_AT { get; set; }

        public virtual TCSTATUS? IDSTATUSNavigation { get; set; }
        public virtual ICollection<TCCITY> TCCITY { get; set; }
    }
}
