using System;
using System.IO;
using System.Windows.Forms;

namespace CanchaFuentes.Formulario
{
    public partial class FormConfiguracion : Form
    {
        private const string RutaArchivo = "Precios.txt";

        private PreciosCancha preciosCancha = new PreciosCancha();

        public FormConfiguracion()
        {
            InitializeComponent();
            CargarPrecios();
        }


        private void CargarPrecios()
        {
            try
            {
                if (File.Exists(RutaArchivo))
                {
                    using (StreamReader sr = new StreamReader(RutaArchivo))
                    {
                        string[] precios = sr.ReadLine().Split(',');
                        if (precios.Length == 2)
                        {
                            preciosCancha.PrecioDia = int.Parse(precios[0]);
                            preciosCancha.PrecioNoche = int.Parse(precios[1]);

                            lblDia.Text = $"Precio día: {preciosCancha.PrecioDia}";
                            lblNoche.Text = $"Precio noche: {preciosCancha.PrecioNoche}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los precios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarPrecios()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(RutaArchivo))
                {
                    sw.WriteLine($"{preciosCancha.PrecioDia},{preciosCancha.PrecioNoche}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los precios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            FormCancha formCancha = new FormCancha(preciosCancha.PrecioDia, preciosCancha.PrecioNoche);
            formCancha.Show();
            this.Hide();
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtDia.Text, out int nuevoPrecioDia) && int.TryParse(txtNoche.Text, out int nuevoPrecioNoche))
            {
                preciosCancha.PrecioDia = nuevoPrecioDia;
                preciosCancha.PrecioNoche = nuevoPrecioNoche;

                GuardarPrecios();

                MessageBox.Show("Los precios han sido cambiados.", "Cambios Guardados", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblDia.Text = $"Precio día: {preciosCancha.PrecioDia}";
                lblNoche.Text = $"Precio noche: {preciosCancha.PrecioNoche}";
            }
            else
            {
                MessageBox.Show("Por favor ingrese valores numéricos enteros para los precios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class PreciosCancha
    {
        public int PrecioDia { get; set; }
        public int PrecioNoche { get; set; }
    }
}
