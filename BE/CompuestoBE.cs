using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BE
{
    public class CompuestoBE : ComponenteBE
    {
        private List<ComponenteBE> _hijos = new List<ComponenteBE>();

        public override void AgregarHijo(ComponenteBE hijo)
        {
            _hijos.Add(hijo);
        }

        public override void QuitarHijo(ComponenteBE hijo)
        {
            _hijos.Remove(hijo);
        }

        public override IReadOnlyCollection<ComponenteBE> ObtenerHijos()
        {            
            return _hijos.AsReadOnly();
        }

        public override bool ValidarPermisos(string permisoBuscado)
        {
            foreach (var hijo in _hijos)
            {
                if (hijo.ValidarPermisos(permisoBuscado)) return true;
            }
            return false;
        }
    }
}