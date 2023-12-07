using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TRUSERPROPERTIES
    {
        public int IDUSER { get; set; }
        public int IDPROPERTY { get; set; }
        public int? IDSTATUS { get; set; }
        public DateTime? CREATE_AT { get; set; }
        public DateTime? UPDATED_AD { get; set; }

        public virtual TPROPERTIES IDPROPERTYNavigation { get; set; } = null!;
        public virtual TCSTATUS? IDSTATUSNavigation { get; set; }
        public virtual TUSERS IDUSERNavigation { get; set; } = null!;
    }
}
