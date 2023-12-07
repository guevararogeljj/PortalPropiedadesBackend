using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TEMAILVALIDATION
    {
        public int ID { get; set; }
        public int? IDUSER { get; set; }
        public string EMAIL { get; set; } = null!;
        public string TOKEN { get; set; } = null!;
        public DateTime? USED_AT { get; set; }
        public DateTime? CREATED_AT { get; set; }

        public virtual TUSERS? IDUSERNavigation { get; set; }
    }
}
