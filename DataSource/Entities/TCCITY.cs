using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCCITY
    {
        public TCCITY()
        {
            TADDRESSES = new HashSet<TADDRESSES>();
        }

        public int ID { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? IDSTATE { get; set; }
        public int? IDSTATUS { get; set; }
        public DateTime? CREATE_AT { get; set; }

        public virtual TCSTATE? IDSTATENavigation { get; set; }
        public virtual TCSTATUS? IDSTATUSNavigation { get; set; }
        public virtual ICollection<TADDRESSES> TADDRESSES { get; set; }
    }
}
