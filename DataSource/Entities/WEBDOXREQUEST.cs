using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class WEBDOXREQUEST
    {
        public int ID { get; set; }
        public int? ORDER { get; set; }
        public int? IDUSER { get; set; }
        public string? FORMWEBDOXID { get; set; }
        public string? JSON { get; set; }
        public string? DESCRIPTION { get; set; }
        public bool? STATUS { get; set; }
        public DateTime? CREATE_AT { get; set; }

        public virtual TUSERS? IDUSERNavigation { get; set; }
    }
}
