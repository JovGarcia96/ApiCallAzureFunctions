using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using DispatchBalanceAPI.Model;
using DispatchBalanceAPI.Controllers;


namespace DispatchBalanceAPI.Model;

public class DispatchBalanceContext : DbContext
{
    public DbSet<DispatchBalanceHeader> dbHeader { get; set; } //Para que aparezca la tabla dentro del _context y se pueda hacer el AddRange de la lista de datos completa
    public DbSet<DispatchBalanceItem> dbDetail { get; set; }
    public DbSet<DispatchBalanceFooter> dbTotal { get; set; }
    public DbSet<AcceptedASN> dbAsn { get; set; }

    public DbSet<SystemDate> dbSystem { get; set; }
    public DbSet<ServiceSalesDate> dbServicesDate { get; set; }

    public DbSet<ServiceLogRecords> dbLogDetail { get; set; }
    //AG-GRID COMENTAR ESTA LÍNEA
    //public DbSet<Records> dbInventoryTransaction { get; set; }
    //AG-GRID DESCOMENTAR ESTA LÍNEA
    //public DbSet<InventoryTransactionsRecords> dbInventoryTransaction { get; set; }
    public DbSet<DatesNonworking> dbDatesNonworking { get; set; }
    public DbSet<MovementRouting> dbMovementRouting { get; set; }
    public DbSet<ServiceDetail> dbServiceDetail { get; set; }
    public DbSet<InventoryTransactionsCatalog> dbInventoryTransactionsCatalog { get; set; }
    public DbSet<TransferProduct> dbTransferProduct { get; set; }
    public DbSet<ProductsList> dbProductsList { get; set; }


    private readonly IConfiguration config;
    internal readonly object ServiceDetails;
    internal object InventoryTransactionsCatalog;

    public DispatchBalanceContext(DbContextOptions<DispatchBalanceContext> contextOptions, IConfiguration config) : base(contextOptions)
    {
        this.config = config;
        RelationalDatabaseCreator databaseCreator =
                    (RelationalDatabaseCreator)Database.GetService<IDatabaseCreator>();
        databaseCreator.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(config.GetConnectionString("DbContext"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DispatchBalanceHeader>().ToTable("DispatchBalanceHeader").HasKey(e => new { e.CeveCode, e.SaleDate });
        modelBuilder.Entity<DispatchBalanceItem>().ToTable("DispatchBalanceItem").HasKey(e => new {e.LineNumber, e.CeveCode, e.SaleDate });
        modelBuilder.Entity<DispatchBalanceFooter>().ToTable("DispatchBalanceFooter").HasKey(e => new {e.CeveCode, e.SaleDate });
        modelBuilder.Entity<AcceptedASN>().ToTable("AcceptedASN").HasKey(e => new { e.CeveCode, e.SaleDate });
        modelBuilder.Entity<SystemDate>().ToTable("SystemDate").HasKey(e => new { e.CeveCode, e.SaleDate });
        modelBuilder.Entity<ServiceLogRecords>().ToTable("ServiceLog").HasKey(e => new { e.LogId });

        /*Adding new Models for  the return of  4 new data */
        modelBuilder.Entity<ServiceSalesDate>().ToTable("ServiceSalesDate").HasKey(e => new { e.ServiceCode, e.CeveCode, e.SaleDate });
        //AG-GRID COMENTAR ESTA LÍNEA REVISAR ESTA LÍNEA PORQUE SI SE AGREGA MANDA ERROR
        //modelBuilder.Entity<Records>().ToTable("InventoryTransactionsCatalog").HasKey(e => new { e.TransactionCode });
        //AG-GRID DESCOMENTAR ESTA LÍNEA
        //modelBuilder.Entity<InventoryTransactionsRecords>().ToTable("InventoryTransactionsCatalog").HasKey(e => new { e.TransactionCode });
        modelBuilder.Entity<DatesNonworking>().ToTable("DateNonworking").HasKey(e => new { e.OrganizationCode, e.CountryCode, e.CeveCode, e.DateNonworking });
        modelBuilder.Entity<ServiceDetail>().ToTable("ServiceDetail").HasKey(e => new { e.DetailId });
        modelBuilder.Entity<MovementRouting>().ToTable("MovementRouting").HasKey(e => new { e.TransactionCode, e.CeveCode, e.TransactionDescription });
        modelBuilder.Entity<InventoryTransactionsCatalog>().ToTable("InventoryTransactionsCatalog").HasKey(e => new { e.TransactionCode });
        modelBuilder.Entity<ProductsList>().ToTable("ProductsList").HasKey(e => new { e.Internal_codes });
        modelBuilder.Entity<TransferProduct>().ToTable("TransferProduct").HasKey(e => new { e.DetailId });
    }

}
