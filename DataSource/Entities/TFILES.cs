using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TFILES
    {
        public int ID { get; set; }
        public int IDPROPERTY { get; set; }
        public string? PATH { get; set; }
        public string? URI { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? TITLE { get; set; }
        public int? IDSTATUS { get; set; }
        public int? STATUSBLOB { get; set; }
        public string? FILEEXTENCION { get; set; }
        public int? PREVIEW { get; set; }
        public DateTime? CREATE_AT { get; set; }
        public DateTime? UPDATE_AT { get; set; }

        public virtual TPROPERTIES IDPROPERTYNavigation { get; set; } = null!;
        public virtual TCSTATUS? IDSTATUSNavigation { get; set; }
    }
}
