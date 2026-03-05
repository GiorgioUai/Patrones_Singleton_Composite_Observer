using System;
using System.Windows.Forms;
using BE;
using BL;

namespace UI
{
    public partial class frmABMPermisos : Form
    {
        private readonly PermisoBL _permisoBL;

        public frmABMPermisos()
        {
            InitializeComponent();
            // La UI sigue sin conocer la DAL gracias al constructor por defecto de la BL
            _permisoBL = new PermisoBL();
        }

        private void frmABMPermisos_Load(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                PermisoBE nuevoPermiso = new PermisoBE();
                nuevoPermiso.Nombre = txtPermiso.Text.Trim();

                _permisoBL.GuardarPermisoSimple(nuevoPermiso);

                MessageBox.Show("Permiso guardado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FinalizarOperacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Validamos que haya una fila seleccionada
            if (dgvListaDePermisos.CurrentRow != null)
            {
                // Recuperamos el objeto BE asociado a la fila
                var permiso = (PermisoBE)dgvListaDePermisos.CurrentRow.DataBoundItem;

                var rta = MessageBox.Show($"¿Está seguro de eliminar el permiso {permiso.Nombre}?",
                                          "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (rta == DialogResult.Yes)
                {
                    try
                    {
                        // Invocamos a la BL para ejecutar la eliminación validada
                        _permisoBL.EliminarPermiso(permiso);

                        MessageBox.Show("Permiso eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FinalizarOperacion();
                    }
                    catch (Exception ex)
                    {
                        // Capturamos errores de integridad o de base de datos
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un permiso de la lista.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FinalizarOperacion();
        }

        private void CargarGrilla()
        {
            dgvListaDePermisos.DataSource = null;
            dgvListaDePermisos.DataSource = _permisoBL.ListarPermisosSimples();
        }

        private void FinalizarOperacion()
        {
            txtPermiso.Clear();
            CargarGrilla();
        }
    }
}