using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly MigrationDbContext _context;

    public ClientesController(MigrationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
    {
        if (_context.Clientes.Any(c => c.Email == cliente.Email))
        {
            return BadRequest("El correo electrónico ya está en uso.");
        }

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        // Verifica que el método GetCliente exista
        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
    }

    // Asegúrate de tener esta acción para que el método CreatedAtAction funcione
    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
        {
            return NotFound();
        }
        return cliente;
    }
}
