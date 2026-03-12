using System;
using System.Windows.Forms;
using BE;
using BE.Interfaces;
using BL;

namespace UI
{
    /// <summary>
    /// Formulario para el cambio obligatorio o voluntario de contraseña.
    /// Implementa el patrón Observer para soporte multiidioma.
    /// </summary>
    public partial class frmCambioPassword : Form, IIdiomaObserver
    {
        #region "Atributos Privados"

        private readonly IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();
        private readonly UsuarioBL _usuarioBL = new UsuarioBL();
        private UsuarioBE _usuario;

        #endregion

        #region "Constructor y Observer"

        /// <summary>
        /// Inicializa una nueva instancia del formulario con el usuario a modificar.
        /// </summary>
        /// <param name="pUsuario">Entidad de usuario que requiere el cambio.</param>
        public frmCambioPassword(UsuarioBE pUsuario)
        {
            InitializeComponent();
            _usuario = pUsuario;
        }

        private void frmCambioPassword_Load(object sender, EventArgs e)
        {
            // Registro en el sistema de notificación de idiomas
            _idiomaManager.Suscribir(this);

            // Configuración de máscara para datos sensibles
            txtPasswordNueva.PasswordChar = '*';
            txtPasswordRepetir.PasswordChar = '*';

            ActualizarIdioma();
        }

        private void frmCambioPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Desuscripción obligatoria para liberar recursos
            _idiomaManager.Desuscribir(this);
        }

        #endregion

        #region "Implementación IIdiomaObserver"

        public void ActualizarIdioma()
        {
            Traductor.Traducir(this.Controls);

            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        #endregion

        #region "Lógica de Negocio y Eventos"

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validaciones de consistencia de interfaz
                if (string.IsNullOrWhiteSpace(txtPasswordNueva.Text) || string.IsNullOrWhiteSpace(txtPasswordRepetir.Text))
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_CamposIncompletos"), "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtPasswordNueva.Text != txtPasswordRepetir.Text)
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_PasswordsNoCoinciden"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Ejecución del cambio a través de la capa de negocio
                if (_usuarioBL.CambiarPassword(_usuario, txtPasswordNueva.Text))
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_CambioPasswordExitoso"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_ErrorCambioPassword"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}