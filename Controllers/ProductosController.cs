using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica_API.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly MigrationDbContext _context;

    public ProductosController(MigrationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    //[Authorize(Roles = "Admin,Cliente")]  // Permitir a Admin y Cliente
    public async Task<ActionResult<IEnumerable<Productos>>> GetProductos(decimal? minPrecio, decimal? maxPrecio, int? minStock)
    {
        try
        {
            // Validar que minPrecio no sea mayor que maxPrecio
            if (minPrecio.HasValue && maxPrecio.HasValue && minPrecio > maxPrecio)
            {
                return BadRequest("El precio mínimo no puede ser mayor que el precio máximo.");
            }

            var query = _context.Productos.AsQueryable();

            if (minPrecio.HasValue)
                query = query.Where(p => p.Precio >= minPrecio);
            if (maxPrecio.HasValue)
                query = query.Where(p => p.Precio <= maxPrecio);
            if (minStock.HasValue)
                query = query.Where(p => p.Stock >= minStock);

            var productos = await query.ToListAsync();

            return Ok(productos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error interno del servidor: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin")]  // Solo Admin puede actualizar productos
    public async Task<IActionResult> PutProducto(int id, Productos producto)
    {
        if (id != producto.Id)
        {
            return BadRequest("El ID del producto no coincide.");
        }

        var existingProducto = await _context.Productos.FindAsync(id);
        if (existingProducto == null)
        {
            return NotFound();
        }

        existingProducto.Nombre = producto.Nombre;
        existingProducto.Precio = producto.Precio;
        existingProducto.Stock = producto.Stock;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
