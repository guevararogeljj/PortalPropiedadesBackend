using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class CH_PWP_Archivo
    {
        public int PWP_ID_ARCHIVOS { get; set; }
        public string PWP_PROPIEDAD_ID { get; set; } = null!;
        public string? PWP_A_PATH { get; set; }
        public string? PWP_A_URI { get; set; }
        public string? PWP_A_DESCRIPTION { get; set; }
        public string? PWP_A_TITLE { get; set; }
        public short? PWP_A_STATUS { get; set; }
        public string? PWP_A_TYPE { get; set; }
        public DateTime PWP_A_UPDATED_AT { get; set; }
        public DateTime PWP_A_CREATED_AT { get; set; }
        public short PWP_A_STATUSBLOB { get; set; }

        public virtual CH_PaginaWebPropiedade PWP_PROPIEDAD { get; set; } = null!;
    }
}
