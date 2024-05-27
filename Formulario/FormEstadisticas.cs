using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanchaFuentes.Formulario
{
    public partial class FormEstadisticas : Form
    {
        private decimal precioDia;
        private decimal precioNoche;

        public FormEstadisticas(decimal precioDia, decimal precioNoche)
        {
            InitializeComponent();
            this.precioDia = precioDia;
            this.precioNoche = precioNoche;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormCancha formCancha = new FormCancha(precioDia, precioNoche);
            formCancha.Show();
            this.Hide();
        }
    }

}
