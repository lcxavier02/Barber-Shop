using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace BarberShopApp
{
    public partial class MainForm : Form
    {
        private List<Barbero> barberos = new List<Barbero>();
        private Queue<Cliente> colaClientes = new Queue<Cliente>();
        private System.Windows.Forms.Timer timer1;

        public MainForm()
        {
            InitializeComponent();
            dgvBarberosClientes.AutoGenerateColumns = false;
            dgvBarberosClientes.DataSource = new BindingSource(new List<object>(), null);
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void btnAgregarBarbero_Click(object sender, EventArgs e)
        {
            int id = barberos.Count + 1;
            Barbero barbero = new Barbero(id);
            barberos.Add(barbero);
            dgvBarberosClientes.DataSource = new BindingSource(barberos, null);
        }

        private void btnEliminarBarbero_Click(object sender, EventArgs e)
        {
            int rowIndex = dgvBarberosClientes.CurrentCell.RowIndex;
            barberos.RemoveAt(rowIndex);
            dgvBarberosClientes.DataSource = new BindingSource(barberos, null);
        }

        private void btnAgregarCliente_Click(object sender, EventArgs e)
        {
            int id = colaClientes.Count + 1;
            Cliente cliente = new Cliente(id);
            colaClientes.Enqueue(cliente);
            dgvBarberosClientes.DataSource = new BindingSource(colaClientes, null);
        }

        private void btnEliminarCliente_Click(object sender, EventArgs e)
        {
            int rowIndex = dgvBarberosClientes.CurrentCell.RowIndex;
            colaClientes.Dequeue();
            dgvBarberosClientes.DataSource = new BindingSource(colaClientes, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (colaClientes.Count > 0)
            {
                Barbero barberoDisponible = null;
                foreach (Barbero barbero in barberos)
                {
                    if (barbero.Estado == EstadoBarbero.Disponible)
                    {
                        barberoDisponible = barbero;
                        break;
                    }
                }

                if (barberoDisponible != null)
                {
                    Cliente proximoCliente = colaClientes.Dequeue();
                    barberoDisponible.Atender(proximoCliente);
                    dgvBarberosClientes.DataSource = new BindingSource(barberos, null);
                    dgvBarberosClientes.DataSource = new BindingSource(colaClientes, null);
                }
            }
        }

        private DataGridView dgvBarberosClientes;

        private void InitializeComponent()
        {
            this.dgvBarberosClientes = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBarberosClientes)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBarberosClientes
            // 
            this.dgvBarberosClientes.AccessibleName = "dgvBarberosClientes";
            this.dgvBarberosClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBarberosClientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Estado});
            this.dgvBarberosClientes.Location = new System.Drawing.Point(84, 144);
            this.dgvBarberosClientes.Name = "dgvBarberosClientes";
            this.dgvBarberosClientes.RowHeadersWidth = 51;
            this.dgvBarberosClientes.RowTemplate.Height = 29;
            this.dgvBarberosClientes.Size = new System.Drawing.Size(300, 188);
            this.dgvBarberosClientes.TabIndex = 0;
            this.dgvBarberosClientes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.Width = 125;
            // 
            // Estado
            // 
            this.Estado.HeaderText = "Estado";
            this.Estado.MinimumWidth = 6;
            this.Estado.Name = "Estado";
            this.Estado.Width = 125;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(903, 628);
            this.Controls.Add(this.dgvBarberosClientes);
            this.Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBarberosClientes)).EndInit();
            this.ResumeLayout(false);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Estado;

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }

    enum EstadoBarbero
    {
        Disponible,
        CortandoCabello,
        Esperando
    }

    class Barbero
    {
        public int Id { get; private set; }
        public EstadoBarbero Estado { get; private set; }
        private Cliente clienteActual;
        private Thread hilo;

        public Barbero(int id)
        {
            Id = id;
            Estado = EstadoBarbero.Disponible;
        }

        public void Atender(Cliente cliente)
        {
            clienteActual = cliente;
            Estado = EstadoBarbero.CortandoCabello;

            hilo = new Thread(() =>
            {
                Random rnd = new Random();
                int tiempoCorte = rnd.Next(5, 10);
                Thread.Sleep(tiempoCorte * 1000);
                Estado = EstadoBarbero.Disponible;
                clienteActual = null;
            });
            hilo.Start();
        }

        public override string ToString()
        {
            string estadoStr;
            switch (Estado)
            {
                case EstadoBarbero.Disponible:
                    estadoStr = "Disponible";
                    break;
                case EstadoBarbero.CortandoCabello:
                    estadoStr = "Cortando cabello de cliente " + clienteActual.Id;
                    break;
                case EstadoBarbero.Esperando:
                    estadoStr = "Esperando a que termine el corte";
                    break;
                default:
                    estadoStr = "";
                    break;
            }
            return $"Barbero {Id}: {estadoStr}";
        }
    }

    class Cliente
    {
        public int Id { get; private set; }

        public Cliente(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"Cliente {Id}";
        }
    }
}