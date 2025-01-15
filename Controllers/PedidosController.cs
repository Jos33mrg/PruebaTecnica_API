using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica_API.Models;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly MigrationDbContext _context;

    public PedidosController(MigrationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Cliente")]  // Admin y Cliente pueden crear pedidos
    public async Task<ActionResult<Pedidos>> PostPedido(Pedidos nuevoPedido)
    {
        var totalPedido = 0m;

        foreach (var pedidoProducto in nuevoPedido.PedidoProductos)
        {
            var producto = await _context.Productos.FindAsync(pedidoProducto.ProductoId);
            if (producto == null)
            {
                return NotFound($"Producto con ID {pedidoProducto.ProductoId} no encontrado.");
            }
            if (producto.Stock < pedidoProducto.Cantidad)
            {
                return BadRequest($"No hay suficiente stock para el producto {producto.Nombre}.");
            }

            totalPedido += producto.Precio * pedidoProducto.Cantidad;
            producto.Stock -= pedidoProducto.Cantidad;
        }

        nuevoPedido.Total = totalPedido;
        nuevoPedido.FechaPedido = DateTime.UtcNow;

        _context.Pedidos.Add(nuevoPedido);
        await _context.SaveChangesAsync(); 

        return CreatedAtAction("GetPedido", new { id = nuevoPedido.Id }, nuevoPedido);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Cliente")]  // Admin y Cliente pueden ver los pedidos
    public async Task<ActionResult<Pedidos>> GetPedido(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.PedidoProductos)
            .ThenInclude(pp => pp.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
        {
            return NotFound();
        }

        return pedido;
    }
}
