using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TUSERSINFO
    {
        public int ID { get; set; }
        public int? IDUSER { get; set; }
        public int? IDGENDER { get; set; }
        public string? CURP { get; set; }
        public string? NAMES { get; set; }
        public string? LASTNAME { get; set; }
        public string? LASTNAME2 { get; set; }
        public DateTime? BIRTHDAY { get; set; }
        public string? BIRTHPLACE { get; set; }
        public bool? TERMS { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public int? IDOCCUPATION { get; set; }
        public bool? RFCDOC { get; set; }
        public string? RFC { get; set; }
        public int? IDMARITALSTATUS { get; set; }
        public string? HOMETOWN { get; set; }
        public string? ADDRESS { get; set; }
        public bool? RFCTYPE { get; set; }
        public string? PLACEDOCUMENT { get; set; }
        public string? NUMBERDOCUMENT { get; set; }

        public virtual TCGENDER? IDGENDERNavigation { get; set; }
        public virtual TCMARITALSTATUS? IDMARITALSTATUSNavigation { get; set; }
        public virtual TCOCCUPATIONS? IDOCCUPATIONNavigation { get; set; }
        public virtual TUSERS? IDUSERNavigation { get; set; }

    }
}
