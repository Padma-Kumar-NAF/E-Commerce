# How to Connect Your Backend to Cosmos DB Emulator

## ✅ Good News: You're Already Connected!

Your backend is **already configured** to connect to the Cosmos DB Emulator. Here's how it works:

## 🔌 Connection Flow

```
┌─────────────────────────────────────────────────────────────┐
│  Cosmos DB Emulator (Running on your machine)               │
│                                                              │
│  Shows 4 values:                                             │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ URI: https://localhost:8081/                           │ │
│  │ Primary Key: C2y6yDjf5/R+ob0N8A7Cgv30VRDJ...          │ │
│  │ Primary Connection String: AccountEndpoint=https://... │ │ ← YOU NEED THIS
│  │ Mongo Connection String: mongodb://localhost:10255/... │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                              │
                              │ Already configured in
                              ▼
┌─────────────────────────────────────────────────────────────┐
│  appsettings.Development.json                                │
│                                                              │
│  {                                                           │
│    "CosmosDb": {                                             │
│      "ConnectionString": "AccountEndpoint=https://local...  │ ← MATCHES!
│      "DatabaseName": "ECommerceDb",                          │
│      "ContainerName": "Products"                             │
│    }                                                         │
│  }                                                           │
└─────────────────────────────────────────────────────────────┘
                              │
                              │ Used by
                              ▼
┌─────────────────────────────────────────────────────────────┐
│  CosmosDbContext.cs                                          │
│                                                              │
│  - Reads connection string from configuration                │
│  - Creates CosmosClient                                      │
│  - Connects to emulator                                      │
│  - Provides ProductsContainer to repositories                │
└─────────────────────────────────────────────────────────────┘
```

## 📝 Understanding the 4 Values in Emulator

### 1. URI (Endpoint)

```
https://localhost:8081/
```

- **What it is:** Just the endpoint URL
- **Do you need it separately?** ❌ No, it's already in the connection string

### 2. Primary Key

```
C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==
```

- **What it is:** Authentication key
- **Do you need it separately?** ❌ No, it's already in the connection string
- **Note:** This is a well-known emulator key (same for everyone, safe for local dev)

### 3. Primary Connection String ✅ **THIS IS WHAT YOU USE**

```
AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==
```

- **What it is:** Complete connection string (URI + Key combined)
- **Do you need it?** ✅ **YES - Already in your config!**
- **Used for:** SQL API (what your project uses)

### 4. Mongo Connection String

```
mongodb://localhost:10255/...
```

- **What it is:** Connection string for MongoDB API
- **Do you need it?** ❌ No, you're using SQL API, not MongoDB API

## ✅ Verification Steps

### Step 1: Verify Emulator is Running

Run this command:

```powershell
.\test-connection.ps1
```

Or manually check:

- Look for "Azure Cosmos DB Emulator" in system tray
- Open: https://localhost:8081/\_explorer/index.html

### Step 2: Verify Configuration

Your `appsettings.Development.json` should have:

```json
{
  "CosmosDb": {
    "ConnectionString": "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
    "DatabaseName": "ECommerceDb",
    "ContainerName": "Products"
  }
}
```

✅ **This is already correct!**

### Step 3: Run Your Application

```bash
cd Product.API
dotnet run
```

You should see:

```
Database 'ECommerceDb' created or already exists.
Container 'Products' created or already exists.
Cosmos DB initialized successfully.
Seeding sample products...
  ✓ Added: Wireless Mouse
  ✓ Added: Mechanical Keyboard
  ✓ Added: USB-C Hub
  ✓ Added: Laptop Stand
  ✓ Added: Webcam HD
Sample data seeding completed. Added 5 products.
```

### Step 4: Verify in Data Explorer

1. Open: https://localhost:8081/\_explorer/index.html
2. Look for `ECommerceDb` database
3. Click on `Products` container
4. Click "Items" - you should see 5 products

### Step 5: Test the API

```bash
# Get all products
curl https://localhost:<your-port>/api/products

# Expected response:
[
  {
    "id": "product-001",
    "name": "Wireless Mouse",
    "category": "Electronics",
    "price": 29.99,
    "stock": 150,
    "imageUrl": "https://example.com/images/wireless-mouse.jpg"
  },
  ...
]
```

## 🔧 If You Need to Change the Connection

### Scenario 1: Emulator on Different Port

If your emulator runs on a different port (not 8081):

1. Check the emulator's actual URI
2. Update `appsettings.Development.json`:

```json
"ConnectionString": "AccountEndpoint=https://localhost:YOUR_PORT/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
```

### Scenario 2: Using Azure Cosmos DB (Production)

For production, update `appsettings.json` (not Development):

1. Get your connection string from Azure Portal
2. Update `appsettings.json`:

```json
{
  "CosmosDb": {
    "ConnectionString": "YOUR_AZURE_COSMOS_CONNECTION_STRING",
    "DatabaseName": "ECommerceDb",
    "ContainerName": "Products"
  }
}
```

**⚠️ Important:** Never commit production connection strings to source control!

## 🎯 Quick Test Commands

### Test 1: Check Emulator

```powershell
.\test-connection.ps1
```

### Test 2: Run Application

```bash
cd Product.API
dotnet run
```

### Test 3: Query API

```bash
# Replace <port> with your actual port (shown when app starts)
curl https://localhost:<port>/api/products
curl https://localhost:<port>/api/stock/product-001
```

### Test 4: View in Data Explorer

```
https://localhost:8081/_explorer/index.html
```

## 📊 Connection Status Checklist

- [ ] Emulator is running (check system tray)
- [ ] Can access https://localhost:8081/\_explorer/index.html
- [ ] `appsettings.Development.json` has correct connection string
- [ ] Application starts without errors
- [ ] See "Cosmos DB initialized successfully" in console
- [ ] Database `ECommerceDb` exists in Data Explorer
- [ ] Container `Products` exists with 5 items
- [ ] API returns products when queried

## 🎉 Success!

If all checklist items are complete, your backend is successfully connected to the Cosmos DB Emulator!

## 🆘 Troubleshooting

### Problem: "Unable to connect to the remote server"

**Solution:** Make sure the emulator is running

```powershell
.\start-cosmosdb-emulator.ps1
```

### Problem: "The SSL connection could not be established"

**Solution:** Already handled in code. If you still see this, verify `CosmosDbContext.cs` has the SSL bypass.

### Problem: "Database not created"

**Solution:** Check console output for errors. The app auto-creates it on startup.

### Problem: "No data in container"

**Solution:** The app seeds data automatically. Check console for seeding messages.

## 📚 Related Documentation

- **Quick Start:** [QUICK_START.md](QUICK_START.md)
- **Full Setup:** [COSMOS_DB_SETUP.md](COSMOS_DB_SETUP.md)
- **Architecture:** [ARCHITECTURE.md](ARCHITECTURE.md)
- **Checklist:** [CHECKLIST.md](CHECKLIST.md)

---

**Bottom Line:** Your backend is already connected! Just start the emulator and run your app. 🚀
