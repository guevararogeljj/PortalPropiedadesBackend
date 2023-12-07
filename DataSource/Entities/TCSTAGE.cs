using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCSTAGE
    {
        public TCSTAGE()
        {
            TPROPERTIES = new HashSet<TPROPERTIES>();
        }

        public int ID { get; set; }
        public string? DESCRPTION { get; set; }
        public int? IDSTATUS { get; set; }

        public virtual TCSTATUS? IDSTATUSNavigation { get; set; }
        public virtual ICollection<TPROPERTIES> TPROPERTIES { get; set; }
    }
}
