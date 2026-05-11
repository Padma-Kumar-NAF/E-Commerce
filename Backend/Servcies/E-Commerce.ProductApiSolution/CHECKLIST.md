# Cosmos DB Emulator Setup Checklist

Use this checklist to verify your setup is complete and working.

## ☑️ Pre-Setup

- [ ] Cosmos DB Emulator installed
- [ ] .NET 10.0 SDK installed
- [ ] Project builds successfully

## ☑️ Configuration Files

- [x] `appsettings.Development.json` - Contains emulator connection string
- [x] `CosmosDbContext.cs` - SSL bypass configured
- [x] `CosmosDbInitializer.cs` - Database initialization logic
- [x] `SampleDataSeeder.cs` - Sample data seeding logic
- [x] `Program.cs` - Startup initialization configured

## ☑️ Emulator Setup

- [ ] Cosmos DB Emulator is installed
- [ ] Emulator is running (check system tray)
- [ ] Can access https://localhost:8081/\_explorer/index.html
- [ ] No SSL certificate errors in browser

## ☑️ Application Setup

- [ ] Project builds without errors (`dotnet build`)
- [ ] No compilation warnings (except NuGet warnings)
- [ ] All dependencies restored

## ☑️ First Run

- [ ] Run `dotnet run` from Product.API folder
- [ ] See "Cosmos DB initialized successfully" in console
- [ ] See "Seeding sample products..." in console
- [ ] See "✓ Added: [Product Name]" messages (5 times)
- [ ] Application starts without errors

## ☑️ Database Verification

- [ ] Open Data Explorer: https://localhost:8081/\_explorer/index.html
- [ ] Database `ECommerceDb` exists
- [ ] Container `Products` exists
- [ ] Container has 5 items (sample products)
- [ ] Can query: `SELECT * FROM c`

## ☑️ API Testing

### Get All Products

- [ ] Endpoint: `GET /api/products`
- [ ] Returns 200 OK
- [ ] Returns array of 5 products
- [ ] Each product has: id, Name, Category, Price, Stock, ImageUrl

### Get Stock

- [ ] Endpoint: `GET /api/stock/product-001`
- [ ] Returns 200 OK
- [ ] Returns stock quantity (150)
- [ ] Try with invalid ID returns appropriate response

## ☑️ Data Verification

Verify each sample product exists:

- [ ] product-001: Wireless Mouse (Stock: 150)
- [ ] product-002: Mechanical Keyboard (Stock: 75)
- [ ] product-003: USB-C Hub (Stock: 200)
- [ ] product-004: Laptop Stand (Stock: 120)
- [ ] product-005: Webcam HD (Stock: 90)

## ☑️ Testing Scenarios

### Scenario 1: Fresh Start

- [ ] Stop application
- [ ] Delete database in Data Explorer
- [ ] Restart application
- [ ] Database and container recreated
- [ ] Sample data seeded again

### Scenario 2: Restart with Existing Data

- [ ] Stop application
- [ ] Keep database intact
- [ ] Restart application
- [ ] See "Container already has 5 products. Skipping seed."
- [ ] No duplicate data created

### Scenario 3: Emulator Restart

- [ ] Stop application
- [ ] Stop emulator
- [ ] Start emulator
- [ ] Wait for emulator to be ready
- [ ] Start application
- [ ] Everything works as expected

## ☑️ Error Handling

### Test Error Scenarios:

- [ ] Start app without emulator running
  - Expected: Error message about emulator not running
- [ ] Query non-existent product
  - Expected: Appropriate null/not found response
- [ ] Invalid product ID format
  - Expected: Validation error

## ☑️ Performance Check

- [ ] API responds in < 1 second
- [ ] No timeout errors
- [ ] Console shows no warnings (except NuGet)
- [ ] Memory usage is reasonable

## ☑️ Documentation

- [x] SETUP_SUMMARY.md created
- [x] QUICK_START.md created
- [x] COSMOS_DB_SETUP.md created
- [x] ARCHITECTURE.md created
- [x] CHECKLIST.md created (this file)
- [x] start-cosmosdb-emulator.ps1 created

## ☑️ Code Quality

- [x] No compilation errors
- [x] No runtime errors
- [x] Proper error handling implemented
- [x] Console logging for debugging
- [x] Development-only initialization

## ☑️ Security

- [x] Emulator connection string only in Development config
- [x] SSL bypass only for emulator
- [x] Production config placeholder present
- [ ] Production secrets not committed to source control

## ☑️ Next Steps Planning

- [ ] Plan additional CRUD operations
- [ ] Design error handling strategy
- [ ] Plan validation rules
- [ ] Consider pagination for large datasets
- [ ] Plan for production deployment

## 🎯 Success Criteria

Your setup is complete when:

✅ All items in "First Run" section are checked
✅ All items in "Database Verification" section are checked
✅ All items in "API Testing" section are checked
✅ You can successfully query products through the API
✅ You can view data in the Data Explorer

## 📝 Notes

Use this space to track any issues or customizations:

```
Date: ___________
Issues encountered:


Solutions applied:


Custom configurations:


```

## 🆘 If Something Doesn't Work

1. **Check this list** - Did you miss a step?
2. **Check console output** - What errors do you see?
3. **Check Data Explorer** - Is the database created?
4. **Check emulator** - Is it running?
5. **Restart everything** - Sometimes that's all you need
6. **Review documentation** - Check COSMOS_DB_SETUP.md for troubleshooting

## ✅ Final Verification

Run this command to verify everything:

```bash
# Build the project
dotnet build

# Run the application
cd Product.API
dotnet run

# In another terminal, test the API
curl https://localhost:<port>/api/products
```

If you see a JSON array with 5 products, **you're all set!** 🎉

---

**Setup Date:** ****\_\_\_****
**Verified By:** ****\_\_\_****
**Status:** ⬜ Complete ⬜ In Progress ⬜ Issues Found
