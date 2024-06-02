using System;
using System.IO;
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
   

        private void rjButton1_Click(object sender, EventArgs e)
        {
            FormCancha formCancha = new FormCancha(precioDia, precioNoche);
            formCancha.Show();
            this.Hide();
        }

        private void FormEstadisticas_Load(object sender, EventArgs e)
        {

        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            string diaSeleccionado = dateTimePickerDatosDia.Value.ToShortDateString();

            dataGridViewDatos.Rows.Clear();

            string rutaArchivoReservas = "ReservasRealizadas.txt";
            if (File.Exists(rutaArchivoReservas))
            {
                string[] lineasReservas = File.ReadAllLines(rutaArchivoReservas);

                foreach (string linea in lineasReservas)
                {
                    string[] datosReserva = linea.Split(',');

                    if (datosReserva.Length >= 5 && datosReserva[4] == diaSeleccionado)
                    {
                        string tipoHorario = datosReserva[2];
                        string horario = datosReserva[3];
                        decimal costoReserva = decimal.Parse(datosReserva[5]);

                        dataGridViewDatos.Rows.Add(datosReserva[0], datosReserva[1], tipoHorario, horario, costoReserva);
                    }
                }
            }
        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio = dtp1.Value.Date;
            DateTime fechaFin = dtp2.Value.Date;

            if (fechaInicio <= fechaFin)
            {
                decimal costoTotal = 0;

                string rutaArchivoReservas = "ReservasRealizadas.txt";
                if (File.Exists(rutaArchivoReservas))
                {
                    string[] lineasReservas = File.ReadAllLines(rutaArchivoReservas);

                    foreach (string linea in lineasReservas)
                    {
                        string[] datosReserva = linea.Split(',');

                        DateTime fechaReserva = DateTime.Parse(datosReserva[4]);

                        if (fechaReserva >= fechaInicio && fechaReserva <= fechaFin)
                        {
                            decimal costoReserva = decimal.Parse(datosReserva[5]);
                            costoTotal += costoReserva;
                        }
                    }

                    MessageBox.Show($"El costo total desde {fechaInicio.ToShortDateString()} hasta {fechaFin.ToShortDateString()} es de Q{costoTotal}", "Costo Total", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No hay reservas para calcular el costo total en el rango de fechas seleccionado.", "Sin Reservas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("La fecha de inicio debe ser anterior o igual a la fecha de fin.", "Error de Fechas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rjButton4_Click(object sender, EventArgs e)
        {
            string diaSeleccionado = dateTimePickerDatosDia.Value.ToShortDateString();

            decimal costoTotal = 0;

            string rutaArchivoReservas = "ReservasRealizadas.txt";
            if (File.Exists(rutaArchivoReservas))
            {
                string[] lineasReservas = File.ReadAllLines(rutaArchivoReservas);

                foreach (string linea in lineasReservas)
                {
                    string[] datosReserva = linea.Split(',');

                    if (datosReserva.Length >= 5 && datosReserva[4] == diaSeleccionado)
                    {
                        decimal costoReserva = decimal.Parse(datosReserva[5]);
                        costoTotal += costoReserva;
                    }
                }

                MessageBox.Show($"El costo total de las reservas del día es de Q{costoTotal}", "Costo Total del Día", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No hay reservas para calcular el costo total del día.", "Sin Reservas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

