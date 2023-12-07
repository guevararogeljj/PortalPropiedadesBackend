using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class CH_CAT_Municipio
    {
        public CH_CAT_Municipio()
        {
            CH_PWP_Domicilios = new HashSet<CH_PWP_Domicilio>();
        }

        public int M_IdMunicipio { get; set; }
        public string? M_Municipio { get; set; }
        public int M_IdTipoPlazaFK { get; set; }
        public int? M_Jurisdiccion { get; set; }

        public virtual CH_CAT_TipoPlaza M_IdTipoPlazaFKNavigation { get; set; } = null!;
        public virtual ICollection<CH_PWP_Domicilio> CH_PWP_Domicilios { get; set; }
    }
}
