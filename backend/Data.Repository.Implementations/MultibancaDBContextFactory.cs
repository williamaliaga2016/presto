using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Repository.Implementations
{
    public class MultibancaDBContextFactory : IDesignTimeDbContextFactory<MultibancaDBContext>
    {
        public MultibancaDBContext CreateDbContext(string[] args)
        {
            string? connectionString =
                GetArgumentValue(args, "--connection")
                ?? GetArgumentValue(args, "-Connection")
                ?? Environment.GetEnvironmentVariable("MULTIBANCA_EF_CONNECTION")
                ?? Environment.GetEnvironmentVariable("ConnectionStrings__Default");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "No se encontro connection string para MultibancaDBContext. " +
                    "Defina ConnectionStrings__Default o MULTIBANCA_EF_CONNECTION antes de ejecutar migraciones.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<MultibancaDBContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new MultibancaDBContext(optionsBuilder.Options);
        }

        private static string? GetArgumentValue(string[] args, string argumentName)
        {
            for (int index = 0; index < args.Length - 1; index++)
            {
                if (string.Equals(args[index], argumentName, StringComparison.OrdinalIgnoreCase))
                {
                    return args[index + 1];
                }
            }

            return null;
        }
    }
}
