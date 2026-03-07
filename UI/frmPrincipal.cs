using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BE.Interfaces;
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
            // 1. Configuración visual del menú de idiomas
            idiomasToolStripMenuItem.DropDownDirection = ToolStripDropDownDirection.BelowLeft;

            // 2. Registro de este formulario como observador
            _sesionManager.Suscribir(this);
            _idiomaManager.Suscribir(this);

            // 3. Actualizaciones iniciales
            ActualizarSesion();
            ActualizarIdioma();
        }

        /// <summary>
        /// Evento central que controla cualquier intento de cierre del formulario (incluyendo la "X").
        /// </summary>
        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 1. Verificamos si hay una sesión activa para pedir confirmación
            if (_sesionManager._Usuario != null)
            {
                string mensaje = _idiomaManager.ObtenerTexto("msg_ConfirmarLogout");
                string titulo = _idiomaManager.ObtenerTexto("cap_Confirmacion");

                DialogResult respuesta = MessageBox.Show(mensaje, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.No)
                {
                    // Si el usuario se arrepiente, cancelamos el cierre de la ventana
                    e.Cancel = true;
                    return;
                }

                // 2. Si confirma (SI), procedemos a cerrar la sesión lógica antes de que el form desaparezca
                _sesionManager.LogOut();
            }

            // 3. Desuscripción de ambos gestores para liberar recursos y evitar fugas de memoria
            _sesionManager.Desuscribir(this);
            _idiomaManager.Desuscribir(this);
        }

        #endregion

        #region "Implementación de Interfaces (Observer)"

        public void ActualizarSesion()
        {
            ValidarSeguridadMenu(this.MainMenuStrip.Items);
        }

        public void ActualizarIdioma()
        {
            Traductor.Traducir(this.Controls);

            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        #endregion

        #region "Lógica de Seguridad (Permisos)"

        private void ValidarSeguridadMenu(ToolStripItemCollection pItems)
        {
            foreach (ToolStripItem item in pItems)
            {
                if (item.Tag != null)
                {
                    string permisoRequerido = item.Tag.ToString();
                    item.Visible = _sesionManager.TienePermiso(permisoRequerido);
                }

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

        #region "Cierre de Sesión"

        /// <summary>
        /// Al cerrar desde el menú, simplemente invocamos el cierre del formulario.
        /// La confirmación y el LogOut se gestionan automáticamente en FormClosing.
        /// </summary>
        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();

            // Si el cierre fue exitoso (el usuario confirmó), mostramos el login limpio
            if (_sesionManager._Usuario == null)
            {
                frmLogIn login = (frmLogIn)Application.OpenForms["frmLogIn"];
                if (login != null)
                {
                    login.limpiarCampos();
                    login.Show();
                }
            }
        }

        #endregion
    }
}