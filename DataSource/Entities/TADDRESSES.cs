using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TADDRESSES
    {
        public int ID { get; set; }
        public int? IDPROPERTY { get; set; }
        public string? SETTLEMENT { get; set; }
        public string? POSTCODE { get; set; }
        public string? STREETNAME { get; set; }
        public string? EXTERIORNUMBER { get; set; }
        public string? INTERIORNUMBER { get; set; }
        public double? LATITUDE { get; set; }
        public double? LONGITUDE { get; set; }
        public DateTime? UPDATE_AT { get; set; }
        public DateTime? CREATE_AT { get; set; }
        public int? IDCITYv2 { get; set; }

        public virtual TCCITIES? IDCITYv2Navigation { get; set; }
        public virtual TPROPERTIES? IDPROPERTYNavigation { get; set; }
    }
}
