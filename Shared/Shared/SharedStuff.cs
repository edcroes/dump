using System.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shared
{
    public static class SharedStuff
    {
        public static void AddAuthStuff(this IServiceCollection services)
        {
            _ = services
                .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();

            _ = services.AddAuthorization(options =>
            {
                options.FallbackPolicy = options.DefaultPolicy;
            });
        }

        public static IApplicationBuilder UseAuthStuff(this IApplicationBuilder builder) => builder.UseAuthorization();

        public static string[] FileStuff(string fileName = "C:\\temp\\stuff.txt")
        {
            if (File.Exists(fileName))
            {
                return File.ReadLines(fileName).ToArray();
            }

            return Array.Empty<string>();
        }

        public static void DatabaseStuffWithPredefinedConnectionStrings()
        {
            var connectionString1 = "Data Source=localhost;Database=StuffDB;Integrated Security=sspi;";
            var connectionString2 = "Data Source=SomeOtherServer.somewhere.test;Database=OtherStuffDB;Integrated Security=sspi;";

            DatabaseStuff(connectionString1, connectionString2);
        }

        public static void DatabaseStuff(string connectionString1, string connectionString2)
        {
            using (var scope = new System.Transactions.TransactionScope())
            {
                using (var connection = new SqlConnection(connectionString1))
                {
                    connection.Open();

                    // Mutate table 1
                    using var command1 = connection.CreateCommand();
                    command1.CommandText = "UPDATE Table1 SET Column1 = 'New Value' WHERE Id = 1";
                    command1.ExecuteNonQuery();
                }

                using (var connection = new SqlConnection(connectionString2))
                {
                    connection.Open();

                    // Mutate table 2
                    using var command2 = connection.CreateCommand();
                    command2.CommandText = "UPDATE Table2 SET Column1 = 'New Value' WHERE Id = 1";
                    command2.ExecuteNonQuery();
                }

                // Complete the distributed transaction
                scope.Complete();
            }
        }
    }
}