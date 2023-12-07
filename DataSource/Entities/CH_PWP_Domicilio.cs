using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace DataSource.Entities
{
    public partial class CH_PWP_Domicilio
    {
        public CH_PWP_Domicilio()
        {
            CH_PaginaWebPropiedades = new HashSet<CH_PaginaWebPropiedade>();
        }

        public int PWP_ID_DOMICILIOS { get; set; }
        public string? PWP_D_COUNTRY { get; set; }
        public int? PWP_D_STATE { get; set; }
        public int? PWP_D_CITY { get; set; }
        public string? PWP_D_POSTALCODE { get; set; }
        public string? PWP_D_STREETNAME { get; set; }
        public string? PWP_D_EXTERIORNUMBER { get; set; }
        public string? PWP_D_INTERIORNUMBER { get; set; }
        public Geometry? PWP_D_COORDENADAS { get; set; }
        public DateTime PWP_D_UPDATED_AT { get; set; }
        public DateTime PWP_D_CREATED_AT { get; set; }
        public string? PWP_D_SETTLEMENT { get; set; }

        public virtual CH_CAT_Municipio? PWP_D_CITYNavigation { get; set; }
        public virtual CH_CAT_TipoPlaza? PWP_D_STATENavigation { get; set; }
        public virtual ICollection<CH_PaginaWebPropiedade> CH_PaginaWebPropiedades { get; set; }
    }
}
