using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzeriaPos.WinForms.Services;
using PizzeriaPos.WinForms.Models;

namespace PizzeriaPos.WinForms.Forms
{
    // Formulario de gestion de Clientes (CRUD)
    public partial class ClientesForm : Form
    {
        private DataGridView dgvClientes = new();
        private TextBox txtNombre = new();
        private TextBox txtApellido = new();
        private TextBox txtTelefono = new();
        private TextBox txtEmail = new();
        private Button btnNuevo = new();
        private Button btnGuardar = new();
        private Button btnEliminar = new();
        private Button btnCargar = new();
        private Label lblNombre = new();
        private Label lblApellido = new();
        private Label lblTelefono = new();
        private Label lblEmail = new();
        private Panel panelForm = new();
        private readonly ApiService _api = new();
        private int _idSeleccionado = 0;

        public ClientesForm()
        {
            InicializarComponentes();
            _ = CargarClientesAsync();
        }

        private void InicializarComponentes()
        {
            this.BackColor = Color.White;

            panelForm.Location = new Point(10, 10);
            panelForm.Size = new Size(300, 320);
            panelForm.BorderStyle = BorderStyle.FixedSingle;
            panelForm.BackColor = Color.WhiteSmoke;

            lblNombre.Text = "Nombre:";
            lblNombre.Location = new Point(10, 15);
            lblNombre.AutoSize = true;
            txtNombre.Location = new Point(10, 35);
            txtNombre.Size = new Size(270, 25);

            lblApellido.Text = "Apellido:";
            lblApellido.Location = new Point(10, 70);
            lblApellido.AutoSize = true;
            txtApellido.Location = new Point(10, 90);
            txtApellido.Size = new Size(270, 25);

            lblTelefono.Text = "Telefono:";
            lblTelefono.Location = new Point(10, 125);
            lblTelefono.AutoSize = true;
            txtTelefono.Location = new Point(10, 145);
            txtTelefono.Size = new Size(270, 25);

            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(10, 180);
            lblEmail.AutoSize = true;
            txtEmail.Location = new Point(10, 200);
            txtEmail.Size = new Size(270, 25);

            btnNuevo.Text = "Nuevo";
            btnNuevo.Location = new Point(10, 245);
            btnNuevo.Size = new Size(80, 30);
            btnNuevo.BackColor = Color.FromArgb(30, 100, 200);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Click += (s, e) => LimpiarFormulario();

            btnGuardar.Text = "Guardar";
            btnGuardar.Location = new Point(100, 245);
            btnGuardar.Size = new Size(80, 30);
            btnGuardar.BackColor = Color.FromArgb(40, 167, 69);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Click += BtnGuardar_Click;

            btnEliminar.Text = "Eliminar";
            btnEliminar.Location = new Point(190, 245);
            btnEliminar.Size = new Size(80, 30);
            btnEliminar.BackColor = Color.OrangeRed;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Click += BtnEliminar_Click;

            panelForm.Controls.AddRange(new Control[]
            {
                lblNombre, txtNombre, lblApellido, txtApellido,
                lblTelefono, txtTelefono, lblEmail, txtEmail,
                btnNuevo, btnGuardar, btnEliminar
            });

            btnCargar.Text = "🔄 Actualizar lista";
            btnCargar.Location = new Point(320, 10);
            btnCargar.Size = new Size(150, 30);
            btnCargar.BackColor = Color.OrangeRed;
            btnCargar.ForeColor = Color.White;
            btnCargar.FlatStyle = FlatStyle.Flat;
            btnCargar.Click += async (s, e) => await CargarClientesAsync();

            dgvClientes.Location = new Point(320, 50);
            dgvClientes.Size = new Size(530, 380);
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.ReadOnly = true;
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.BackgroundColor = Color.White;
            dgvClientes.CellClick += DgvClientes_CellClick;

            this.Controls.AddRange(new Control[] { panelForm, btnCargar, dgvClientes });
        }

        private async Task CargarClientesAsync()
        {
            try
            {
                var clientes = await _api.GetAsync<List<ClienteModel>>("Cliente");
                dgvClientes.DataSource = null;

                if (clientes != null && clientes.Count > 0)
                {
                    var tabla = clientes.Select(c => new
                    {
                        c.Id,
                        c.Nombre,
                        Apellido = c.Apellido ?? "",
                        Telefono = c.Telefono ?? "",
                        Email = c.Email ?? ""
                    }).ToList();

                    dgvClientes.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DgvClientes_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var id = (int)dgvClientes.Rows[e.RowIndex].Cells["Id"].Value;
            _idSeleccionado = id;

            try
            {
                var cliente = await _api.GetAsync<ClienteModel>($"Cliente/{id}");
                if (cliente != null)
                {
                    txtNombre.Text = cliente.Nombre;
                    txtApellido.Text = cliente.Apellido ?? "";
                    txtTelefono.Text = cliente.Telefono ?? "";
                    txtEmail.Text = cliente.Email ?? "";
                }
            }
            catch { }
        }

        private async void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = new
            {
                nombre = txtNombre.Text.Trim(),
                apellido = txtApellido.Text.Trim(),
                telefono = txtTelefono.Text.Trim(),
                email = txtEmail.Text.Trim()
            };

            try
            {
                if (_idSeleccionado == 0)
                    await _api.PostAsync<dynamic>("Cliente", dto);
                else
                    await _api.PutAsync<dynamic>($"Cliente/{_idSeleccionado}", dto);

                MessageBox.Show("Cliente guardado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                await CargarClientesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un cliente primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Eliminar este cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                await _api.DeleteAsync($"Cliente/{_idSeleccionado}");
                MessageBox.Show("Cliente eliminado.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                await CargarClientesAsync();
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
            txtApellido.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
        }
    }
}