using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCMARITALSTATUS
    {
        public TCMARITALSTATUS()
        {
            TUSERSINFO = new HashSet<TUSERSINFO>();
        }

        public int ID { get; set; }
        public string DESCRIPTION { get; set; } = null!;
        public int? IDSTATUS { get; set; }
        public DateTime? CREATED_AT { get; set; }

        public virtual ICollection<TUSERSINFO> TUSERSINFO { get; set; }
    }
}
