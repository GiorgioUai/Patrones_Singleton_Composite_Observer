using System;
using System.Collections.Generic;

namespace BE
{
    public class UsuarioBE
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Usamos una lista privada y una propiedad pública blindada
        private List<ComponenteBE> _permisos = new List<ComponenteBE>();

        public IReadOnlyCollection<ComponenteBE> Permisos => _permisos.AsReadOnly();

        public void AgregarPermiso(ComponenteBE permiso)
        {
            _permisos.Add(permiso);
        }

        // El método de conveniencia que usará la UI
        public bool ValidarPermisos(string nombrePermiso)
        {
            foreach (var p in _permisos)
            {
                if (p.ValidarPermisos(nombrePermiso)) return true;
            }
            return false;
        }
    }
}