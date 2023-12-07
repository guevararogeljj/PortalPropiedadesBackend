using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCSTATESv2
    {
        public TCSTATESv2()
        {
            TCCITIES = new HashSet<TCCITIES>();
        }

        public int ID { get; set; }
        public string DESCRIPTION { get; set; } = null!;
        public string CODE { get; set; } = null!;

        public virtual ICollection<TCCITIES> TCCITIES { get; set; }
    }
}
