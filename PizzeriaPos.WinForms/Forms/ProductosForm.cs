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
    // Formulario de gestion de Productos (CRUD)
    public partial class ProductosForm : Form
    {
        private DataGridView dgvProductos = new();
        private TextBox txtNombre = new();
        private TextBox txtDescripcion = new();
        private TextBox txtPrecio = new();
        private TextBox txtCategoria = new();
        private CheckBox chkDisponible = new();
        private Button btnNuevo = new();
        private Button btnGuardar = new();
        private Button btnEliminar = new();
        private Button btnCargar = new();
        private Label lblNombre = new();
        private Label lblDescripcion = new();
        private Label lblPrecio = new();
        private Label lblCategoria = new();
        private Panel panelForm = new();
        private readonly ApiService _api = new();
        private int _idSeleccionado = 0;

        public ProductosForm()
        {
            InicializarComponentes();
            _ = CargarProductosAsync();
        }

        private void InicializarComponentes()
        {
            this.BackColor = Color.White;

            // Panel del formulario de datos
            panelForm.Location = new Point(10, 10);
            panelForm.Size = new Size(300, 350);
            panelForm.BorderStyle = BorderStyle.FixedSingle;
            panelForm.BackColor = Color.WhiteSmoke;

            // Campos del formulario
            lblNombre.Text = "Nombre:";
            lblNombre.Location = new Point(10, 15);
            lblNombre.AutoSize = true;
            txtNombre.Location = new Point(10, 35);
            txtNombre.Size = new Size(270, 25);

            lblDescripcion.Text = "Descripcion:";
            lblDescripcion.Location = new Point(10, 70);
            lblDescripcion.AutoSize = true;
            txtDescripcion.Location = new Point(10, 90);
            txtDescripcion.Size = new Size(270, 25);

            lblPrecio.Text = "Precio:";
            lblPrecio.Location = new Point(10, 125);
            lblPrecio.AutoSize = true;
            txtPrecio.Location = new Point(10, 145);
            txtPrecio.Size = new Size(270, 25);

            lblCategoria.Text = "Categoria:";
            lblCategoria.Location = new Point(10, 180);
            lblCategoria.AutoSize = true;
            txtCategoria.Location = new Point(10, 200);
            txtCategoria.Size = new Size(270, 25);

            chkDisponible.Text = "Disponible";
            chkDisponible.Location = new Point(10, 235);
            chkDisponible.Checked = true;

            // Botones
            btnNuevo.Text = "Nuevo";
            btnNuevo.Location = new Point(10, 270);
            btnNuevo.Size = new Size(80, 30);
            btnNuevo.BackColor = Color.FromArgb(30, 100, 200);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Click += (s, e) => LimpiarFormulario();

            btnGuardar.Text = "Guardar";
            btnGuardar.Location = new Point(100, 270);
            btnGuardar.Size = new Size(80, 30);
            btnGuardar.BackColor = Color.FromArgb(40, 167, 69);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Click += BtnGuardar_Click;

            btnEliminar.Text = "Eliminar";
            btnEliminar.Location = new Point(190, 270);
            btnEliminar.Size = new Size(80, 30);
            btnEliminar.BackColor = Color.OrangeRed;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Click += BtnEliminar_Click;

            panelForm.Controls.AddRange(new Control[]
            {
                lblNombre, txtNombre, lblDescripcion, txtDescripcion,
                lblPrecio, txtPrecio, lblCategoria, txtCategoria,
                chkDisponible, btnNuevo, btnGuardar, btnEliminar
            });

            // Boton cargar
            btnCargar.Text = "🔄 Actualizar lista";
            btnCargar.Location = new Point(320, 10);
            btnCargar.Size = new Size(150, 30);
            btnCargar.BackColor = Color.OrangeRed;
            btnCargar.ForeColor = Color.White;
            btnCargar.FlatStyle = FlatStyle.Flat;
            btnCargar.Click += async (s, e) => await CargarProductosAsync();

            // DataGridView
            dgvProductos.Location = new Point(320, 50);
            dgvProductos.Size = new Size(530, 380);
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.ReadOnly = true;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.BackgroundColor = Color.White;
            dgvProductos.CellClick += DgvProductos_CellClick;

            this.Controls.AddRange(new Control[] { panelForm, btnCargar, dgvProductos });
        }

        // Carga los productos desde la API y los muestra en el grid
        private async Task CargarProductosAsync()
        {
            try
            {
                var productos = await _api.GetAsync<List<ProductoModel>>("Producto");
                dgvProductos.DataSource = null;

                if (productos != null && productos.Count > 0)
                {
                    var tabla = productos.Select(p => new
                    {
                        p.Id,
                        p.Nombre,
                        p.Precio,
                        p.Categoria,
                        p.Disponible
                    }).ToList();

                    dgvProductos.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Al hacer clic en una fila, carga los datos en el formulario
        private async void DgvProductos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var id = (int)dgvProductos.Rows[e.RowIndex].Cells["Id"].Value;
            _idSeleccionado = id;

            try
            {
                var producto = await _api.GetAsync<ProductoModel>($"Producto/{id}");
                if (producto != null)
                {
                    txtNombre.Text = producto.Nombre;
                    txtDescripcion.Text = producto.Descripcion ?? "";
                    txtPrecio.Text = producto.Precio.ToString();
                    txtCategoria.Text = producto.Categoria;
                    chkDisponible.Checked = producto.Disponible;
                }
            }
            catch { }
        }

        // Guarda (crea o actualiza) un producto
        private async void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Nombre y precio son obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio debe ser un numero valido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = new
            {
                nombre = txtNombre.Text.Trim(),
                descripcion = txtDescripcion.Text.Trim(),
                precio = precio,
                categoria = txtCategoria.Text.Trim(),
                disponible = chkDisponible.Checked
            };

            try
            {
                if (_idSeleccionado == 0)
                    await _api.PostAsync<dynamic>("Producto", dto);
                else
                    await _api.PutAsync<dynamic>($"Producto/{_idSeleccionado}", dto);

                MessageBox.Show("Producto guardado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                await CargarProductosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Elimina el producto seleccionado
        private async void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un producto primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                await _api.DeleteAsync($"Producto/{_idSeleccionado}");
                MessageBox.Show("Producto eliminado.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                await CargarProductosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _idSeleccionado = 0;
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtCategoria.Clear();
            chkDisponible.Checked = true;
        }
    }
}
