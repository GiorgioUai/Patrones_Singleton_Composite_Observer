using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BE.Interfaces;
using BL;

namespace UI
{
    /// <summary>
    /// Formulario de autenticación de usuarios con soporte Multiidioma.
    /// Implementa el patrón Observer para reaccionar a cambios de idioma.
    /// </summary>
    public partial class frmLogIn : Form, IIdiomaObserver
    {
        #region "Atributos Privados"

        private readonly IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();
        private readonly SesionManagerBL _sesionManager = SesionManagerBL.GetInstance();
        private bool _placeholderActivo = true;

        #endregion

        #region "Constructor y Observer"

        public frmLogIn()
        {
            InitializeComponent();
        }

        private void frmLogIn_Load(object sender, EventArgs e)
        {
            // Suscripción al sistema de notificaciones de idioma
            _idiomaManager.Suscribir(this);

            txtPassword.PasswordChar = '*';
            txtPassword.UseSystemPasswordChar = true;

            ActualizarIdioma();
        }

        private void frmLogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Desuscripción obligatoria para liberar recursos
            _idiomaManager.Desuscribir(this);
        }

        #endregion

        #region "Implementación IIdiomaObserver"

        public void ActualizarIdioma()
        {
            // Traducción técnica de los controles mediante sus Tags
            Traductor.Traducir(this.Controls);

            if (_placeholderActivo) EstablecerPlaceholder();

            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        #endregion

        #region "Lógica de UX y Login"

        private void EstablecerPlaceholder()
        {
            txtUsuario.Text = _idiomaManager.ObtenerTexto("txt_Placeholder_Email");
            txtUsuario.ForeColor = Color.Gray;
            _placeholderActivo = true;
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            string placeholderTraducido = _idiomaManager.ObtenerTexto("txt_Placeholder_Email");

            if (txtUsuario.Text == placeholderTraducido)
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.Black;
                _placeholderActivo = false;
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                EstablecerPlaceholder();
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBL negocio = new UsuarioBL();
                string emailParaLogin = _placeholderActivo ? "" : txtUsuario.Text;

                // 1. Solicitud de autenticación a la capa de negocio
                UsuarioBE usuarioLogueado = negocio.LogIn(emailParaLogin, txtPassword.Text);

                if (usuarioLogueado != null)
                {
                    // 2. Validación de Estado de Seguridad (Link entre formularios)
                    if (usuarioLogueado.DebeCambiarPassword)
                    {
                        MessageBox.Show(_idiomaManager.ObtenerTexto("msg_CambioClaveObligatorio"),
                                        "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        // Invocación modal del formulario de cambio de clave
                        using (frmCambioPassword frmCambio = new frmCambioPassword(usuarioLogueado))
                        {
                            if (frmCambio.ShowDialog() == DialogResult.OK)
                            {
                                // Si el cambio fue exitoso, obligamos a re-ingresar con la nueva clave
                                MessageBox.Show(_idiomaManager.ObtenerTexto("msg_ReingresoNecesario"), "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiarCampos();
                            }
                            else
                            {
                                // Si cancela, limpiamos y no permitimos el acceso
                                limpiarCampos();
                            }
                        }
                    }
                    else
                    {
                        // 3. Flujo normal de acceso al sistema
                        string msgExito = _idiomaManager.ObtenerTexto("msg_SesionIniciada");
                        MessageBox.Show($"{msgExito}\nBienvenido: {usuarioLogueado.Nombre}", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        frmPrincipal principal = new frmPrincipal();
                        principal.Show();
                        this.Hide();
                    }
                }
                else
                {
                    // Error de credenciales
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_ErrorLogin"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Crítico");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e) => Application.Exit();

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación de campos mandatorios
                if (_placeholderActivo || string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_CamposIncompletos"),
                                    _idiomaManager.ObtenerTexto("cap_Atencion"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UsuarioBE nuevoUsuario = new UsuarioBE
                {
                    Email = txtUsuario.Text,
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    IdIdioma = _idiomaManager.IdiomaActual.Id,
                    DebeCambiarPassword = false // Estado inicial por defecto
                };

                UsuarioBL negocio = new UsuarioBL();
                if (negocio.Registrar(nuevoUsuario, txtPassword.Text))
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_RegistroExitoso"),
                                    _idiomaManager.ObtenerTexto("cap_Registro"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void limpiarCampos()
        {
            if (txtNombre != null) txtNombre.Clear();
            if (txtApellido != null) txtApellido.Clear();
            txtPassword.Text = string.Empty;
            txtUsuario.Text = "";
            EstablecerPlaceholder();
            txtUsuario.Focus();
        }

        #endregion
    }
}