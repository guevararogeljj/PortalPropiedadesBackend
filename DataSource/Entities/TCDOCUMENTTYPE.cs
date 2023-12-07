using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCDOCUMENTTYPE
    {
        public TCDOCUMENTTYPE()
        {
            TDOCUMENTS = new HashSet<TDOCUMENTS>();
        }

        public int ID { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? IDSTATUS { get; set; }
        public DateTime? CREATE_AT { get; set; }

        public virtual TCSTATUS? IDSTATUSNavigation { get; set; }
        public virtual ICollection<TDOCUMENTS> TDOCUMENTS { get; set; }
    }
}
