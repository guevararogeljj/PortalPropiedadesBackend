using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class TCONTACTS
    {
        public int ID { get; set; }
        public string? FULLNAME { get; set; }
        public string? EMAIL { get; set; }
        public string? CELLPHONE { get; set; }
        public string? MESSAGE { get; set; }
        public DateTime? CREATE_AT { get; set; }
    }
}
