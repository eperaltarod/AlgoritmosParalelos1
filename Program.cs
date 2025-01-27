using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Barberia
{
    private static ConcurrentQueue<string> turnos = new ConcurrentQueue<string>();
    private static List<string> barberosDisponibles = new List<string>();
    private static List<string> barberosInactivos = new List<string>() { "Juan", "Pedro", "Luis" };

    public static void Main()
    {
        int opcion;
        do
        {
            Console.WriteLine("\n--- Barbería ---");
            Console.WriteLine("1. Habilitar un barbero");
            Console.WriteLine("2. Asignar turno");
            Console.WriteLine("3. Completar turno");
            Console.WriteLine("4. Mostrar listado de turnos");
            Console.WriteLine("5. Consultar disponibilidad de barberos");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione una opción: ");

            if (int.TryParse(Console.ReadLine(), out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        HabilitarBarbero();
                        break;
                    case 2:
                        AsignarTurno();
                        break;
                    case 3:
                        CompletarTurno();
                        break;
                    case 4:
                        MostrarListadoDeTurnos();
                        break;
                    case 5:
                        ConsultarDisponibilidadDeBarberos();
                        break;
                    case 6:
                        Console.WriteLine("Saliendo...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Por favor, ingrese un número válido.");
            }

        } while (opcion != 6);
    }

    private static void HabilitarBarbero()
    {
        if (!barberosInactivos.Any())
        {
            Console.WriteLine("No hay barberos inactivos para habilitar.");
            return;
        }

        Console.WriteLine("\nBarberos inactivos: ");
        for (int i = 0; i < barberosInactivos.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {barberosInactivos[i]}");
        }

        Console.Write("Seleccione el número del barbero a habilitar: ");
        if (int.TryParse(Console.ReadLine(), out int seleccion) && seleccion > 0 && seleccion <= barberosInactivos.Count)
        {
            string barberoSeleccionado = barberosInactivos[seleccion - 1];
            barberosInactivos.Remove(barberoSeleccionado);
            barberosDisponibles.Add(barberoSeleccionado);
            Console.WriteLine($"Barbero {barberoSeleccionado} habilitado.");
        }
        else
        {
            Console.WriteLine("Selección no válida.");
        }
    }

    private static void AsignarTurno()
    {
        if (!barberosDisponibles.Any())
        {
            Console.WriteLine("No hay barberos disponibles para asignar turnos.");
            return;
        }

        Console.Write("Ingrese su nombre: ");
        string cliente = Console.ReadLine();

        Console.WriteLine("Barberos disponibles: ");
        for (int i = 0; i < barberosDisponibles.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {barberosDisponibles[i]}");
        }

        Console.Write("Seleccione el número del barbero: ");
        if (int.TryParse(Console.ReadLine(), out int seleccion) && seleccion > 0 && seleccion <= barberosDisponibles.Count)
        {
            string barberoSeleccionado = barberosDisponibles[seleccion - 1];
            turnos.Enqueue($"Cliente: {cliente}, Barbero: {barberoSeleccionado}");
            Console.WriteLine("Turno asignado exitosamente.");
        }
        else
        {
            Console.WriteLine("Selección no válida.");
        }
    }

    private static void CompletarTurno()
    {
        if (!barberosDisponibles.Any())
        {
            Console.WriteLine("No hay barberos disponibles para completar turnos.");
            return;
        }

        Console.WriteLine("Barberos disponibles: ");
        for (int i = 0; i < barberosDisponibles.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {barberosDisponibles[i]}");
        }

        Console.Write("Seleccione el número del barbero que completó el turno: ");
        if (int.TryParse(Console.ReadLine(), out int seleccion) && seleccion > 0 && seleccion <= barberosDisponibles.Count)
        {
            string barberoSeleccionado = barberosDisponibles[seleccion - 1];

            Task.Run(() =>
            {
                if (turnos.TryDequeue(out string turnoCompletado))
                {
                    Console.WriteLine($"Turno completado por {barberoSeleccionado}: {turnoCompletado}");
                }
                else
                {
                    Console.WriteLine("No hay turnos en la cola para completar.");
                }
            }).Wait();
        }
        else
        {
            Console.WriteLine("Selección no válida.");
        }
    }

    private static void MostrarListadoDeTurnos()
    {
        Task.Run(() =>
        {
            Console.WriteLine("\nListado de turnos:");
            if (turnos.IsEmpty)
            {
                Console.WriteLine("No hay turnos asignados.");
            }
            else
            {
                foreach (var turno in turnos)
                {
                    Console.WriteLine(turno);
                }
            }
        }).Wait();
    }

    private static void ConsultarDisponibilidadDeBarberos()
    {
        Console.WriteLine("\nBarberos disponibles:");
        if (!barberosDisponibles.Any())
        {
            Console.WriteLine("No hay barberos disponibles.");
        }
        else
        {
            foreach (var barbero in barberosDisponibles)
            {
                Console.WriteLine(barbero);
            }
        }

        Console.WriteLine("\nBarberos inactivos:");
        if (!barberosInactivos.Any())
        {
            Console.WriteLine("No hay barberos inactivos.");
        }
        else
        {
            foreach (var barbero in barberosInactivos)
            {
                Console.WriteLine(barbero);
            }
        }
    }
}
