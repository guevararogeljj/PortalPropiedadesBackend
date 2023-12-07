using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class CH_PaginaWebPropiedade
    {
        public CH_PaginaWebPropiedade()
        {
            CH_PWP_Archivos = new HashSet<CH_PWP_Archivo>();
        }

        public string PWP_ID { get; set; } = null!;
        public string PWP_NOCREDITO { get; set; } = null!;
        public string? PWP_TITLE { get; set; }
        public string? PWP_DESCRIPTION { get; set; }
        public string? PWP_TYPE { get; set; }
        public string? PWP_CURRENCY { get; set; }
        public int? PWP_STATUS { get; set; }
        public decimal? PWP_SALESPRICE { get; set; }
        public int? PWP_BEDROOMS { get; set; }
        public int? PWP_FULLBATHROOMS { get; set; }
        public int? PWP_HALFBATHROOMS { get; set; }
        public int? PWP_PARKINGSPACES { get; set; }
        public decimal? PWP_CONSTRUCTIONSIZE { get; set; }
        public decimal? PWP_LOTSIZE { get; set; }
        public int? PWP_LEVELS { get; set; }
        public int? PWP_ETAPAJUDICIAL { get; set; }
        public decimal? PWP_ADEUDOTOTAL { get; set; }
        public decimal? PWP_VALORDEGARANTIA { get; set; }
        public int? PWP_PLAZODEADQUISICION { get; set; }
        public int? PWP_DOMICILIO { get; set; }
        public DateTime PWP_UPDATED_AT { get; set; }
        public DateTime PWP_CREATED_AT { get; set; }
        public string? PWP_CARTERA { get; set; }

        public virtual CH_PWP_Domicilio? PWP_DOMICILIONavigation { get; set; }
        public virtual CH_CAT_EtapasProcesale? PWP_ETAPAJUDICIALNavigation { get; set; }
        public virtual ICollection<CH_PWP_Archivo> CH_PWP_Archivos { get; set; }
    }
}
