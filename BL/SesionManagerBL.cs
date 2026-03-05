using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public sealed class SesionManagerBL
    {
        private static SesionManagerBL _instance;
        private static readonly object _lock = new object();

        public UsuarioBE _Usuario { get; private set; } = null;

        private SesionManagerBL() { }
        public static SesionManagerBL GetInstance()
        {             
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SesionManagerBL();
                    }
                }
            }
            return _instance;
        }

        public void LogIn(UsuarioBE usuario)
        {
            if (usuario != null)
            _Usuario = usuario;
            // Aquí podrías agregar lógica adicional, como registrar la hora de inicio de sesión, etc.
            
        }
        public void LogOut()
        {
            _Usuario = null;
            // Aquí podrías agregar lógica adicional, como registrar la hora de cierre de sesión, etc.
        }
    }
}
