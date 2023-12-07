using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCREGISTERSTATUS
    {
        public TCREGISTERSTATUS()
        {
            TRUSERSTATUSREGISTER = new HashSet<TRUSERSTATUSREGISTER>();
        }

        public int ID { get; set; }
        public string DESCRIPTION { get; set; } = null!;
        public int? IDSTATUS { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public virtual ICollection<TRUSERSTATUSREGISTER> TRUSERSTATUSREGISTER { get; set; }
    }
}
