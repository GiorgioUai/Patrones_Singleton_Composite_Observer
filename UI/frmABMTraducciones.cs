using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BE.Interfaces;
using BL;

namespace UI
{
    public partial class frmABMTraducciones : Form, IIdiomaObserver
    {
        #region "Atributos Privados"

        private readonly IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();
        private List<IdiomaBE> _idiomas;

        #endregion

        #region "Constructor y Observer"

        public frmABMTraducciones()
        {
            InitializeComponent();
        }

        private void frmABMTraducciones_Load(object sender, EventArgs e)
        {
            _idiomaManager.Suscribir(this);

            ConfigurarGrilla();
            CargarIdiomas();
            ActualizarIdioma();
        }

        private void frmABMTraducciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            _idiomaManager.Desuscribir(this);
        }

        public void ActualizarIdioma()
        {
            Traductor.Traducir(this.Controls);

            // Decoración y Traducción de Encabezados
            if (dgvTraducciones.Columns.Count >= 2)
            {
                dgvTraducciones.Columns[0].HeaderText = _idiomaManager.ObtenerTexto("lbl_Nombre_Tag");
                dgvTraducciones.Columns[1].HeaderText = _idiomaManager.ObtenerTexto("lbl_Texto_Traduccion");
            }

            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        #endregion

        #region "Métodos de Configuración y Carga"

        private void ConfigurarGrilla()
        {
            dgvTraducciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTraducciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTraducciones.MultiSelect = false;
            dgvTraducciones.AllowUserToAddRows = false;
            dgvTraducciones.ReadOnly = true;
            dgvTraducciones.RowHeadersVisible = false;

            if (dgvTraducciones.Columns.Count == 0)
            {
                dgvTraducciones.Columns.Add("Tag", "Tag");
                dgvTraducciones.Columns.Add("Texto", "Traducción");
            }
        }

        private void CargarIdiomas()
        {
            _idiomas = _idiomaManager.ObtenerIdiomas();
            cmbIdiomas.DataSource = null;
            cmbIdiomas.DataSource = _idiomas;
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "Id";
        }

        private void CargarGrilla()
        {
            if (cmbIdiomas.SelectedItem is IdiomaBE idioma)
            {
                dgvTraducciones.Rows.Clear();
                List<string> tags = _idiomaManager.ListarTagsExistentes();

                foreach (string tag in tags)
                {
                    string texto = _idiomaManager.ObtenerTextoPorIdioma(tag, idioma.Nombre);
                    dgvTraducciones.Rows.Add(tag, texto);
                }
            }
        }

        private void LimpiarCampos()
        {
            txtNombreTag.Clear();
            txtTraduccion.Clear();
            txtNombreTag.Enabled = true;
            dgvTraducciones.ClearSelection();
        }

        #endregion

        #region "Eventos de Controles"

        private void cmbIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void dgvTraducciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombreTag.Text = dgvTraducciones.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtTraduccion.Text = dgvTraducciones.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                txtNombreTag.Enabled = false;
            }
        }

        #endregion

        #region "Botones de Acción"

        private void btnNuevoTag_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombreTag.Text))
            {
                _idiomaManager.GuardarEtiqueta(txtNombreTag.Text.Trim());
                MessageBox.Show("Tag registrado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrilla();
                LimpiarCampos();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbIdiomas.SelectedItem is IdiomaBE idioma && !string.IsNullOrWhiteSpace(txtNombreTag.Text))
            {
                _idiomaManager.GuardarTraduccion(idioma.Id, txtNombreTag.Text.Trim(), txtTraduccion.Text.Trim());
                CargarGrilla();
                LimpiarCampos();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e) => btnGuardar_Click(sender, e);

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreTag.Text)) return;

            var result = MessageBox.Show("¿Desea eliminar este Tag y todas sus traducciones?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _idiomaManager.EliminarEtiqueta(txtNombreTag.Text.Trim());
                CargarGrilla();
                LimpiarCampos();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => LimpiarCampos();

        private void btnVolver_Click(object sender, EventArgs e) => this.Close();

        #endregion
    }
}