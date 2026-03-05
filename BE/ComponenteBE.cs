using System;
using System.Collections.Generic;

namespace BE
{
    public abstract class ComponenteBE
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public abstract bool ValidarPermisos(string permisoBuscado);
                
        public abstract void AgregarHijo(ComponenteBE hijo);
        public abstract void QuitarHijo(ComponenteBE hijo);
        public abstract IReadOnlyCollection<ComponenteBE> ObtenerHijos();
    }
}