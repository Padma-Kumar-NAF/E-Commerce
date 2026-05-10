# Cosmos DB Emulator Setup Guide

## Prerequisites

1. **Install Cosmos DB Emulator**
   - Download from: https://aka.ms/cosmosdb-emulator
   - Install and run the emulator
   - The emulator will start on `https://localhost:8081`

## Configuration

The project is now configured to connect to the Cosmos DB emulator with the following settings:

### Connection Details (in appsettings.Development.json)

```json
{
  "CosmosDb": {
    "ConnectionString": "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
    "DatabaseName": "ECommerceDb",
    "ContainerName": "Products"
  }
}
```

**Note:** The connection string above is the default emulator connection string. This is a well-known key and is safe to use for local development.

## What Was Changed

1. **appsettings.Development.json** - Added Cosmos DB emulator connection string
2. **CosmosDbContext.cs** - Added SSL certificate validation bypass for emulator
3. **CosmosDbInitializer.cs** - New class to automatically create database and container
4. **Program.cs** - Added automatic database initialization on startup

## How to Use

### Step 1: Start the Cosmos DB Emulator

1. Open the Cosmos DB Emulator from your Start menu
2. Wait for it to fully start (you'll see a browser window open)
3. The emulator runs at: `https://localhost:8081`

### Step 2: Run Your Application

```bash
cd Product.API
dotnet run
```

The application will:

- Automatically connect to the emulator
- Create the `ECommerceDb` database if it doesn't exist
- Create the `Products` container if it doesn't exist
- Be ready to accept requests

### Step 3: Verify Connection

You can verify the connection by:

1. **Using the Emulator Data Explorer:**

   - Open `https://localhost:8081/_explorer/index.html`
   - You should see the `ECommerceDb` database
   - Inside it, you'll find the `Products` container

2. **Testing the API:**
   ```bash
   # Get all products (should return empty array initially)
   curl https://localhost:<your-port>/api/products
   ```

## Troubleshooting

### Issue: "The SSL connection could not be established"

**Solution:** The code now handles this automatically by bypassing SSL validation for the emulator. If you still see this error, ensure the emulator is running.

### Issue: "Unable to connect to the remote server"

**Solution:**

- Make sure the Cosmos DB Emulator is running
- Check that it's running on port 8081
- Try restarting the emulator

### Issue: "The remote certificate is invalid"

**Solution:** This is handled by the `ServerCertificateCustomValidationCallback` in `CosmosDbContext.cs`. The emulator uses a self-signed certificate which is bypassed in development mode.

### Issue: Database/Container not created

**Solution:**

- Check the console output when starting the application
- Look for initialization messages
- Manually create them using the Data Explorer at `https://localhost:8081/_explorer/index.html`

## Emulator Data Explorer

Access the Cosmos DB Emulator Data Explorer at:

```
https://localhost:8081/_explorer/index.html
```

Here you can:

- View your databases and containers
- Query data using SQL
- Add sample data manually
- Monitor request units (RUs)

## Adding Sample Data

You can add sample products through the API or directly in the Data Explorer:

```json
{
  "id": "product-001",
  "name": "Sample Product",
  "description": "This is a sample product",
  "price": 29.99,
  "stock": 100,
  "imageUrl": "https://example.com/image.jpg"
}
```

## Production Configuration

For production, update `appsettings.json` with your actual Azure Cosmos DB connection string:

```json
{
  "CosmosDb": {
    "ConnectionString": "YOUR_AZURE_COSMOS_CONNECTION_STRING",
    "DatabaseName": "ECommerceDb",
    "ContainerName": "Products"
  }
}
```

**Important:** Never commit production connection strings to source control. Use Azure Key Vault or environment variables instead.

## Additional Resources

- [Cosmos DB Emulator Documentation](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator)
- [Cosmos DB .NET SDK Documentation](https://docs.microsoft.com/en-us/azure/cosmos-db/sql/sql-api-sdk-dotnet-standard)
