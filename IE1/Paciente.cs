using System; 
namespace IE1
{
    // Clase que representa a un paciente en un sistema médico
    public class Paciente
    {
        public string dni;          
        public string nombre;       
        public string apellido;    
        public bool vigente;       
        public Paciente siguiente;  // Referencia al siguiente paciente (para lista enlazada)

        // Constructor: crea un nuevo paciente con DNI, nombre y apellido
        public Paciente(string dni, string nombre, string apellido)
        {
            this.dni = dni;
            this.nombre = nombre;
            this.apellido = apellido;
            this.siguiente = null;  // Inicialmente no está enlazado a otro paciente
        }
    }
}

