using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RepairIT.Data;

public class RepairItDbContextFactory : IDesignTimeDbContextFactory<RepairItDbContext>
{
    public RepairItDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<RepairItDbContext>()
            .UseSqlite("Data Source=repairit.db")
            .Options;

        return new RepairItDbContext(options);
    }
}
