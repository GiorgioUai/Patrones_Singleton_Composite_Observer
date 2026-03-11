using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BE;
using BE.Interfaces;
using BL;

namespace UI
{
    public partial class frmGestionUsuariosPermiso : Form, IIdiomaObserver, ISesionObserver
    {
        #region "Atributos Privados"

        private readonly UsuarioBL _usuarioBL;
        private readonly PermisoBL _permisoBL;
        private UsuarioBE _usuarioSeleccionado;
        private List<UsuarioBE> _cacheUsuarios;
        private Timer _tmrDebounce;

        // Managers para el patrón Observer
        private readonly SesionManagerBL _sesionManager = SesionManagerBL.GetInstance();
        private readonly IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();

        #endregion

        #region "Constructor y Ciclo de Vida (Observer)"

        public frmGestionUsuariosPermiso()
        {
            InitializeComponent();
            _usuarioBL = new UsuarioBL();
            _permisoBL = new PermisoBL();

            ConfigurarDebounce();
            DecorarControles();

            this.Resize += (s, e) => AlinearContenedorAcciones();
        }

        private void frmGestionUsuariosPermiso_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            // SUSCRIPCIÓN AL PATRÓN OBSERVER
            _sesionManager.Suscribir(this);
            _idiomaManager.Suscribir(this);

            CargarUsuarios();
            CargarCatalogoMaestro();

            // Ejecución inicial de actualizaciones
            ActualizarIdioma();
            ActualizarSesion();

            this.PerformLayout();
            AlinearContenedorAcciones();
        }

        private void frmGestionUsuariosPermiso_FormClosing(object sender, FormClosingEventArgs e)
        {
            // DESUSCRIPCIÓN OBLIGATORIA: Libera la referencia en el Singleton
            _sesionManager.Desuscribir(this);
            _idiomaManager.Desuscribir(this);
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
            // Valida el permiso requerido para este formulario específico
            if (!_sesionManager.TienePermiso("Seguridad_Asignacion"))
            {
                this.Close();
            }
        }

        #endregion

        #region "Lógica de Carga y Datos"

        private void ConfigurarDebounce()
        {
            _tmrDebounce = new Timer();
            _tmrDebounce.Interval = 300;
            _tmrDebounce.Tick += (s, e) => { _tmrDebounce.Stop(); EjecutarFiltrado(); };
        }

        private void CargarUsuarios()
        {
            _cacheUsuarios = _usuarioBL.ListarTodos();
            ActualizarGrilla(_cacheUsuarios);
        }

        private void ActualizarGrilla(List<UsuarioBE> lista)
        {
            dgvListaDeUsuarios.DataSource = null;
            dgvListaDeUsuarios.DataSource = lista;
            if (dgvListaDeUsuarios.Columns["Id"] != null) dgvListaDeUsuarios.Columns["Id"].Visible = false;
            if (dgvListaDeUsuarios.Columns["IdIdioma"] != null) dgvListaDeUsuarios.Columns["IdIdioma"].Visible = false;
            if (dgvListaDeUsuarios.Columns["Password"] != null) dgvListaDeUsuarios.Columns["Password"].Visible = false;
            dgvListaDeUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarCatalogoMaestro()
        {
            tvCatalogoPermisos.BeginUpdate();
            try
            {
                tvCatalogoPermisos.Nodes.Clear();
                var listaRaiz = _permisoBL.ListarTodo();

                foreach (var item in listaRaiz)
                {
                    tvCatalogoPermisos.Nodes.Add(CrearNodoRecursivo(item));
                }

                tvCatalogoPermisos.ExpandAll();
            }
            finally
            {
                tvCatalogoPermisos.EndUpdate();
            }
        }

        #endregion

        #region "Decoración y Adaptabilidad UI"

        private void DecorarControles()
        {
            dgvListaDeUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListaDeUsuarios.MultiSelect = false;
            dgvListaDeUsuarios.ReadOnly = true;
            dgvListaDeUsuarios.RowHeadersVisible = false;
            dgvListaDeUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dgvListaDeUsuarios.BackgroundColor = Color.White;
            dgvListaDeUsuarios.AllowUserToAddRows = false;

            dgvListaDeUsuarios.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(dgvListaDeUsuarios, true);

            dgvListaDeUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tvCatalogoPermisos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tvPermisosAsignados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            pnlAcciones.Anchor = AnchorStyles.None;
            pnlAcciones.BackColor = Color.Transparent;
            pnlAcciones.Size = new Size(100, 200);

            btnAgregar.Size = new Size(80, 70);
            btnQuitar.Size = new Size(80, 70);
            btnAgregar.Text = "AGREGAR" + Environment.NewLine + ">>";
            btnQuitar.Text = "QUITAR" + Environment.NewLine + "<<";
            btnAgregar.TextAlign = ContentAlignment.MiddleCenter;
            btnQuitar.TextAlign = ContentAlignment.MiddleCenter;
            btnAgregar.Anchor = AnchorStyles.None;
            btnQuitar.Anchor = AnchorStyles.None;

            btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnVolver.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            tvCatalogoPermisos.FullRowSelect = true;
            tvCatalogoPermisos.ShowLines = true;
            tvPermisosAsignados.FullRowSelect = true;
            tvPermisosAsignados.ShowLines = true;

            if (imageList1 != null)
            {
                tvCatalogoPermisos.ImageList = imageList1;
                tvPermisosAsignados.ImageList = imageList1;
            }
        }

        private void AlinearContenedorAcciones()
        {
            pnlAcciones.BringToFront();
            int xInicio = tvCatalogoPermisos.Right;
            int xFin = tvPermisosAsignados.Left;
            int centroX = xInicio + ((xFin - xInicio) / 2);
            int centroY = this.ClientSize.Height / 2;
            pnlAcciones.Location = new Point(centroX - (pnlAcciones.Width / 2), centroY - (pnlAcciones.Height / 2));
            pnlAcciones.Invalidate();
        }

        #endregion

        #region "Eventos de Selección y Búsqueda"

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            _tmrDebounce.Stop();
            _tmrDebounce.Start();
        }

