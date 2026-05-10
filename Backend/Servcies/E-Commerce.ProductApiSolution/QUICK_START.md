# Quick Start - Cosmos DB Emulator Connection

## 🚀 Quick Setup (3 Steps)

### 1. Start Cosmos DB Emulator

**Option A - Using PowerShell Script:**

```powershell
.\start-cosmosdb-emulator.ps1
```

**Option B - Manual:**

- Open "Azure Cosmos DB Emulator" from Start Menu
- Wait for browser window to open

### 2. Run the Application

```bash
cd Product.API
dotnet run
```

The app will automatically:

- ✓ Connect to emulator
- ✓ Create database `ECommerceDb`
- ✓ Create container `Products`

### 3. Test the Connection

```bash
# Get all products
curl https://localhost:<port>/api/products

# Get stock for a product
curl https://localhost:<port>/api/stock/{productId}
```

## 📋 Connection Details

| Setting           | Value                    |
| ----------------- | ------------------------ |
| **Endpoint**      | `https://localhost:8081` |
| **Database**      | `ECommerceDb`            |
| **Container**     | `Products`               |
| **Partition Key** | `/id`                    |

## 🔧 Configuration Files Changed

- ✅ `appsettings.Development.json` - Emulator connection string added
- ✅ `CosmosDbContext.cs` - SSL bypass for emulator
- ✅ `CosmosDbInitializer.cs` - Auto-creates DB/Container
- ✅ `Program.cs` - Runs initializer on startup

## 🌐 Useful URLs

- **Data Explorer:** https://localhost:8081/\_explorer/index.html
- **Emulator UI:** https://localhost:8081/

## ⚠️ Common Issues

| Problem            | Solution                         |
| ------------------ | -------------------------------- |
| SSL Error          | Already handled in code          |
| Connection refused | Start the emulator               |
| Port 8081 in use   | Close other apps using port 8081 |

## 📝 Next Steps

1. **View your data:** Open Data Explorer
2. **Add sample data:** Use the API or Data Explorer
3. **Monitor requests:** Check the emulator UI for RU consumption

## 🔐 Security Note

The emulator connection string is a well-known development key. It's safe for local development only. Never use it in production!

---

For detailed information, see [COSMOS_DB_SETUP.md](COSMOS_DB_SETUP.md)
