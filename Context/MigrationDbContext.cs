using Microsoft.EntityFrameworkCore;
using PruebaTecnica_API.Models;
using PruebaTecnica_API.Models.Auth;

public class MigrationDbContext : DbContext
{
    public MigrationDbContext(DbContextOptions<MigrationDbContext> options) : base(options) { }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Productos> Productos { get; set; }
    public DbSet<Pedidos> Pedidos { get; set; }
    public DbSet<PedidoProductos> PedidoProductos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>(e =>
        {
            e.HasIndex(ix => ix.Id).IsUnique(); 
        });

        modelBuilder.Entity<Productos>(e =>
        {
            e.HasIndex(ix => ix.Nombre).IsUnique(); 
        });

        modelBuilder.Entity<Pedidos>()
            .HasOne(p => p.Cliente) 
            .WithMany() 
            .HasForeignKey(p => p.ClienteId) 
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<PedidoProductos>()
            .HasKey(pp => new { pp.PedidoId, pp.ProductoId }); 

        modelBuilder.Entity<PedidoProductos>()
            .HasOne(pp => pp.Pedido)
            .WithMany(p => p.PedidoProductos)
            .HasForeignKey(pp => pp.PedidoId);

        modelBuilder.Entity<PedidoProductos>()
            .HasOne(pp => pp.Producto)
            .WithMany()
            .HasForeignKey(pp => pp.ProductoId);
    }
}
