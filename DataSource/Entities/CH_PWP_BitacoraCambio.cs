using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class CH_PWP_BitacoraCambio
    {
        public int PWP_ID_BITACORACAMBIOS { get; set; }
        public string? PWP_BC_USER { get; set; }
        public string? PWP_BC_DESCRIPTION { get; set; }
        public string? PWP_BC_CHANGE { get; set; }
        public string? PWP_BC_IP { get; set; }
        public DateTime PWP_BC_CREATED_AT { get; set; }
    }
}
