# BasicConnectApi

BasicConnectApi is a simple API developed with .NET Core 8 that provides basic connection functionalities.

## Installation

Make sure you have .NET Core 8 installed on your machine. Then, follow these steps:

1. Clone the repository:

   ```bash
   git clone https://github.com/nicolasquintanam/basic-connect-api.git
   ```

2. Navigate to the project directory:

   ```bash
   cd basic-connect-api/src
   ```

3. Modify the appsettings.json file:

   Open the appsettings.json file and provide the necessary database connection string under the ConnectionStrings section.

4. Run Entity Framework Core Migrations:

   Execute the following command to create the initial database schema:

   ```bash
   dotnet ef migrations add InitialCreate --context ApplicationDbContext
   ```

5. Apply Database Migrations:

   Execute the following command to apply the database migrations and create the database:

   ```bash
   dotnet ef database update
   ```

6. Run the application:

   ```bash
   dotnet run --launch-profile https-dev
   ```

## Testing

To run the unit tests for Basic Connect API, follow these steps:

1. Navigate to the project's test directory:

   ```bash
   cd basic-connect-api/test
   ```

2. Run the tests:
   ```bash
   dotnet test
   ```

## Contributing

Contributions are welcome! If you want to contribute to BasicConnectApi, follow these steps:

1. Fork the repository.
2. Create a new branch: git checkout -b your-branch.
3. Make your changes and commit: git commit -m 'Description of your changes'.
4. Push the branch: git push origin your-branch.
5. Open a pull request on GitHub.

## Issues

If you encounter any issues or have questions, please create an issue.

```

```
