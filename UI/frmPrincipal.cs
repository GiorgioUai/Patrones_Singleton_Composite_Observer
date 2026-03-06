using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BL;

namespace UI
{
    /// <summary>
    /// Formulario contenedor principal (MDI). 
    /// Implementa ISesionObserver e IIdiomaObserver para reaccionar a cambios de seguridad e idioma.
    /// </summary>
    public partial class frmPrincipal : Form, ISesionObserver, IIdiomaObserver
    {
        #region "Atributos / Variables Privadas"

        private SesionManagerBL _sesionManager = SesionManagerBL.GetInstance();
        private IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();

        #endregion

        #region "Constructor"

        public frmPrincipal()
        {
            InitializeComponent();
        }

        #endregion

        #region "Eventos del Formulario"

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            // 1. Configuración visual del menú de idiomas (Despliegue hacia la izquierda)
            // Usamos el nombre detectado en el diseñador: idiomasToolStripMenuItem
            idiomasToolStripMenuItem.DropDownDirection = ToolStripDropDownDirection.BelowLeft;

            // 2. Registro de este formulario como observador de sesión e idioma
            _sesionManager.Suscribir(this);
            _idiomaManager.Suscribir(this);

            // 3. Actualizaciones iniciales
            ActualizarSesion();
            ActualizarIdioma();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Desuscripción de ambos gestores para liberar recursos
            _sesionManager.Desuscribir(this);
            _idiomaManager.Desuscribir(this);
        }

        #endregion

        #region "Implementación de Interfaces (Observer)"

        /// <summary>
        /// Método gatillado por el SesionManagerBL cuando ocurre un Login o Logout.
        /// </summary>
        public void ActualizarSesion()
        {
            // Ejecutamos la validación de permisos sobre los ítems del menú
            ValidarSeguridadMenu(this.MainMenuStrip.Items);
        }

        /// <summary>
        /// Método gatillado por el IdiomaManagerBL cuando cambia el idioma global.
        /// </summary>
        public void ActualizarIdioma()
        {
            // 1. Traducir todos los controles y menús usando el Traductor centralizado de la UI
            Traductor.Traducir(this.Controls);

            // 2. Traducir el título del formulario si tiene Tag asignado
            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        #endregion

        #region "Lógica de Seguridad (Permisos)"

        /// <summary>
        /// Recorre recursivamente los ítems del menú evaluando la visibilidad según Roles y permisos.
        /// </summary>
        private void ValidarSeguridadMenu(ToolStripItemCollection pItems)
        {
            foreach (ToolStripItem item in pItems)
            {
                // Evaluación de visibilidad basada en permisos del usuario actual
                if (item.Tag != null)
                {
                    string permisoRequerido = item.Tag.ToString();

                    // Si el usuario tiene el permiso o rol correspondiente, el ítem es visible
                    if (_sesionManager._Usuario != null && _sesionManager._Usuario.ValidarPermisos(permisoRequerido))
                    {
                        item.Visible = true;
                    }
                    else
                    {
                        // En aplicaciones profesionales, las opciones que el usuario no puede usar se ocultan
                        item.Visible = false;
                    }
                }

                // Aplicar recursividad si el ítem tiene sub-menús (DropDownItems)
                if (item is ToolStripMenuItem menuItem && menuItem.DropDownItems.Count > 0)
                {
                    ValidarSeguridadMenu(menuItem.DropDownItems);
                }
            }
        }

        #endregion

        #region "Eventos de Cambio de Idioma (Menú)"

        private void españolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _idiomaManager.CambiarIdioma("ES");
        }

        private void inglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _idiomaManager.CambiarIdioma("EN");
        }

        #endregion

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Limpiamos la sesión lógica en el Singleton
            SesionManagerBL.GetInstance().LogOut();

            // 2. Buscamos el formulario de Login que está oculto y lo casteamos al tipo correcto
            frmLogIn login = (frmLogIn)Application.OpenForms["frmLogIn"];

            if (login != null)
            {
                // 3. LIMPIEZA: Borramos password y reseteamos el placeholder antes de mostrarlo
                login.limpiarCampos();

                login.Show();
                this.Close();
            }
        }
    }
}