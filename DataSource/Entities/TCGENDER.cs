using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCGENDER
    {
        public TCGENDER()
        {
            TUSERSINFO = new HashSet<TUSERSINFO>();
        }

        public int ID { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? IDSTATUS { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public virtual TCSTATUS? IDSTATUSNavigation { get; set; }
        public virtual ICollection<TUSERSINFO> TUSERSINFO { get; set; }
    }
}