        private void EjecutarFiltrado()
        {
            string busqueda = txtFiltro.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                ActualizarGrilla(_cacheUsuarios);
            }
            else
            {
                var filtrados = _cacheUsuarios.Where(u =>
                    u.Nombre.ToLower().Contains(busqueda) ||
                    u.Apellido.ToLower().Contains(busqueda) ||
                    u.Email.ToLower().Contains(busqueda)
                ).ToList();
                ActualizarGrilla(filtrados);
            }
        }

        private void dgvListaDeUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvListaDeUsuarios.CurrentRow == null) return;
            _usuarioSeleccionado = (UsuarioBE)dgvListaDeUsuarios.CurrentRow.DataBoundItem;
            _usuarioBL.CargarSeguridad(_usuarioSeleccionado);
            RefrescarArbolAsignados();
        }

        private void RefrescarArbolAsignados()
        {
            tvPermisosAsignados.Nodes.Clear();
            if (_usuarioSeleccionado != null)
            {
                foreach (var p in _usuarioSeleccionado.Permisos)
                {
                    tvPermisosAsignados.Nodes.Add(CrearNodoRecursivo(p));
                }
                tvPermisosAsignados.ExpandAll();
            }
        }

        #endregion

        #region "Acciones (Botones)"

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null || tvCatalogoPermisos.SelectedNode == null) return;
            ComponenteBE nuevo = (ComponenteBE)tvCatalogoPermisos.SelectedNode.Tag;

            bool yaLoTiene = false;
            string conflicto = "";
            foreach (var pExistente in _usuarioSeleccionado.Permisos)
            {
                if (_permisoBL.EstructuraContieneDuplicados(pExistente, nuevo, out conflicto))
                {
                    yaLoTiene = true;
                    break;
                }
                if (pExistente.Id == nuevo.Id) { yaLoTiene = true; conflicto = nuevo.Nombre; break; }
            }

            if (yaLoTiene)
            {
                MessageBox.Show($"El usuario ya cuenta con: {conflicto}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _usuarioSeleccionado.AgregarPermiso(nuevo);
            RefrescarArbolAsignados();
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null || tvPermisosAsignados.SelectedNode == null) return;
            TreeNode nodoActual = tvPermisosAsignados.SelectedNode;
            if (nodoActual.Parent != null)
            {
                TreeNode nodoRaiz = nodoActual;
                while (nodoRaiz.Parent != null) nodoRaiz = nodoRaiz.Parent;
                if (MessageBox.Show($"¿Desea quitar el Rol '{nodoRaiz.Text}'?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _usuarioSeleccionado.QuitarPermiso((ComponenteBE)nodoRaiz.Tag);
                    RefrescarArbolAsignados();
                }
            }
            else
            {
                _usuarioSeleccionado.QuitarPermiso((ComponenteBE)nodoActual.Tag);
                RefrescarArbolAsignados();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_usuarioSeleccionado != null && _usuarioBL.GuardarPermisos(_usuarioSeleccionado))
                {
                    MessageBox.Show("Cambios persistidos correctamente.", "Éxito");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void btnVolver_Click(object sender, EventArgs e) => this.Close();

        #endregion

        #region "Soporte UI - Recursividad"

        private TreeNode CrearNodoRecursivo(ComponenteBE componente)
        {
            TreeNode nodo = new TreeNode(componente.Nombre) { Tag = componente };
            if (componente is CompuestoBE compuesto)
            {
                nodo.ImageIndex = 0;
                nodo.SelectedImageIndex = 0;
                foreach (var hijo in compuesto.ObtenerHijos())
                {
                    nodo.Nodes.Add(CrearNodoRecursivo(hijo));
                }
            }
            else
            {
                nodo.ImageIndex = 1;
                nodo.SelectedImageIndex = 1;
            }
            return nodo;
        }

        #endregion
    }
}