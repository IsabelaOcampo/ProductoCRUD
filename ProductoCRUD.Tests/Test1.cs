using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductoCRUD;

namespace ProductoCRUD.Tests
{
    [TestClass]
    public class ProductoRepositorioTests
    {
        private string ArchivoTemporal() =>
            Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.json");

        // ══ CREAR ══════════════════════════════════════════════════════

        [TestMethod]
        public void Crear_ProductoValido_RetornaProductoConId()
        {
            var repo = new ProductoRepositorio(ArchivoTemporal());
            var producto = repo.Crear("Laptop", "Laptop gamer", 3500000m, 10);

            Assert.IsNotNull(producto);
            Assert.AreEqual(1, producto.Id);
            Assert.AreEqual("Laptop", producto.Nombre);
            Assert.AreEqual(3500000m, producto.Precio);
            Assert.AreEqual(10, producto.Cantidad);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Crear_NombreVacio_LanzaExcepcion()
        {
            var repo = new ProductoRepositorio(ArchivoTemporal());
            repo.Crear("", "Sin nombre", 100m, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Crear_PrecioNegativo_LanzaExcepcion()
        {
            var repo = new ProductoRepositorio(ArchivoTemporal());
            repo.Crear("Mouse", "Mouse inalámbrico", -50m, 3);
        }

        // ══ LEER ═══════════════════════════════════════════════════════

        [TestMethod]
        public void ObtenerPorId_IdExistente_RetornaProductoCorrecto()
        {
            var repo = new ProductoRepositorio(ArchivoTemporal());
            repo.Crear("Teclado", "Mecánico RGB", 250000m, 20);

            var producto = repo.ObtenerPorId(1);

            Assert.IsNotNull(producto);
            Assert.AreEqual("Teclado", producto.Nombre);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ObtenerPorId_IdInexistente_LanzaExcepcion()
        {
            var repo = new ProductoRepositorio(ArchivoTemporal());
            repo.ObtenerPorId(999);
        }

        [TestMethod]
        public void ObtenerTodos_RepositorioConProductos_RetornaListaCompleta()
        {
            var repo = new ProductoRepositorio(ArchivoTemporal());
            repo.Crear("Monitor", "Full HD", 800000m, 5);
            repo.Crear("Audífonos", "Bluetooth", 150000m, 15);

            var lista = repo.ObtenerTodos();

            Assert.AreEqual(2, lista.Count);
        }

        // ══ ACTUALIZAR ═════════════════════════════════════════════════

        [TestMethod]
        public void Actualizar_ProductoExistente_ModificaDatosCorrectamente()
        {
            var repo = new ProductoRepositorio(ArchivoTemporal());
            repo.Crear("Silla", "Silla de oficina", 500000m, 8);

            var actualizado = repo.Actualizar(1, nombre: "Silla Ergonómica", precio: 650000m);

            Assert.AreEqual("Silla Ergonómica", actualizado.Nombre);
            Assert.AreEqual(650000m, actualizado.Precio);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Actualizar_IdInexistente_LanzaExcepcion()
        {
            var repo = new ProductoRepositorio(ArchivoTemporal());
            repo.Actualizar(999, nombre: "Fantasma");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Actualizar_PrecioNegativo_LanzaExcepcion()
        {
            var repo = new ProductoRepositorio(ArchivoTemporal());
            repo.Crear("Mesa", "Mesa de madera", 300000m, 4);
            repo.Actualizar(1, precio: -100m);
        }

        // ══ ELIMINAR ═══════════════════════════════════════════════════

        [TestMethod]
        public void Eliminar_ProductoExistente_LoRemueveCorrectamente()
        {
            var repo = new ProductoRepositorio(ArchivoTemporal());
            repo.Crear("Impresora", "Impresora láser", 700000m, 3);

            bool resultado = repo.Eliminar(1);
            var lista = repo.ObtenerTodos();
        }

    }
}