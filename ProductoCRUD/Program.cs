using ProductoCRUD;

class Program
{
    static readonly ProductoRepositorio repo = new ProductoRepositorio();

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        bool salir = false;

        while (!salir)
        {
            Console.WriteLine("\n========== GESTIÓN DE PRODUCTOS ==========");
            Console.WriteLine("1. Crear producto");
            Console.WriteLine("2. Listar todos los productos");
            Console.WriteLine("3. Buscar producto por ID");
            Console.WriteLine("4. Actualizar producto");
            Console.WriteLine("5. Eliminar producto");
            Console.WriteLine("0. Salir");
            Console.Write("Opción: ");

            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1": Crear(); break;
                case "2": Listar(); break;
                case "3": Buscar(); break;
                case "4": Actualizar(); break;
                case "5": Eliminar(); break;
                case "0": salir = true; break;
                default: Console.WriteLine("Opción inválida."); break;
            }
        }
    }

    static void Crear()
    {
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine() ?? "";
        Console.Write("Descripción: ");
        string desc = Console.ReadLine() ?? "";
        Console.Write("Precio: ");
        decimal.TryParse(Console.ReadLine(), out decimal precio);
        Console.Write("Cantidad: ");
        int.TryParse(Console.ReadLine(), out int cantidad);

        try
        {
            var p = repo.Crear(nombre, desc, precio, cantidad);
            Console.WriteLine($"Producto creado: {p}");
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    static void Listar()
    {
        var lista = repo.ObtenerTodos();
        if (lista.Count == 0) { Console.WriteLine("No hay productos registrados."); return; }
        foreach (var p in lista) Console.WriteLine(p);
    }

    static void Buscar()
    {
        Console.Write("ID del producto: ");
        int.TryParse(Console.ReadLine(), out int id);
        try { Console.WriteLine(repo.ObtenerPorId(id)); }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    static void Actualizar()
    {
        Console.Write("ID del producto a actualizar: ");
        int.TryParse(Console.ReadLine(), out int id);
        Console.Write("Nuevo nombre (Enter para no cambiar): ");
        string? nombre = Console.ReadLine();
        Console.Write("Nueva descripción (Enter para no cambiar): ");
        string? desc = Console.ReadLine();
        Console.Write("Nuevo precio (Enter para no cambiar): ");
        string? precioStr = Console.ReadLine();
        Console.Write("Nueva cantidad (Enter para no cambiar): ");
        string? cantStr = Console.ReadLine();

        decimal? precio = string.IsNullOrWhiteSpace(precioStr) ? null : decimal.Parse(precioStr);
        int? cantidad = string.IsNullOrWhiteSpace(cantStr) ? null : int.Parse(cantStr);

        try
        {
            var p = repo.Actualizar(id,
                string.IsNullOrWhiteSpace(nombre) ? null : nombre,
                string.IsNullOrWhiteSpace(desc) ? null : desc,
                precio, cantidad);
            Console.WriteLine($"Actualizado: {p}");
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    static void Eliminar()
    {
        Console.Write("ID del producto a eliminar: ");
        int.TryParse(Console.ReadLine(), out int id);
        try
        {
            repo.Eliminar(id);
            Console.WriteLine($"Producto con ID {id} eliminado correctamente.");
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }
}
