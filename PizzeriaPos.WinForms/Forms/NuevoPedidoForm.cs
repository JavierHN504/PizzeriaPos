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
    // Formulario para crear un nuevo pedido
    public partial class NuevoPedidoForm : Form
    {
        private ComboBox cmbCliente = new();
        private ComboBox cmbProducto = new();
        private TextBox txtCantidad = new();
        private TextBox txtObservaciones = new();
        private DataGridView dgvDetalle = new();
        private Label lblCliente = new();
        private Label lblProducto = new();
        private Label lblCantidad = new();
        private Label lblObservaciones = new();
        private Label lblTotal = new();
        private Button btnAgregarProducto = new();
        private Button btnCrearPedido = new();
        private Button btnCancelar = new();
        private readonly ApiService _api = new();
        private List<dynamic> _clientes = new();
        private List<dynamic> _productos = new();
        private List<(int ProductoId, string Nombre, int Cantidad, decimal Precio)> _detalles = new();
        private decimal _total = 0;

        public NuevoPedidoForm()
        {
            InicializarComponentes();
            _ = CargarDatosAsync();
        }

        private void InicializarComponentes()
        {
            this.Text = "Nuevo Pedido";
            this.Size = new Size(600, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            lblCliente.Text = "Cliente:";
            lblCliente.Location = new Point(20, 20);
            lblCliente.AutoSize = true;

            cmbCliente.Location = new Point(20, 40);
            cmbCliente.Size = new Size(250, 25);
            cmbCliente.DropDownStyle = ComboBoxStyle.DropDownList;

            lblProducto.Text = "Producto:";
            lblProducto.Location = new Point(20, 80);
            lblProducto.AutoSize = true;

            cmbProducto.Location = new Point(20, 100);
            cmbProducto.Size = new Size(250, 25);
            cmbProducto.DropDownStyle = ComboBoxStyle.DropDownList;

            lblCantidad.Text = "Cantidad:";
            lblCantidad.Location = new Point(290, 80);
            lblCantidad.AutoSize = true;

            txtCantidad.Location = new Point(290, 100);
            txtCantidad.Size = new Size(80, 25);
            txtCantidad.Text = "1";

            btnAgregarProducto.Text = "Agregar";
            btnAgregarProducto.Location = new Point(390, 100);
            btnAgregarProducto.Size = new Size(80, 25);
            btnAgregarProducto.BackColor = Color.OrangeRed;
            btnAgregarProducto.ForeColor = Color.White;
            btnAgregarProducto.FlatStyle = FlatStyle.Flat;
            btnAgregarProducto.Click += BtnAgregarProducto_Click;

            dgvDetalle.Location = new Point(20, 140);
            dgvDetalle.Size = new Size(545, 200);
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.ReadOnly = true;
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.BackgroundColor = Color.White;

            lblTotal.Text = "Total: L. 0.00";
            lblTotal.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTotal.ForeColor = Color.OrangeRed;
            lblTotal.Location = new Point(20, 350);
            lblTotal.AutoSize = true;

            lblObservaciones.Text = "Observaciones:";
            lblObservaciones.Location = new Point(20, 385);
            lblObservaciones.AutoSize = true;

            txtObservaciones.Location = new Point(20, 405);
            txtObservaciones.Size = new Size(545, 50);
            txtObservaciones.Multiline = true;

            btnCrearPedido.Text = "✔ Crear Pedido";
            btnCrearPedido.Location = new Point(20, 465);
            btnCrearPedido.Size = new Size(150, 35);
            btnCrearPedido.BackColor = Color.DarkGreen;
            btnCrearPedido.ForeColor = Color.White;
            btnCrearPedido.FlatStyle = FlatStyle.Flat;
            btnCrearPedido.Click += BtnCrearPedido_Click;

            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(180, 465);
            btnCancelar.Size = new Size(100, 35);
            btnCancelar.BackColor = Color.Gray;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[]
            {
                lblCliente, cmbCliente, lblProducto, cmbProducto,
                lblCantidad, txtCantidad, btnAgregarProducto,
                dgvDetalle, lblTotal, lblObservaciones, txtObservaciones,
                btnCrearPedido, btnCancelar
            });
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                _clientes = await _api.GetAsync<List<dynamic>>("Cliente") ?? new();
                _productos = await _api.GetAsync<List<dynamic>>("Producto") ?? new();

                cmbCliente.DataSource = _clientes.Select(c => new
                {
                    Id = (int)c.id,
                    Nombre = $"{c.nombre} {c.apellido}"
                }).ToList();
                cmbCliente.DisplayMember = "Nombre";
                cmbCliente.ValueMember = "Id";

                cmbProducto.DataSource = _productos.Select(p => new
                {
                    Id = (int)p.id,
                    Nombre = $"{p.nombre} - L.{p.precio}"
                }).ToList();
                cmbProducto.DisplayMember = "Nombre";
                cmbProducto.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAgregarProducto_Click(object? sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue == null) return;

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad valida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var productoId = (int)cmbProducto.SelectedValue;
            var producto = _productos.FirstOrDefault(p => (int)p.id == productoId);
            if (producto == null) return;

            var precio = (decimal)producto.precio;
            var nombre = (string)producto.nombre;

            _detalles.Add((productoId, nombre, cantidad, precio));
            _total += cantidad * precio;

            ActualizarGrilllaDetalle();
        }

        private void ActualizarGrilllaDetalle()
        {
            dgvDetalle.DataSource = null;
            dgvDetalle.DataSource = _detalles.Select(d => new
            {
                Producto = d.Nombre,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.Precio,
                Subtotal = d.Cantidad * d.Precio
            }).ToList();

            lblTotal.Text = $"Total: L. {_total:F2}";
        }

        private async void BtnCrearPedido_Click(object? sender, EventArgs e)
        {
            if (cmbCliente.SelectedValue == null || _detalles.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente y agregue al menos un producto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pedido = new
            {
                clienteId = (int)cmbCliente.SelectedValue,
                estado = "Pendiente",
                observaciones = txtObservaciones.Text.Trim(),
                detalles = _detalles.Select(d => new
                {
                    productoId = d.ProductoId,
                    cantidad = d.Cantidad,
                    precioUnitario = d.Precio
                }).ToList()
            };

            try
            {
                await _api.PostAsync<dynamic>("Pedido", pedido);
                MessageBox.Show("Pedido creado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear pedido: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
