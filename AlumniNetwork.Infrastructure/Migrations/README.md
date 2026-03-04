Run migrations from solution root after installing EF tools:

1. dotnet tool install --global dotnet-ef
2. dotnet ef migrations add InitialCreate --project AlumniNetwork.Infrastructure --startup-project AlumniNetwork.API
3. dotnet ef database update --project AlumniNetwork.Infrastructure --startup-project AlumniNetwork.API
