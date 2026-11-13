using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace formulariofuncional
{
    public sealed partial class MainWindow : Window
    {
        // Lista donde se guardan los productos
        private List<Producto> productos = new List<Producto>();

        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "Inventario de productos";

            // Carga de datos de ejemplo al abrir la ventana
            CargarDatos();
        }

        private void CargarDatos()
        {
            productos.Add(new Producto("Cuaderno", 35.50, 20, "Papelería", true));
            productos.Add(new Producto("Mouse USB", 150.00, 5, "Electrónica", true));
            productos.Add(new Producto("Cloro", 28.00, 0, "Limpieza", false));
            ActualizarLista();
        }

        // Refresca la lista y calcula el total
        private void ActualizarLista(bool mantenerSeleccion = false)
        {
            int seleccionado = lstProductos.SelectedIndex;

            lstProductos.ItemsSource = null;
            lstProductos.ItemsSource = productos;

            if (mantenerSeleccion && seleccionado >= 0 && seleccionado < productos.Count)
            {
                lstProductos.SelectedIndex = seleccionado;
            }

            double total = 0;
            foreach (Producto p in productos)
            {
                total = total + p.Subtotal();
            }

            int disponibles = productos.Count(p => p.Disponible);
            lblResumen.Text = productos.Count + " productos  ·  " + disponibles + " disponibles";
            lblTotal.Text = "Valor total del inventario: $" + total.ToString("0.00");
        }

        // Boton Agregar
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtProducto.Text.Trim();

            // Validar campos vacios
            if (nombre == "" || txtPrecio.Text.Trim() == "" || txtCantidad.Text.Trim() == "" || cmbCategoria.SelectedItem == null)
            {
                MostrarMensaje("Llena todos los campos.", false);
                return;
            }

            // Validar que no exista un producto con el mismo nombre
            if (productos.Any(p => string.Equals(p.Nombre, nombre, StringComparison.OrdinalIgnoreCase)))
            {
                MostrarMensaje("Ya existe un producto con ese nombre.", false);
                return;
            }

            // Validar que el precio sea un numero
            double precio;
            if (!double.TryParse(txtPrecio.Text, out precio) || precio <= 0)
            {
                MostrarMensaje("El precio debe ser un número mayor a 0.", false);
                return;
            }

            // Validar que la cantidad sea un numero entero
            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad < 0)
            {
                MostrarMensaje("La cantidad debe ser un número entero de 0 o más.", false);
                return;
            }

            bool disponible = chkDisponible.IsChecked == true;

            // Un producto sin existencias no puede estar disponible
            if (disponible && cantidad == 0)
            {
                MostrarMensaje("Un producto con cantidad 0 no puede estar disponible. Desmarca la casilla o cambia la cantidad.", false);
                return;
            }

            Producto nuevo = new Producto(
                nombre,
                precio,
                cantidad,
                cmbCategoria.SelectedItem?.ToString() ?? "",
                disponible);

            productos.Add(nuevo);
            ActualizarLista();
            LimpiarCampos();
            MostrarMensaje("Producto agregado correctamente.", true);
        }

        // Boton Cambiar disponibilidad
        private void btnDisponibilidad_Click(object sender, RoutedEventArgs e)
        {
            int indice = lstProductos.SelectedIndex;

            if (indice == -1)
            {
                MostrarMensaje("Selecciona un producto de la lista para cambiar su disponibilidad.", false);
                return;
            }

            Producto p = productos[indice];

            // No se puede activar un producto sin existencias
            if (!p.Disponible && p.Cantidad == 0)
            {
                MostrarMensaje("No se puede marcar como disponible un producto con cantidad 0.", false);
                return;
            }

            p.Disponible = !p.Disponible;
            ActualizarLista(true);
            MostrarMensaje(p.Nombre + " ahora está " + (p.Disponible ? "disponible." : "no disponible."), true);
        }

        // Boton Eliminar
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            int indice = lstProductos.SelectedIndex;

            if (indice == -1)
            {
                MostrarMensaje("Selecciona un producto de la lista para eliminarlo.", false);
                return;
            }

            productos.RemoveAt(indice);
            ActualizarLista();
            MostrarMensaje("Producto eliminado.", true);
        }

        // Boton Limpiar
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
            lblMensaje.Text = "";
        }

        private void LimpiarCampos()
        {
            txtProducto.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";
            cmbCategoria.SelectedIndex = -1;
            chkDisponible.IsChecked = true;
        }

        // Mensaje verde si salio bien, rojo si hubo error
        private void MostrarMensaje(string texto, bool exito)
        {
            lblMensaje.Text = texto;

            if (exito)
            {
                lblMensaje.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                lblMensaje.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}