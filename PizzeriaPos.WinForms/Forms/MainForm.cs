using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PizzeriaPos.WinForms.Forms
{
    // Formulario principal con menu de navegacion
    public partial class MainForm : Form
    {
        private TabControl tabControl = new();
        private TabPage tabProductos = new();
        private TabPage tabClientes = new();
        private TabPage tabPedidos = new();
        private Button btnCerrarSesion = new();
        private Label lblBienvenida = new();

        public MainForm()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Pizzeria POS - Sistema de Punto de Venta";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Label bienvenida
            lblBienvenida.Text = "🍕 Pizzeria POS";
            lblBienvenida.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblBienvenida.ForeColor = Color.OrangeRed;
            lblBienvenida.Location = new Point(20, 10);
            lblBienvenida.AutoSize = true;

            // Boton cerrar sesion
            btnCerrarSesion.Text = "Cerrar Sesion";
            btnCerrarSesion.Location = new Point(770, 10);
            btnCerrarSesion.Size = new Size(110, 30);
            btnCerrarSesion.BackColor = Color.OrangeRed;
            btnCerrarSesion.ForeColor = Color.White;
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;

            // TabControl con las secciones
            tabControl.Location = new Point(10, 50);
            tabControl.Size = new Size(865, 500);
            tabControl.Font = new Font("Segoe UI", 10);

            tabProductos.Text = "  Productos  ";
            tabClientes.Text = "  Clientes  ";
            tabPedidos.Text = "  Pedidos  ";

            tabControl.TabPages.Add(tabProductos);
            tabControl.TabPages.Add(tabClientes);
            tabControl.TabPages.Add(tabPedidos);

            // Agregar los formularios de cada seccion dentro de cada tab
            var productosForm = new ProductosForm { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            tabProductos.Controls.Add(productosForm);
            productosForm.Show();

            var clientesForm = new ClientesForm { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            tabClientes.Controls.Add(clientesForm);
            clientesForm.Show();

            var pedidosForm = new PedidosForm { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            tabPedidos.Controls.Add(pedidosForm);
            pedidosForm.Show();

            this.Controls.AddRange(new Control[] { lblBienvenida, btnCerrarSesion, tabControl });
        }

        // Cerrar sesion: volver al login
        private void BtnCerrarSesion_Click(object? sender, EventArgs e)
        {
            var login = new LoginForm();
            login.Show();
            this.Close();
        }
    }
}