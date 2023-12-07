using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TRUSERSTATUSREGISTER
    {
        public int IDUSER { get; set; }
        public int IDREGISTERSTATUS { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public virtual TCREGISTERSTATUS IDREGISTERSTATUSNavigation { get; set; } = null!;
        public virtual TUSERS IDUSERNavigation { get; set; } = null!;
    }
}
