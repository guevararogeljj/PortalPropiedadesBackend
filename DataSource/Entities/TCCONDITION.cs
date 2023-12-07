using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCCONDITION
    {
        public TCCONDITION()
        {
            InverseIDSTATUSNavigation = new HashSet<TCCONDITION>();
            TPROPERTIES = new HashSet<TPROPERTIES>();
        }

        public int ID { get; set; }
        public string DESCRIPTION { get; set; } = null!;
        public int? IDSTATUS { get; set; }
        public DateTime? CREATED_AT { get; set; }

        public virtual TCCONDITION? IDSTATUSNavigation { get; set; }
        public virtual ICollection<TCCONDITION> InverseIDSTATUSNavigation { get; set; }
        public virtual ICollection<TPROPERTIES> TPROPERTIES { get; set; }
    }
}
