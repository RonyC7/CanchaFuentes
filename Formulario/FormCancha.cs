using System;
using System.IO;
using System.Windows.Forms;

namespace CanchaFuentes.Formulario
{
    public partial class FormCancha : Form
    {
        private decimal precioDia;
        private decimal precioNoche;

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

            // Calcular el costo de la reserva según el tipo de horario
            decimal costoReserva = CalcularCostoReserva(tipoHorario);

            // Mostrar el costo de la reserva en el label
            lblCosto.Text = $"Costo de la reserva: Q{costoReserva}";

            // Guardar los datos de la reserva en un archivo de texto
            GuardarReserva(nombreCliente, telefono, tipoHorario, horario, diaReserva, costoReserva);
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
            // Agregar horarios de AM
            for (int hora = 8; hora <= 10; hora++)
            {
                comboBoxHorario.Items.Add($"{hora}:00 AM - {hora + 1}:00 AM");
            }

            // Agregar horario de AM para las 11:00
            comboBoxHorario.Items.Add("11:00 AM - 12:00 PM");

            // Agregar horarios de PM
            for (int hora = 12; hora <= 22; hora++)
            {
                comboBoxHorario.Items.Add($"{hora}:00 PM - {hora + 1}:00 PM");
            }

            comboBoxHorario.SelectedIndex = 0; // Seleccionar el primer horario por defecto
        }

        private void CargarTiposHorario()
        {
            comboBoxTHorario.Items.Add("Dia");
            comboBoxTHorario.Items.Add("Noche");
            comboBoxTHorario.SelectedIndex = 0; // Seleccionar el primer tipo de horario por defecto
        }

        private void comboBoxTHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limpiar los horarios al cambiar la selección
            comboBoxHorario.Items.Clear();

            // Agregar horarios según la selección de Dia o Noche
            if (comboBoxTHorario.SelectedItem.ToString() == "Dia")
            {
                // Agregar horarios de AM para el día
                for (int hora = 8; hora <= 16; hora++)
                {
                    comboBoxHorario.Items.Add($"{hora}:00 AM - {hora + 1}:00 AM");
                }
            }
            else if (comboBoxTHorario.SelectedItem.ToString() == "Noche")
            {
                // Agregar horarios de PM para la noche
                for (int hora = 18; hora <= 22; hora++)
                {
                    comboBoxHorario.Items.Add($"{hora}:00 PM - {hora + 1}:00 PM");
                }
            }
        }
    }
}
