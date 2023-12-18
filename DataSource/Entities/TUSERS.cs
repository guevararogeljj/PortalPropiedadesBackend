using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TUSERS
    {
        public TUSERS()
        {
            TDOCUMENTS = new HashSet<TDOCUMENTS>();
            TEMAILVALIDATION = new HashSet<TEMAILVALIDATION>();
            TPASSWORDRECOVERY = new HashSet<TPASSWORDRECOVERY>();
            TPROPERTYREQUEST = new HashSet<TPROPERTYREQUEST>();
            TRUSERPROPERTIES = new HashSet<TRUSERPROPERTIES>();
            TRUSERSTATUSREGISTER = new HashSet<TRUSERSTATUSREGISTER>();
            TUSERSETTINGS = new HashSet<TUSERSETTINGS>();
            WEBDOXREQUEST = new HashSet<WEBDOXREQUEST>();
        }

        public int ID { get; set; }
        public string? EMAIL { get; set; }
        public string? PASSWORD { get; set; }
        public string? CELLPHONE { get; set; }
        public short? LOGIN { get; set; }
        public DateTime? CELLPHONE_VALIDATED_AT { get; set; }
        public DateTime? EMAIL_VALIDATED_AT { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public bool? TERMS { get; set; }
        public bool? REQUIREDCODE { get; set; }
        public string? TOKEN { get; set; }
        public DateTime? IDENTITY_VALIDATED_AT { get; set; }
        public DateTime? CONTRACT_SIGN_AT { get; set; }
        public DateTime? PROCESSCONTRACT { get; set; }
        public int? ATTEMPTS { get; set; }
        public string? EMAILSECONDARY { get; set; }
        public string? CELLPHONESECONDARY { get; set; }

        public virtual TUSERSINFO? TUSERSINFO { get; set; }
        public virtual ICollection<TDOCUMENTS> TDOCUMENTS { get; set; }
        public virtual ICollection<TEMAILVALIDATION> TEMAILVALIDATION { get; set; }
        public virtual ICollection<TPASSWORDRECOVERY> TPASSWORDRECOVERY { get; set; }
        public virtual ICollection<TPROPERTYREQUEST> TPROPERTYREQUEST { get; set; }
        public virtual ICollection<TRUSERPROPERTIES> TRUSERPROPERTIES { get; set; }
        public virtual ICollection<TRUSERSTATUSREGISTER> TRUSERSTATUSREGISTER { get; set; }
        public virtual ICollection<TUSERSETTINGS> TUSERSETTINGS { get; set; }
        public virtual ICollection<WEBDOXREQUEST> WEBDOXREQUEST { get; set; }
        
    }
}
