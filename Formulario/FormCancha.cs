using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CanchaFuentes.Formulario
{
    public partial class FormCancha : Form
    {
        private decimal precioDia;
        private decimal precioNoche;
        private string[] reservasRealizadas;

        public FormCancha(decimal precioDia, decimal precioNoche)
        {
            InitializeComponent();

            this.precioDia = precioDia;
            this.precioNoche = precioNoche;

            lblCosto.Text = $"Precio día: Q{precioDia} / Precio noche: Q{precioNoche}";

            CargarHorarios();
            CargarTiposHorario();
            CargarReservasRealizadas();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormEstadisticas formEstadistica = new FormEstadisticas(precioDia, precioNoche);
            formEstadistica.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormConfiguracion formConfiguracion = new FormConfiguracion();
            formConfiguracion.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRReserva_Click(object sender, EventArgs e)
        {
            string nombreCliente = txtNCliente.Text;
            string telefono = txtTelefono.Text;
            string tipoHorario = comboBoxTHorario.SelectedItem.ToString();
            string horario = comboBoxHorario.SelectedItem.ToString();
            string diaReserva = dateTimePickerDiaReserva.Value.ToShortDateString();

            if (!ValidarHorarioParaTipo(tipoHorario, horario))
            {
                MessageBox.Show("El horario seleccionado no es válido para el tipo de horario. Por favor, selecciona un horario correcto.", "Error de Horario", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ReservaExistenteEnMismoHorario(diaReserva, horario))
            {
                MessageBox.Show("Ya existe una reserva para el mismo horario en el mismo día. Por favor, selecciona otro horario o día.", "Error de Reserva", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal costoReserva = CalcularCostoReserva(tipoHorario);

            lblCosto.Text = $"Costo de la reserva: Q{costoReserva}";

            GuardarReserva(nombreCliente, telefono, tipoHorario, horario, diaReserva, costoReserva);

            MessageBox.Show("Reserva realizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private bool ValidarHorarioParaTipo(string tipoHorario, string horario)
        {
            if (tipoHorario == "Dia")
            {
                if (!(horario == "8:00 AM - 9:00 AM" || horario == "9:00 AM - 10:00 AM" || horario == "10:00 AM - 11:00 AM" || horario == "11:00 AM - 12:00 PM" ||
                      horario == "12:00 PM - 1:00 PM" || horario == "1:00 PM - 2:00 PM" || horario == "2:00 PM - 3:00 PM" || horario == "3:00 PM - 4:00 PM" ||
                      horario == "4:00 PM - 5:00 PM"))
                {
                    return false;
                }
            }
            else if (tipoHorario == "Noche")
            {
                if (!(horario == "5:00 PM - 6:00 PM" || horario == "6:00 PM - 7:00 PM" || horario == "7:00 PM - 8:00 PM" || horario == "8:00 PM - 9:00 PM" ||
                      horario == "9:00 PM - 10:00 PM" || horario == "10:00 PM - 11:00 PM"))
                {
                    return false;
                }
            }
            return true;
        }

        private bool ReservaExistenteEnMismoHorario(string diaReserva, string horario)
        {
            return reservasRealizadas.Any(reserva =>
            {
                string[] datosReserva = reserva.Split(',');
                string diaReservaExistente = datosReserva[4];
                string horarioReservaExistente = datosReserva[3];

                return diaReservaExistente == diaReserva && horarioReservaExistente == horario;
            });
        }

        private decimal CalcularCostoReserva(string tipoHorario)
        {
            if (tipoHorario == "Dia")
            {
                return precioDia;
            }
            else
            {
                return precioNoche;
            }
        }

        private void GuardarReserva(string nombreCliente, string telefono, string tipoHorario, string horario, string diaReserva, decimal costoReserva)
        {
            string datosReserva = $"{nombreCliente},{telefono},{tipoHorario},{horario},{diaReserva},{costoReserva}";
            string rutaArchivo = "Reservas.txt";

            using (StreamWriter sw = File.AppendText(rutaArchivo))
            {
                sw.WriteLine(datosReserva);
            }

            string rutaArchivoReservasRealizadas = "ReservasRealizadas.txt";

            using (StreamWriter sw = File.AppendText(rutaArchivoReservasRealizadas))
            {
                sw.WriteLine(datosReserva);
            }
        }

        private void CargarHorarios()
        {
            comboBoxHorario.Items.Clear();

            comboBoxHorario.Items.Add("8:00 AM - 9:00 AM");
            comboBoxHorario.Items.Add("9:00 AM - 10:00 AM");
            comboBoxHorario.Items.Add("10:00 AM - 11:00 AM");
            comboBoxHorario.Items.Add("11:00 AM - 12:00 PM");
            comboBoxHorario.Items.Add("12:00 PM - 1:00 PM");
            comboBoxHorario.Items.Add("1:00 PM - 2:00 PM");
            comboBoxHorario.Items.Add("2:00 PM - 3:00 PM");
            comboBoxHorario.Items.Add("3:00 PM - 4:00 PM");
            comboBoxHorario.Items.Add("4:00 PM - 5:00 PM");
            comboBoxHorario.Items.Add("5:00 PM - 6:00 PM");
            comboBoxHorario.Items.Add("6:00 PM - 7:00 PM");
            comboBoxHorario.Items.Add("7:00 PM - 8:00 PM");
            comboBoxHorario.Items.Add("8:00 PM - 9:00 PM");
            comboBoxHorario.Items.Add("9:00 PM - 10:00 PM");
            comboBoxHorario.Items.Add("10:00 PM - 11:00 PM");

            comboBoxHorario.SelectedIndex = 0;
        }

        private void CargarTiposHorario()
        {
            comboBoxTHorario.Items.Add("Dia");
            comboBoxTHorario.Items.Add("Noche");
            comboBoxTHorario.SelectedIndex = 0;
        }

        private void CargarReservasRealizadas()
        {
            string rutaArchivoReservasRealizadas = "ReservasRealizadas.txt";

            if (File.Exists(rutaArchivoReservasRealizadas))
            {
                reservasRealizadas = File.ReadAllLines(rutaArchivoReservasRealizadas);
            }
            else
            {
                reservasRealizadas = new string[0];
            }
        }
    }
}
