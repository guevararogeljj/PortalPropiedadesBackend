using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCTITLES
    {
        public int ID { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? IDPROCEDURALSTAGE { get; set; }
        public DateTime? CREATEDAT { get; set; }
        public DateTime? UPDATEDAT { get; set; }

        public virtual TCPROCEDURALSTAGE? IDPROCEDURALSTAGENavigation { get; set; }
    }
}
