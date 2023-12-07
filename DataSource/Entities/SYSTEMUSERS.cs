using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class SYSTEMUSERS
    {
        public int ID { get; set; }
        public string? EMAIL { get; set; }
        public string? FULLNAME { get; set; }
        public string? ROLE { get; set; }
        public int? IDSTATUS { get; set; }

        public virtual TCSTATUS? IDSTATUSNavigation { get; set; }
    }
}
