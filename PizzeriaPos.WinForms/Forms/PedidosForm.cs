using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PizzeriaPos.WinForms.Services;

namespace PizzeriaPos.WinForms.Forms
{
    // Formulario de gestion de Pedidos con vista de detalle al hacer clic
    public partial class PedidosForm : Form
    {
        private DataGridView dgvPedidos = new();
        private DataGridView dgvDetalle = new();
        private Label lblPedidos = new();
        private Label lblDetalle = new();
        private Label lblInfoPedido = new();
        private Button btnCargar = new();
        private Button btnNuevoPedido = new();
        private readonly ApiService _api = new();

        public PedidosForm()
        {
            InicializarComponentes();
            _ = CargarPedidosAsync();
        }

        private void InicializarComponentes()
        {
            this.BackColor = Color.White;

            // Label titulo pedidos
            lblPedidos.Text = "Lista de Pedidos (clic para ver detalle):";
            lblPedidos.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPedidos.Location = new Point(10, 10);
            lblPedidos.AutoSize = true;

            // Boton cargar
            btnCargar.Text = "🔄 Actualizar";
            btnCargar.Location = new Point(600, 5);
            btnCargar.Size = new Size(120, 30);
            btnCargar.BackColor = Color.OrangeRed;
            btnCargar.ForeColor = Color.White;
            btnCargar.FlatStyle = FlatStyle.Flat;
            btnCargar.Click += async (s, e) => await CargarPedidosAsync();

            // Boton nuevo pedido
            btnNuevoPedido.Text = "+ Nuevo Pedido";
            btnNuevoPedido.Location = new Point(730, 5);
            btnNuevoPedido.Size = new Size(120, 30);
            btnNuevoPedido.BackColor = Color.DarkGreen;
            btnNuevoPedido.ForeColor = Color.White;
            btnNuevoPedido.FlatStyle = FlatStyle.Flat;
            btnNuevoPedido.Click += BtnNuevoPedido_Click;

            // Grid de pedidos
            dgvPedidos.Location = new Point(10, 40);
            dgvPedidos.Size = new Size(840, 200);
            dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.ReadOnly = true;
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.BackgroundColor = Color.White;
            dgvPedidos.CellClick += DgvPedidos_CellClick;

            // Label info pedido seleccionado
            lblInfoPedido.Text = "Seleccione un pedido para ver su detalle:";
            lblInfoPedido.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblInfoPedido.Location = new Point(10, 255);
            lblInfoPedido.AutoSize = true;

            // Label detalle
            lblDetalle.Text = "Detalle del pedido:";
            lblDetalle.Font = new Font("Segoe UI", 9);
            lblDetalle.Location = new Point(10, 275);
            lblDetalle.AutoSize = true;

            // Grid de detalle
            dgvDetalle.Location = new Point(10, 295);
            dgvDetalle.Size = new Size(840, 140);
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.ReadOnly = true;
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.BackgroundColor = Color.White;

            this.Controls.AddRange(new Control[]
            {
                lblPedidos, btnCargar, btnNuevoPedido,
                dgvPedidos, lblInfoPedido, lblDetalle, dgvDetalle
            });
        }

        // Carga todos los pedidos desde la API
        private async Task CargarPedidosAsync()
        {
            try
            {
                var pedidos = await _api.GetAsync<List<dynamic>>("Pedido");
                dgvPedidos.DataSource = null;
                dgvDetalle.DataSource = null;
                lblInfoPedido.Text = "Seleccione un pedido para ver su detalle:";

                if (pedidos != null)
                {
                    var tabla = pedidos.Select(p => new
                    {
                        Id = (int)p.id,
                        Cliente = p.cliente != null ? $"{p.cliente.nombre} {p.cliente.apellido}" : "N/A",
                        Estado = (string)p.estado,
                        Total = (decimal)p.total,
                        Fecha = DateTime.Parse(p.createdAt.ToString()).ToString("dd/MM/yyyy HH:mm")
                    }).ToList();

                    dgvPedidos.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pedidos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Al hacer clic en un pedido, muestra su detalle abajo
        private async void DgvPedidos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var id = (int)dgvPedidos.Rows[e.RowIndex].Cells["Id"].Value;

            try
            {
                var pedido = await _api.GetAsync<dynamic>($"Pedido/{id}");
                if (pedido == null) return;

                string cliente = pedido.cliente != null ? $"{pedido.cliente.nombre} {pedido.cliente.apellido}" : "N/A";
                lblInfoPedido.Text = $"Pedido #{id} — Cliente: {cliente} — Estado: {pedido.estado} — Total: L. {pedido.total}";

                // Cargar detalles del pedido
                dgvDetalle.DataSource = null;
                var detalles = pedido.detalles as IEnumerable<dynamic>;

                if (detalles != null)
                {
                    var tablaDetalle = detalles.Select(d => new
                    {
                        Producto = d.producto != null ? (string)d.producto.nombre : "N/A",
                        Cantidad = (int)d.cantidad,
                        PrecioUnitario = (decimal)d.precioUnitario,
                        Subtotal = (int)d.cantidad * (decimal)d.precioUnitario
                    }).ToList();

                    dgvDetalle.DataSource = tablaDetalle;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar detalle: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Boton nuevo pedido - abre formulario de creacion
        private async void BtnNuevoPedido_Click(object? sender, EventArgs e)
        {
            var form = new NuevoPedidoForm();
            form.ShowDialog();
            await CargarPedidosAsync();
        }
    }
}
