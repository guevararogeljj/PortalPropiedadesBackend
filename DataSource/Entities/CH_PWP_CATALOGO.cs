using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class CH_PWP_CATALOGO
    {
        public int Id { get; set; }
        public string Clave { get; set; } = null!;
        public int Valor { get; set; }
        public string? Descripcion { get; set; }
        public int? Asociacion { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Update_At { get; set; }
    }
}
