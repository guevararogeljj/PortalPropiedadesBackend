using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCCITIES
    {
        public TCCITIES()
        {
            TADDRESSES = new HashSet<TADDRESSES>();
        }

        public int ID { get; set; }
        public string DESCRIPTION { get; set; } = null!;
        public string CODE { get; set; } = null!;
        public string CODESTATE { get; set; } = null!;

        public virtual TCSTATESv2 CODESTATENavigation { get; set; } = null!;
        public virtual ICollection<TADDRESSES> TADDRESSES { get; set; }
    }
}
