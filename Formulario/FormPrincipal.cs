using CanchaFuentes.Formulario;
using System;
using System.Windows.Forms;

namespace CanchaFuentes
{
    public partial class FormPrincipal : Form
    {
        private decimal precioDia;
        private decimal precioNoche;
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FormCancha formCancha = new FormCancha(precioDia, precioNoche);
            formCancha.Show();
            this.Hide();
        }
    }
}
