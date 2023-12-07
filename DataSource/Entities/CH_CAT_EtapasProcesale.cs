using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class CH_CAT_EtapasProcesale
    {
        public CH_CAT_EtapasProcesale()
        {
            CH_PaginaWebPropiedades = new HashSet<CH_PaginaWebPropiedade>();
        }

        public int EP_IdEtapa { get; set; }
        public string EP_NombreEtapa { get; set; } = null!;
        public double EP_SaldoDeudor { get; set; }
        public double EP_SaldoInversionista { get; set; }
        public double EP_GarantiaDeudor { get; set; }
        public double EP_GarantiaInversionista { get; set; }
        public int? EP_DiasModeloEstandar { get; set; }
        public int? EP_DiasModeloExhorto { get; set; }
        public int? EP_DiasModeloEdicto { get; set; }
        public decimal? EP_JudicialEstandar { get; set; }
        public decimal? EP_NegociadoEstandar { get; set; }
        public decimal? EP_JudicialExhorto { get; set; }
        public decimal? EP_NegociadoExhorto { get; set; }
        public decimal? EP_JudicialEdicto { get; set; }
        public decimal? EP_NegociadoEdicto { get; set; }

        public virtual ICollection<CH_PaginaWebPropiedade> CH_PaginaWebPropiedades { get; set; }
    }
}
