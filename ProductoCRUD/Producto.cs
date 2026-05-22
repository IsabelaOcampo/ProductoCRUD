namespace ProductoCRUD
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Nombre} | {Descripcion} | ${Precio:F2} | Cantidad: {Cantidad}";
        }
    }
}