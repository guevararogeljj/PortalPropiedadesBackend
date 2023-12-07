using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TPROPERTIES
    {
        public TPROPERTIES()
        {
            TFILES = new HashSet<TFILES>();
            TPROPERTYREQUEST = new HashSet<TPROPERTYREQUEST>();
            TRUSERPROPERTIES = new HashSet<TRUSERPROPERTIES>();
        }

        public int ID { get; set; }
        public int? IDTYPE { get; set; }
        public int? IDPROCEDURALSTAGE { get; set; }
        public int? IDSTATUS { get; set; }
        public int? IDSTAGE { get; set; }
        public int? IDCONDITION { get; set; }
        public string? CODE { get; set; }
        public string? CREDITNUMBER { get; set; }
        public string? TITLE { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? CURRENCY { get; set; }
        public decimal? SALEPRICE { get; set; }
        public double? CONSTRUCTIONSIZE { get; set; }
        public double? LOTSIZE { get; set; }
        public decimal? TOTALDEBT { get; set; }
        public decimal? GUARANTEEVALUE { get; set; }
        public decimal? ACQUISITIONDEADLINE { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATE_AT { get; set; }
        public decimal? MAINTENANCEFEE { get; set; }
        public int? IDBEDROOM { get; set; }
        public int? IDBATHROOM { get; set; }
        public int? IDHALFBATHROOM { get; set; }
        public int? IDPARKINGSPACE { get; set; }
        public int? IDLEVEL { get; set; }

        public virtual TCSPACES? IDBATHROOMNavigation { get; set; }
        public virtual TCSPACES? IDBEDROOMNavigation { get; set; }
        public virtual TCCONDITION? IDCONDITIONNavigation { get; set; }
        public virtual TCSPACES? IDHALFBATHROOMNavigation { get; set; }
        public virtual TCSPACES? IDLEVELNavigation { get; set; }
        public virtual TCSPACES? IDPARKINGSPACENavigation { get; set; }
        public virtual TCPROCEDURALSTAGE? IDPROCEDURALSTAGENavigation { get; set; }
        public virtual TCSTAGE? IDSTAGENavigation { get; set; }
        public virtual TCSTATUS? IDSTATUSNavigation { get; set; }
        public virtual TCTYPE? IDTYPENavigation { get; set; }
        public virtual TADDRESSES? TADDRESSES { get; set; }
        public virtual ICollection<TFILES> TFILES { get; set; }
        public virtual ICollection<TPROPERTYREQUEST> TPROPERTYREQUEST { get; set; }
        public virtual ICollection<TRUSERPROPERTIES> TRUSERPROPERTIES { get; set; }
    }
}
