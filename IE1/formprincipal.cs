using System.Drawing; 

namespace IE1
{
    
    public partial class formprincipal : Form
    {
        // Colas para cada especialidad médica
        Cola esperaClinicaMedica = new Cola();
        Cola esperaPediatria = new Cola();
        Cola esperaGuardia = new Cola();

        // Formularios secundarios para mostrar información específica
        frmClinica visorClinica = new frmClinica();
        frmGuardia visorGuardia = new frmGuardia();
        frmPediatría visorPediatria = new frmPediatría();

        // Constructor del formulario principal
        public formprincipal()
        {
            InitializeComponent(); // Inicializa los componentes gráficos del formulario
            visorClinica.Show();   // Muestra el formulario de clínica
            visorGuardia.Show();   // Muestra el formulario de guardia
            visorPediatria.Show(); // Muestra el formulario de pediatría
            restaurar();           // Restaura los datos desde los archivos de backup
        }

        // Evento que se ejecuta al presionar el botón Insertar
        private void btnInsertar_Click(object sender, EventArgs e)
        {
            if (!validarCamposPaciente()) return; // para ver  que los campos estén completos

            if (cmbEspecialidad.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una especialidad.");
                return;
            }

            string especialidad = cmbEspecialidad.SelectedItem.ToString();
            Paciente nuevo = crearObjPaciente(); // Crea un nuevo paciente

            // Inserta al paciente en la cola correspondiente según su especialidad
            switch (especialidad)
            {
                case "Clínica":
                    esperaClinicaMedica.Insertar(nuevo.dni, nuevo.nombre, nuevo.apellido);
                    esperaClinicaMedica.Listar(lstClinica);
                    backup();
                    break;
                case "Pediatría":
                    esperaPediatria.Insertar(nuevo.dni, nuevo.nombre, nuevo.apellido);
                    esperaPediatria.Listar(lstPediatria);
                    backup();
                    break;
                case "Guardia":
                    esperaGuardia.Insertar(nuevo.dni, nuevo.nombre, nuevo.apellido);
                    esperaGuardia.Listar(lstGuardia);
                    backup();
                    break;
                default:
                    MessageBox.Show("Especialidad no reconocida.");
                    return;
            }

            // Confirma al usuario la inserción y limpia los campos
            MessageBox.Show($"Paciente insertado en la cola de {especialidad}.");
            txtDNI.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
        }

        // Crea un nuevo objeto Paciente usando los valores ingresados en el formulario
        private Paciente crearObjPaciente()
        {
            return new Paciente(txtDNI.Text, txtNombre.Text, txtApellido.Text)
            {
                vigente = true // Marca al paciente como activo
            };
        }

        // Verifica si todos los campos están correctamente completados
        private bool validarCamposPaciente()
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                cmbEspecialidad.SelectedItem == null)
            {
                MessageBox.Show("Complete todos los campos y seleccione una especialidad.");
                return false;
            }

            return true;
        }

        // Evento para llamar al siguiente paciente en la cola de la especialidad seleccionada
        private void btnLlamar_Click(object sender, EventArgs e)
        {
            if (optClinica.Checked)
            {
                if (esperaClinicaMedica.inicio != null)
                {
                    backup();
                    visorClinica.llamaPaciente(esperaClinicaMedica.inicio);
                    esperaClinicaMedica.Eliminar(); // Elimina al primer paciente de la cola
                    esperaClinicaMedica.Listar(lstClinica); // Actualiza la lista en pantalla
                    List<string> lista = esperaClinicaMedica.devolverRegistros();
                    visorClinica.mostrarProximos(lista); // Muestra los siguientes pacientes
                }
                else
                {
                    MessageBox.Show("No hay pacientes en Clínica Médica.");
                }
            }
            else if (optPediatria.Checked)
            {
                if (esperaPediatria.inicio != null)
                {
                    backup();
                    visorPediatria.llamaPaciente(esperaPediatria.inicio);
                    esperaPediatria.Eliminar();
                    esperaPediatria.Listar(lstPediatria);
                    List<string> lista = esperaPediatria.devolverRegistros();
                    visorPediatria.mostrarProximos(lista);
                }
                else
                {
                    MessageBox.Show("No hay pacientes en Pediatría.");
                }
            }
            else if (optGuardia.Checked)
            {
                if (esperaGuardia.inicio != null)
                {
                    backup();
                    visorGuardia.llamaPaciente(esperaGuardia.inicio);
                    esperaGuardia.Eliminar();
                    esperaGuardia.Listar(lstGuardia);
                    List<string> lista = esperaGuardia.devolverRegistros();
                    visorGuardia.mostrarProximos(lista);
                }
                else
                {
                    MessageBox.Show("No hay pacientes en Guardia.");
                }
            }
            else
            {
                MessageBox.Show("Seleccione una especialidad.");
            }
        }

        // Guarda el estado actual de las colas en archivos de texto
        public void backup()
        {
            // 1. Obtener los registros actuales de la cola de Clínica en formato string
            List<string> listaClinica = esperaClinicaMedica.devolverRegistros();

            // 2. Crear o sobrescribir el archivo backup_clinica.txt
            using (StreamWriter escribir = File.CreateText("backup_clinica.txt"))
            {
                // 3. Escribir cada registro (paciente) en una línea del archivo
                foreach (string registro in listaClinica)
                {
                    escribir.WriteLine(registro);
                }
            }

            // 4. Repetir el proceso para la cola de Pediatría
            List<string> listaPediatria = esperaPediatria.devolverRegistros();
            using (StreamWriter escribir = File.CreateText("backup_pediatria.txt"))
            {
                foreach (string registro in listaPediatria)
                {
                    escribir.WriteLine(registro);
                }
            }

            // 5. Repetir el proceso para la cola de Guardia
            List<string> listaGuardia = esperaGuardia.devolverRegistros();
            using (StreamWriter escribir = File.CreateText("backup_guardia.txt"))
            {
                foreach (string registro in listaGuardia)
                {
                    escribir.WriteLine(registro);
                }
            }
        }


        // Restaura las colas desde los archivos de texto si existen
        public void restaurar()
        {
            if (File.Exists("backup_clinica.txt"))
            {
                using (StreamReader leer = File.OpenText("backup_clinica.txt"))
                {
                    string registro = leer.ReadLine();
                    while (registro != null)
                    {
                        string[] campos = registro.Split(',');
                        esperaClinicaMedica.Insertar(campos[0], campos[1], campos[2]);
                        registro = leer.ReadLine();
                    }
                }
                esperaClinicaMedica.Listar(lstClinica);
            }

            if (File.Exists("backup_pediatria.txt"))
            {
                using (StreamReader leer = File.OpenText("backup_pediatria.txt"))
                {
                    string registro = leer.ReadLine();
                    while (registro != null)
                    {
                        string[] campos = registro.Split(',');
                        esperaPediatria.Insertar(campos[0], campos[1], campos[2]);
                        registro = leer.ReadLine();
                    }
                }
                esperaPediatria.Listar(lstPediatria);
            }

            if (File.Exists("backup_guardia.txt"))
            {
                using (StreamReader leer = File.OpenText("backup_guardia.txt"))
                {
                    string registro = leer.ReadLine();
                    while (registro != null)
                    {
                        string[] campos = registro.Split(',');
                        esperaGuardia.Insertar(campos[0], campos[1], campos[2]);
                        registro = leer.ReadLine();
                    }
                }
                esperaGuardia.Listar(lstGuardia);
            }
        }
    }
}
