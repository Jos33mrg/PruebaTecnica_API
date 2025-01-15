using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica_API.Models
{
    public class PedidoProductos
    {
        [ForeignKey("Pedido")]
        public int PedidoId { get; set; }

        [ForeignKey("Producto")]
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public Pedidos Pedido { get; set; } = new Pedidos();

        public Productos Producto { get; set; } = new Productos();
    }
}
