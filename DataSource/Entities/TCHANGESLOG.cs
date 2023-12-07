using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCHANGESLOG
    {
        public int ID { get; set; }
        public string? USERNAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? CHANGE { get; set; }
        public string? IP { get; set; }
        public DateTime? CREATE_AT { get; set; }
    }
}
