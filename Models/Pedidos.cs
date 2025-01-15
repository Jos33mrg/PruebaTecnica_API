using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica_API.Models
{
    public class Pedidos
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        public DateTime FechaPedido { get; set; }

        //[NotMapped]
        //public decimal Total
        //{
        //    get
        //    {
        //        // Calcular el total basado en los productos del pedido
        //        decimal total = 0;
        //        foreach (var pedidoProducto in PedidoProductos)
        //        {
        //            total += pedidoProducto.Cantidad * pedidoProducto.Producto.Precio;
        //        }
        //        return total;
        //    }
        //}
        public decimal Total {  get; set; }

        
        public Cliente Cliente { get; set; } = new Cliente();

        
        public ICollection<PedidoProductos> PedidoProductos { get; set; } = new List<PedidoProductos>();
    }
}
