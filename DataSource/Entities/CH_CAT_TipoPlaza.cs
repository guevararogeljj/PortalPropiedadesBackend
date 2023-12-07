using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class CH_CAT_TipoPlaza
    {
        public CH_CAT_TipoPlaza()
        {
            CH_CAT_Municipios = new HashSet<CH_CAT_Municipio>();
            CH_PWP_Domicilios = new HashSet<CH_PWP_Domicilio>();
        }

        public int TP_IdEstadoFK { get; set; }
        public string TP_IdEstado { get; set; } = null!;
        public string TP_TipoPlaza { get; set; } = null!;
        public int TP_PlazoAdicional { get; set; }
        public string? TP_Etiqueta { get; set; }
        public double? TP_GarantiaJudicial { get; set; }
        public int? TP_DiasPrescipción { get; set; }
        public string? TP_Region { get; set; }
        public int? TP_Jurisdiccion { get; set; }
        public double? TP_Porcentaje_ImpPorAdjudicacion { get; set; }
        public string? TP_ISAI { get; set; }
        public double? TP_Porcentaje_ISAI { get; set; }
        public string? TP_Impuesto_Local { get; set; }
        public decimal? TP_Cerrajero { get; set; }
        public decimal? TP_Cargadores { get; set; }
        public decimal? TP_Fuerza_Publica { get; set; }
        public decimal? TP_Actuario { get; set; }
        public decimal? TP_Honorario_Notario { get; set; }
        public decimal? TP_Honorario_Gestor { get; set; }
        public decimal? TP_Derechos_Inscripcion { get; set; }
        public decimal? TP_Derechos_Cancelacion_Hipoteca { get; set; }
        public decimal? TP_CNA { get; set; }
        public decimal? TP_Avisos_Preventivos_Definitivos { get; set; }
        public decimal? TP_CLG { get; set; }
        public decimal? TP_Avaluo_Viaticos { get; set; }

        public virtual ICollection<CH_CAT_Municipio> CH_CAT_Municipios { get; set; }
        public virtual ICollection<CH_PWP_Domicilio> CH_PWP_Domicilios { get; set; }
    }
}
