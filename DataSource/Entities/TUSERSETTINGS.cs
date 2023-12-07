using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TUSERSETTINGS
    {
        public int ID { get; set; }
        public int? IDUSER { get; set; }
        public short? ADVERTISINGEMAIL { get; set; }
        public short? ADVERTISINGSMS { get; set; }
        public short? EMAILNOTIFICATION { get; set; }
        public short? SMSNOTIFICATION { get; set; }

        public virtual TUSERS? IDUSERNavigation { get; set; }
    }
}
