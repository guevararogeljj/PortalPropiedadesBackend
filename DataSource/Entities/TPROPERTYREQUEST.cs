using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TPROPERTYREQUEST
    {
        public int ID { get; set; }
        public int? IDPROPERTY { get; set; }
        public int? IDUSER { get; set; }
        public string? SHAREDTO { get; set; }
        public DateTime? CREATEDAT { get; set; }
        public DateTime? UPDATEDAT { get; set; }

        public virtual TPROPERTIES? IDPROPERTYNavigation { get; set; }
        public virtual TUSERS? IDUSERNavigation { get; set; }
    }
}
