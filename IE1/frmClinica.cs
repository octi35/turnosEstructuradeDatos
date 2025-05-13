
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace IE1
{
    // las descripciones sin igual para los otros 
    public partial class frmClinica : Form
    {
        
        public frmClinica()
        {
            InitializeComponent(); 
        }

        
        public void llamaPaciente(Paciente llamado)
        {
            // Muestra el nombre completo del paciente en formato "APELLIDO, Nombre"
            lblPaciente.Text = llamado.apellido.ToUpper() + ", " + llamado.nombre;

            // Muestra el DNI del paciente
            lblDNI.Text = llamado.dni;
        }

        
        public void mostrarConsultario()
        {
            Random rnd = new Random(); // Instancia de generador de números aleatorios
            lblConsultorio.Text = "CLI" + rnd.Next(1, 5).ToString("000#");
        }

       
        public void mostrarProximos(List<string> lista)
        {
            lstProximos.Items.Clear(); // Limpia la lista actual

            if (lista.Count == 0)
            {
                // Si no hay pacientes, muestra un mensaje en la lista
                lstProximos.Items.Add("No hay más pacientes.");
                return;
            }

            // Agrega cada paciente de la lista al ListBox
            foreach (string item in lista)
            {
                lstProximos.Items.Add(item);
            }
        }

        private void frmClinica_Load(object sender, EventArgs e)
        {

        }
    }


}



    

