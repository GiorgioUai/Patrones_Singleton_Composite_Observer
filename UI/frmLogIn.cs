using BE;
using BL;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class frmLogIn : Form
    {
        public frmLogIn()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Instanciamos la lógica de negocio (BL)
                // El constructor por defecto de UsuarioBL se encarga de crear el DAO internamente.
                UsuarioBL negocio = new UsuarioBL();

                // 2. Llamamos al método LogIn pasándole los datos de los TextBox
                // La BL hará el Hash de la clave y consultará a la DAL.
                bool exito = negocio.LogIn(txtUsuario.Text, txtPassword.Text);

                if (exito)
                {
                    // 3. Si tuvo éxito, los datos ya están en el Singleton gracias a la BL.
                    // Accedemos al Singleton para saludar al usuario.
                    var usuarioLogueado = SesionManagerBL.GetInstance()._Usuario;
                    MessageBox.Show($"¡Sesión iniciada correctamente!\nBienvenido: {usuarioLogueado.Nombre} {usuarioLogueado.Apellido}",
                                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Aquí podrías cerrar este form y abrir el Main
                    // this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Email o contraseña incorrectos. Por favor, reintente.",
                                    "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                // Capturamos errores de conexión o configuración sin exponer detalles técnicos a la UI
                MessageBox.Show("Ocurrió un inconveniente al intentar ingresar: " + ex.Message,
                                "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {            
            Application.Exit();
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            // Verificamos si hay alguien en sesión a través del Singleton
            if (SesionManagerBL.GetInstance()._Usuario != null)
            {
                MessageBox.Show($"Bienvenido, {SesionManagerBL.GetInstance()._Usuario.Nombre}");
            }
            else
            {
                MessageBox.Show("No hay nadie logueado actualmente.");
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            SesionManagerBL.GetInstance().LogOut();
            limpiarCampos();
            MessageBox.Show("Sesión cerrada correctamente.");
        }
        private void limpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtUsuario.Clear();
            txtPassword.Clear();
            txtUsuario.Focus();
        }
    }
}
