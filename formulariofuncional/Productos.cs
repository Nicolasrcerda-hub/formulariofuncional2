using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace formulariofuncional
{
    public class Producto
    {
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Cantidad { get; set; }
        public string Categoria { get; set; }
        public bool Disponible { get; set; }

        public Producto(string nombre, double precio, int cantidad, string categoria, bool disponible)
        {
            Nombre = nombre;
            Precio = precio;
            Cantidad = cantidad;
            Categoria = categoria;
            Disponible = disponible;
        }

        // Precio por cantidad
        public double Subtotal()
        {
            return Precio * Cantidad;
        }

        // Propiedades para mostrar el producto en la lista
        public string Detalle => Categoria + "  ·  $" + Precio.ToString("0.00") + " x " + Cantidad;
        public string SubtotalTexto => "Subtotal: $" + Subtotal().ToString("0.00");
        public string EstadoTexto => Disponible ? "Disponible" : "No disponible";

        public SolidColorBrush ColorEstado => new SolidColorBrush(
            Disponible ? ColorHelper.FromArgb(255, 22, 101, 52) : ColorHelper.FromArgb(255, 153, 27, 27));

        public SolidColorBrush FondoEstado => new SolidColorBrush(
            Disponible ? ColorHelper.FromArgb(255, 220, 252, 231) : ColorHelper.FromArgb(255, 254, 226, 226));

        public override string ToString()
        {
            return Nombre + " | $" + Precio.ToString("0.00") + " x " + Cantidad
                   + " | " + Categoria + " | " + EstadoTexto;
        }
    }
}