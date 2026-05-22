using System.Text.Json;

namespace ProductoCRUD
{
    public class ProductoRepositorio
    {
        private readonly string _rutaArchivo;
        private List<Producto> _productos;

        public ProductoRepositorio(string rutaArchivo = "productos.json")
        {
            _rutaArchivo = rutaArchivo;
            _productos = CargarDesdeArchivo();
        }

        // ── CREAR ──────────────────────────────────────────────────────────────
        public Producto Crear(string nombre, string descripcion, decimal precio, int cantidad)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (precio < 0)
                throw new ArgumentException("El precio no puede ser negativo.");
            if (cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.");

            int nuevoId = _productos.Count > 0 ? _productos.Max(p => p.Id) + 1 : 1;

            var producto = new Producto
            {
                Id = nuevoId,
                Nombre = nombre,
                Descripcion = descripcion,
                Precio = precio,
                Cantidad = cantidad
            };

            _productos.Add(producto);
            GuardarEnArchivo();
            return producto;
        }

        // ── LEER TODOS ─────────────────────────────────────────────────────────
        public List<Producto> ObtenerTodos()
        {
            return new List<Producto>(_productos);
        }

        // ── LEER POR ID ────────────────────────────────────────────────────────
        public Producto ObtenerPorId(int id)
        {
            var producto = _productos.FirstOrDefault(p => p.Id == id);
            if (producto == null)
                throw new KeyNotFoundException($"No se encontró el producto con ID {id}.");
            return producto;
        }

        // ── ACTUALIZAR ─────────────────────────────────────────────────────────
        public Producto Actualizar(int id, string? nombre = null, string? descripcion = null,
                                   decimal? precio = null, int? cantidad = null)
        {
            var producto = ObtenerPorId(id);

            if (nombre != null)
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre no puede estar vacío.");
                producto.Nombre = nombre;
            }
            if (descripcion != null) producto.Descripcion = descripcion;
            if (precio != null)
            {
                if (precio < 0)
                    throw new ArgumentException("El precio no puede ser negativo.");
                producto.Precio = precio.Value;
            }
            if (cantidad != null)
            {
                if (cantidad < 0)
                    throw new ArgumentException("La cantidad no puede ser negativa.");
                producto.Cantidad = cantidad.Value;
            }

            GuardarEnArchivo();
            return producto;
        }

        // ── ELIMINAR ───────────────────────────────────────────────────────────
        public bool Eliminar(int id)
        {
            var producto = ObtenerPorId(id);
            _productos.Remove(producto);
            GuardarEnArchivo();
            return true;
        }

        // ── PERSISTENCIA JSON ──────────────────────────────────────────────────
        private void GuardarEnArchivo()
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_productos, opciones);
            File.WriteAllText(_rutaArchivo, json);
        }

        private List<Producto> CargarDesdeArchivo()
        {
            if (!File.Exists(_rutaArchivo))
                return new List<Producto>();

            string json = File.ReadAllText(_rutaArchivo);
            return JsonSerializer.Deserialize<List<Producto>>(json)
                   ?? new List<Producto>();
        }
    }
}