using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TPARAMETERS
    {
        public int ID { get; set; }
        public string NAME { get; set; } = null!;
        public string VALUE { get; set; } = null!;
        public string GROUPNAME { get; set; } = null!;
        public DateTime? CREATED_AT { get; set; }
    }
}
