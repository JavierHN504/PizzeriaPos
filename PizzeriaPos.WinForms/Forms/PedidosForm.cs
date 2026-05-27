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
using PizzeriaPos.WinForms.Models;

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
                var pedidos = await _api.GetAsync<List<PedidoCabeceraModel>>("Pedido");
                dgvPedidos.DataSource = null;
                dgvDetalle.DataSource = null;
                lblInfoPedido.Text = "Seleccione un pedido para ver su detalle:";

                if (pedidos != null && pedidos.Count > 0)
                {
                    var tabla = pedidos.Select(p => new
                    {
                        p.Id,
                        Cliente = p.Cliente != null ? $"{p.Cliente.Nombre} {p.Cliente.Apellido}" : "N/A",
                        p.Estado,
                        p.Total,
                        Fecha = p.CreatedAt.ToString("dd/MM/yyyy HH:mm")
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
                var pedido = await _api.GetAsync<PedidoCabeceraModel>($"Pedido/{id}");
                if (pedido == null) return;

                string cliente = pedido.Cliente != null ? $"{pedido.Cliente.Nombre} {pedido.Cliente.Apellido}" : "N/A";
                lblInfoPedido.Text = $"Pedido #{id} — Cliente: {cliente} — Estado: {pedido.Estado} — Total: L. {pedido.Total}";

                dgvDetalle.DataSource = null;

                if (pedido.Detalles != null && pedido.Detalles.Count > 0)
                {
                    var tablaDetalle = pedido.Detalles.Select(d => new
                    {
                        Producto = d.Producto != null ? d.Producto.Nombre : "N/A",
                        d.Cantidad,
                        d.PrecioUnitario,
                        Subtotal = d.Cantidad * d.PrecioUnitario
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
