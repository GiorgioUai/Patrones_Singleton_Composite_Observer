using BE;
using BE.Interfaces;
using BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class frmABMRoles : Form, IIdiomaObserver, ISesionObserver
    {
        #region "Atributos Privados"
        private PermisoBL _permisoBL = new PermisoBL();
        private CompuestoBE _rolSeleccionado;
        private List<ComponenteBE> _catalogoCompleto;

        private readonly IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();
        private readonly SesionManagerBL _sesionManager = SesionManagerBL.GetInstance();
        #endregion

        public frmABMRoles()
        {
            InitializeComponent();
            ConfigurarEsteticaFormulario();
        }

        #region "Eventos de Formulario (Ciclo de Vida)"
        private void frmABMRoles_Load(object sender, EventArgs e)
        {
            _idiomaManager.Suscribir(this);
            _sesionManager.Suscribir(this);

            ConfigurarGrillasVisuales();
            ActualizarGrillas();

            ActualizarIdioma();
            ActualizarSesion();
        }

        private void frmABMRoles_FormClosing(object sender, FormClosingEventArgs e)
        {
            _idiomaManager.Desuscribir(this);
            _sesionManager.Desuscribir(this);
        }
        #endregion

        #region "Implementación de Interfaces (Observer)"
        public void ActualizarIdioma()
        {
            Traductor.Traducir(this.Controls);
            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        public void ActualizarSesion()
        {
            if (!_sesionManager.TienePermiso("Seguridad_GestionRoles"))
            {
                this.Close();
            }
        }
        #endregion

        #region "Eventos de Grillas y Selección"
        private void dgvListaDeRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var seleccionado = (CompuestoBE)dgvListaDeRoles.Rows[e.RowIndex].DataBoundItem;
                _rolSeleccionado = (CompuestoBE)_permisoBL.ObtenerEstructuraCompleta(seleccionado.Id);
                txtDatosDelRol.Text = _rolSeleccionado.Nombre;
                ActualizarEstructuraVisual(_rolSeleccionado);
            }
        }
        #endregion

        #region "Eventos de Botones"
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvCatalogoDeComponentes.CurrentRow == null || _rolSeleccionado == null) return;

            ComponenteBE seleccionado = (ComponenteBE)dgvCatalogoDeComponentes.CurrentRow.DataBoundItem;

            if (seleccionado is CompuestoBE)
            {
                seleccionado = _permisoBL.ObtenerEstructuraCompleta(seleccionado.Id);
            }

            string nombreConflicto;
            if (_permisoBL.EstructuraContieneDuplicados(_rolSeleccionado, seleccionado, out nombreConflicto))
            {
                MessageBox.Show($"Inconsistencia detectada: El componente '{seleccionado.Nombre}' contiene elementos (como '{nombreConflicto}') que ya existen en el Rol actual.",
                                "Validación de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _rolSeleccionado.AgregarHijo(seleccionado);
            ActualizarEstructuraVisual(_rolSeleccionado);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_rolSeleccionado != null)
                {
                    if (string.IsNullOrWhiteSpace(txtDatosDelRol.Text)) throw new Exception("Nombre inválido.");
                    _rolSeleccionado.Nombre = txtDatosDelRol.Text.Trim();
                    _permisoBL.GuardarRol(_rolSeleccionado);
                    MessageBox.Show("Guardado exitoso.");
                    ActualizarGrillas();
                    FinalizarEdicion();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Métodos originales en una sola línea integrados en su región correspondiente
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            FinalizarEdicion();
            _rolSeleccionado = new CompuestoBE();
            txtDatosDelRol.Focus();
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (_rolSeleccionado != null)
            {
                _rolSeleccionado.Nombre = txtDatosDelRol.Text;
                _rolSeleccionado.Estado = ComponenteBE.EstadoEntidad.Modificado;
                ActualizarEstructuraVisual(_rolSeleccionado);
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_rolSeleccionado != null && _rolSeleccionado.Id > 0)
            {
                _permisoBL.EliminarPermiso(_rolSeleccionado);
                FinalizarEdicion();
            }
        }
        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (tvEstructuraDelRol.SelectedNode != null)
            {
                _rolSeleccionado.QuitarHijo((ComponenteBE)tvEstructuraDelRol.SelectedNode.Tag);
                ActualizarEstructuraVisual(_rolSeleccionado);
            }
        }
        private void btnVolver_Click(object sender, EventArgs e) => this.Close();
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (_rolSeleccionado != null)
                _rolSeleccionado.LimpiarEstados();
            FinalizarEdicion();
        }
        #endregion

        #region "Métodos de Soporte Visual (UX)"
        private void ActualizarEstructuraVisual(CompuestoBE pRol)
        {
            tvEstructuraDelRol.Nodes.Clear();
            if (pRol == null || (pRol.Id == 0 && string.IsNullOrEmpty(pRol.Nombre))) return;
            TreeNode root = new TreeNode(pRol.Nombre) { Tag = pRol };
            tvEstructuraDelRol.Nodes.Add(root);
            GenerarNodosRecursivos(root, pRol);
            tvEstructuraDelRol.ExpandAll();
        }

        private void GenerarNodosRecursivos(TreeNode padreVisual, ComponenteBE padreLogico)
        {
            foreach (var hijo in padreLogico.ObtenerHijos())
            {
                TreeNode nuevoNodo = new TreeNode(hijo.Nombre) { Tag = hijo };
                padreVisual.Nodes.Add(nuevoNodo);
                if (hijo is CompuestoBE) GenerarNodosRecursivos(nuevoNodo, hijo);
            }
        }

        private void ConfigurarGrillasVisuales()
        {
            FormatearGrilla(dgvListaDeRoles);
            FormatearGrilla(dgvCatalogoDeComponentes);
            dgvListaDeRoles.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 40 });
            dgvListaDeRoles.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Rol", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvCatalogoDeComponentes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 40 });
            dgvCatalogoDeComponentes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Componente", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        }

        private void FormatearGrilla(DataGridView pGrilla)
        {
            pGrilla.AutoGenerateColumns = false;
            pGrilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            pGrilla.MultiSelect = false;
            pGrilla.ReadOnly = true;
            pGrilla.RowHeadersVisible = false;
            pGrilla.AllowUserToResizeColumns = false;
            pGrilla.AllowUserToResizeRows = false;
            pGrilla.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
        }

        private void ActualizarGrillas()
        {
            try
            {
                _catalogoCompleto = _permisoBL.ListarTodo();
                dgvCatalogoDeComponentes.DataSource = null;
                dgvCatalogoDeComponentes.DataSource = _catalogoCompleto;
                dgvListaDeRoles.DataSource = null;
                dgvListaDeRoles.DataSource = _catalogoCompleto.Where(x => x is CompuestoBE).ToList();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void FinalizarEdicion()
        {
            if (_rolSeleccionado != null) _rolSeleccionado.LimpiarEstados();
            _rolSeleccionado = null;
            txtDatosDelRol.Clear();
            tvEstructuraDelRol.Nodes.Clear();
            ActualizarGrillas();
        }

        private void ConfigurarEsteticaFormulario()
        {
            this.Text = "Gestión de Roles Jerárquicos";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Tag = "frm_ABMRoles";
        }
        #endregion
    }
}