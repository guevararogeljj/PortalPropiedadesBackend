using System;
using System.Collections.Generic;

namespace DataSource.Entities
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Correo { get; set; } = null!;
        public string? NombreCompleto { get; set; }
        public string Rol { get; set; } = null!;
        public int? estatus { get; set; }
    }
}
