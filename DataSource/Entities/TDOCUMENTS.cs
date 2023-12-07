using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TDOCUMENTS
    {
        public int ID { get; set; }
        public int IDUSER { get; set; }
        public int? IDDOCUMENTTYPE { get; set; }
        public string? PATH { get; set; }
        public string? URI { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? TITLE { get; set; }
        public int? IDSTATUS { get; set; }
        public int? STATUSBLOB { get; set; }
        public string? FILEEXTENCION { get; set; }
        public DateTime? CREATE_AT { get; set; }
        public DateTime? UPDATE_AT { get; set; }

        public virtual TCDOCUMENTTYPE? IDDOCUMENTTYPENavigation { get; set; }
        public virtual TUSERS IDUSERNavigation { get; set; } = null!;
    }
}
