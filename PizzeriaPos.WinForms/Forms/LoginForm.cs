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
    // Formulario de Login y Registro de usuarios
    public partial class LoginForm : Form
    {
        private TextBox txtUsuario = new();
        private TextBox txtPassword = new();
        private TextBox txtNombreCompleto = new();
        private Button btnLogin = new();
        private Button btnRegistrar = new();
        private Label lblTitulo = new();
        private Label lblUsuario = new();
        private Label lblPassword = new();
        private Label lblNombreCompleto = new();
        private Panel panelRegistro = new();
        private readonly ApiService _api = new();

        public LoginForm()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            // Configuracion del formulario
            this.Text = "Pizzeria POS - Login";
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Titulo
            lblTitulo.Text = "🍕 Pizzeria POS";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.ForeColor = Color.OrangeRed;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(110, 20);

            // Label Usuario
            lblUsuario.Text = "Usuario:";
            lblUsuario.Font = new Font("Segoe UI", 10);
            lblUsuario.Location = new Point(40, 80);
            lblUsuario.AutoSize = true;

            // TextBox Usuario
            txtUsuario.Location = new Point(40, 100);
            txtUsuario.Size = new Size(300, 30);
            txtUsuario.Font = new Font("Segoe UI", 10);

            // Label Password
            lblPassword.Text = "Contraseña:";
            lblPassword.Font = new Font("Segoe UI", 10);
            lblPassword.Location = new Point(40, 140);
            lblPassword.AutoSize = true;

            // TextBox Password
            txtPassword.Location = new Point(40, 160);
            txtPassword.Size = new Size(300, 30);
            txtPassword.Font = new Font("Segoe UI", 10);
            txtPassword.PasswordChar = '*';

            // Boton Login
            btnLogin.Text = "Iniciar Sesion";
            btnLogin.Location = new Point(40, 210);
            btnLogin.Size = new Size(300, 40);
            btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLogin.BackColor = Color.OrangeRed;
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Click += BtnLogin_Click;

            // Boton Registrar
            btnRegistrar.Text = "¿No tienes cuenta? Registrate";
            btnRegistrar.Location = new Point(40, 260);
            btnRegistrar.Size = new Size(300, 35);
            btnRegistrar.Font = new Font("Segoe UI", 9);
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.BackColor = Color.WhiteSmoke;
            btnRegistrar.Click += BtnRegistrar_Click;

            // Panel Registro (oculto inicialmente)
            panelRegistro.Location = new Point(40, 305);
            panelRegistro.Size = new Size(300, 80);
            panelRegistro.Visible = false;

            lblNombreCompleto.Text = "Nombre Completo:";
            lblNombreCompleto.Font = new Font("Segoe UI", 10);
            lblNombreCompleto.AutoSize = true;
            lblNombreCompleto.Location = new Point(0, 0);

            txtNombreCompleto.Location = new Point(0, 22);
            txtNombreCompleto.Size = new Size(300, 30);
            txtNombreCompleto.Font = new Font("Segoe UI", 10);

            panelRegistro.Controls.Add(lblNombreCompleto);
            panelRegistro.Controls.Add(txtNombreCompleto);

            // Agregar controles al formulario
            this.Controls.AddRange(new Control[]
            {
                lblTitulo, lblUsuario, txtUsuario,
                lblPassword, txtPassword,
                btnLogin, btnRegistrar, panelRegistro
            });

            this.Icon = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "pizza.ico"));
        }

        // Accion del boton Login
        private async void BtnLogin_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnLogin.Text = "Iniciando sesion...";

                var resultado = await _api.PostAsync<dynamic>("Auth/login", new
                {
                    nombreUsuario = txtUsuario.Text.Trim(),
                    password = txtPassword.Text
                });

                if (resultado != null)
                {
                    // Guardar el token JWT para usarlo en toda la aplicacion
                    ApiService.Token = resultado.token.ToString();

                    // Abrir el formulario principal
                    var mainForm = new MainForm();
                    mainForm.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Credenciales invalidas. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Iniciar Sesion";
            }
        }

        // Accion del boton Registrar
        private async void BtnRegistrar_Click(object? sender, EventArgs e)
        {
            // Si el panel no esta visible, mostrarlo para ingresar nombre completo
            if (!panelRegistro.Visible)
            {
                panelRegistro.Visible = true;
                btnRegistrar.Text = "Confirmar Registro";
                this.Size = new Size(400, 530);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtNombreCompleto.Text))
            {
                MessageBox.Show("Complete todos los campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnRegistrar.Enabled = false;
                await _api.PostAsync<dynamic>("Auth/register", new
                {
                    nombreUsuario = txtUsuario.Text.Trim(),
                    nombreCompleto = txtNombreCompleto.Text.Trim(),
                    password = txtPassword.Text
                });

                MessageBox.Show("Usuario registrado exitosamente. Ahora puede iniciar sesion.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Ocultar panel de registro
                panelRegistro.Visible = false;
                btnRegistrar.Text = "¿No tienes cuenta? Registrate";
                this.Size = new Size(400, 450);
                txtNombreCompleto.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegistrar.Enabled = true;
            }
        }
    }
}
