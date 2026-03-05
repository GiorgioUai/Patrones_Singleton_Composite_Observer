using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class PermisoBE : ComponenteBE
    {
        // Al ser una "Hoja", no permitimos agregar hijos. 
        // Es correcto lanzar la excepción para respetar el contrato del Composite.
        public override void AgregarHijo(ComponenteBE hijo)
        {
            throw new NotSupportedException("Un permiso simple no puede contener hijos.");
        }

        public override void QuitarHijo(ComponenteBE hijo)
        {
            throw new NotSupportedException("Un permiso simple no posee hijos para quitar.");
        }

        // MEJOR PRÁCTICA: Devolver una colección vacía constante o un Array vacío 
        // para evitar instanciar Listas nuevas sin sentido.
        public override IReadOnlyCollection<ComponenteBE> ObtenerHijos()
        {
            // Array.Empty es la forma más performante de devolver una colección de solo lectura vacía.
            return Array.Empty<ComponenteBE>();
        }

        public override bool ValidarPermisos(string permisoBuscado)
        {
            if (string.IsNullOrEmpty(permisoBuscado)) return false;

            // Comparamos el nombre del permiso actual con el buscado.
            return this.Nombre.Equals(permisoBuscado, StringComparison.OrdinalIgnoreCase);
        }
    }
}