using System;
using System.IO;
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

            // Guardar los precios recibidos como parámetros
            this.precioDia = precioDia;
            this.precioNoche = precioNoche;

            // Mostrar los precios en el label correspondiente
            lblCosto.Text = $"Precio día: Q{precioDia} / Precio noche: Q{precioNoche}";

            // Cargar los horarios y tipos de horario
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
            // Obtener los datos ingresados por el usuario
            string nombreCliente = txtNCliente.Text;
            string telefono = txtTelefono.Text;
            string tipoHorario = comboBoxTHorario.SelectedItem.ToString();
            string horario = comboBoxHorario.SelectedItem.ToString();
            string diaReserva = dateTimePickerDiaReserva.Value.ToShortDateString();

            // Validar si el horario seleccionado es válido para el tipo de horario
            if (!ValidarHorarioParaTipo(tipoHorario, horario))
            {
                MessageBox.Show("El horario seleccionado no es válido para el tipo de horario. Por favor, selecciona un horario correcto.", "Error de Horario", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar si ya hay una reserva para el mismo horario en el mismo día
            if (ReservaExistenteEnMismoHorario(diaReserva, horario))
            {
                MessageBox.Show("Ya existe una reserva para el mismo horario en el mismo día. Por favor, selecciona otro horario o día.", "Error de Reserva", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calcular el costo de la reserva según el tipo de horario
            decimal costoReserva = CalcularCostoReserva(tipoHorario);

            // Mostrar el costo de la reserva en el label
            lblCosto.Text = $"Costo de la reserva: Q{costoReserva}";

            // Guardar los datos de la reserva en un archivo de texto
            GuardarReserva(nombreCliente, telefono, tipoHorario, horario, diaReserva, costoReserva);

            // Mostrar mensaje de reserva realizada con éxito
            MessageBox.Show("Reserva realizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidarHorarioParaTipo(string tipoHorario, string horario)
        {
            if (tipoHorario == "Dia")
            {
                // Validar horarios para el día (de 8:00 AM a 5:00 PM)
                if (!horario.StartsWith("8") && !horario.StartsWith("9") && !horario.StartsWith("10") && !horario.StartsWith("11") && !horario.StartsWith("12") &&
                    !horario.StartsWith("13") && !horario.StartsWith("14") && !horario.StartsWith("15") && !horario.StartsWith("16") && !horario.StartsWith("17"))
                {
                    return false;
                }
            }
            else if (tipoHorario == "Noche")
            {
                // Validar horarios para la noche (de 6:00 PM a 11:00 PM y de 12:00 AM a 7:00 AM)
                if (!horario.StartsWith("18") && !horario.StartsWith("19") && !horario.StartsWith("20") && !horario.StartsWith("21") && !horario.StartsWith("22") && !horario.StartsWith("23"))
                {
                    return false;
                }
            }
            return true;
        }

        private bool ReservaExistenteEnMismoHorario(string diaReserva, string horario)
        {
            foreach (string reserva in reservasRealizadas)
            {
                string[] datosReserva = reserva.Split(',');
                string diaReservaExistente = datosReserva[4];
                string horarioReservaExistente = datosReserva[3];

                if (diaReservaExistente == diaReserva && horarioReservaExistente == horario)
                {
                    return true;
                }
            }
            return false;
        }

        private decimal CalcularCostoReserva(string tipoHorario)
        {
            // Calcular el costo de la reserva según el tipo de horario seleccionado
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
            // Guardar los datos de la reserva en un archivo de texto (Reservas.txt)
            string datosReserva = $"{nombreCliente},{telefono},{tipoHorario},{horario},{diaReserva},{costoReserva}";
            string rutaArchivo = "Reservas.txt";

            using (StreamWriter sw = File.AppendText(rutaArchivo))
            {
                sw.WriteLine(datosReserva);
            }

            // Guardar los datos de la reserva en un archivo de texto (ReservasRealizadas.txt)
            string rutaArchivoReservasRealizadas = "ReservasRealizadas.txt";

            using (StreamWriter sw = File.AppendText(rutaArchivoReservasRealizadas))
            {
                sw.WriteLine(datosReserva);
            }
        }

        private void CargarHorarios()
        {
            // Limpiar los horarios antes de cargar nuevos
            comboBoxHorario.Items.Clear();

            // Agregar horarios
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

            comboBoxHorario.SelectedIndex = 0; // Seleccionar el primer horario por defecto
        }

        private void CargarTiposHorario()
        {
            comboBoxTHorario.Items.Add("Dia");
            comboBoxTHorario.Items.Add("Noche");
            comboBoxTHorario.SelectedIndex = 0; // Seleccionar el primer tipo de horario por defecto
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

