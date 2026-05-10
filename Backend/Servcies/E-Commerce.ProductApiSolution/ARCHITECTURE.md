# Architecture Overview - Cosmos DB Integration

## System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                        Your Application                          │
│                         (Product.API)                            │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             │ HTTPS
                             │
┌────────────────────────────▼────────────────────────────────────┐
│                    Application Layer                             │
│                   (Product.Application)                          │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  ProductService (IProductService)                         │  │
│  │  - Business Logic                                         │  │
│  │  - DTOs: ProductDto, StockRequestDTO, StockResponseDTO   │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             │
┌────────────────────────────▼────────────────────────────────────┐
│                  Infrastructure Layer                            │
│                 (Product.Infrastructure)                         │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  ProductRepository (IProductRepository)                   │  │
│  │  - GetAllProductsAsync()                                  │  │
│  │  - GetStockAsync(productId)                               │  │
│  └────────────────────┬─────────────────────────────────────┘  │
│                       │                                          │
│  ┌────────────────────▼─────────────────────────────────────┐  │
│  │  CosmosDbContext                                          │  │
│  │  - ProductsContainer                                      │  │
│  │  - SSL Certificate Bypass (Emulator)                      │  │
│  │  - Gateway Connection Mode                                │  │
│  └────────────────────┬─────────────────────────────────────┘  │
│                       │                                          │
│  ┌────────────────────▼─────────────────────────────────────┐  │
│  │  CosmosDbInitializer                                      │  │
│  │  - Creates Database: ECommerceDb                          │  │
│  │  - Creates Container: Products                            │  │
│  │  - Partition Key: /id                                     │  │
│  └───────────────────────────────────────────────────────────┘  │
│                       │                                          │
│  ┌────────────────────▼─────────────────────────────────────┐  │
│  │  SampleDataSeeder                                         │  │
│  │  - Seeds 5 sample products                                │  │
│  │  - Only if container is empty                             │  │
│  └───────────────────────────────────────────────────────────┘  │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             │ Cosmos DB SDK
                             │ (Microsoft.Azure.Cosmos)
                             │
┌────────────────────────────▼────────────────────────────────────┐
│              Cosmos DB Emulator (Local)                          │
│                                                                  │
│  Endpoint: https://localhost:8081                                │
│  Data Explorer: https://localhost:8081/_explorer/index.html     │
│                                                                  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  Database: ECommerceDb                                    │  │
│  │  ┌────────────────────────────────────────────────────┐  │  │
│  │  │  Container: Products                                │  │  │
│  │  │  Partition Key: /id                                 │  │  │
│  │  │  Throughput: 400 RU/s                               │  │  │
│  │  │                                                      │  │  │
│  │  │  Documents:                                         │  │  │
│  │  │  - product-001 (Wireless Mouse)                     │  │  │
│  │  │  - product-002 (Mechanical Keyboard)                │  │  │
│  │  │  - product-003 (USB-C Hub)                          │  │  │
│  │  │  - product-004 (Laptop Stand)                       │  │  │
│  │  │  - product-005 (Webcam HD)                          │  │  │
│  │  └────────────────────────────────────────────────────┘  │  │
│  └──────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

## Data Flow

### 1. Application Startup (Development Mode)

```
Program.cs
    │
    ├─► CosmosDbInitializer.InitializeAsync()
    │       │
    │       ├─► Create Database "ECommerceDb" (if not exists)
    │       └─► Create Container "Products" (if not exists)
    │
    └─► SampleDataSeeder.SeedFromConfiguration()
            │
            └─► Add 5 sample products (if container is empty)
```

### 2. Get All Products Request

```
Client Request
    │
    ├─► GET /api/products
    │
    └─► ProductsController.GetAllProducts()
            │
            └─► ProductService.GetAllProductsAsync()
                    │
                    └─► ProductRepository.GetAllProductsAsync()
                            │
                            └─► CosmosDbContext.ProductsContainer
                                    │
                                    └─► Cosmos DB Emulator
                                            │
                                            └─► Returns: List<ProductEntity>
```

### 3. Get Stock Request

```
Client Request
    │
    ├─► GET /api/stock/{productId}
    │
    └─► StockController.GetStock(productId)
            │
            └─► ProductService.GetStockAsync(productId)
                    │
                    └─► ProductRepository.GetStockAsync(productId)
                            │
                            └─► CosmosDbContext.ProductsContainer
                                    │
                                    └─► Cosmos DB Emulator
                                            │
                                            └─► Returns: int? (stock quantity)
```

## Configuration Flow

```
appsettings.Development.json
    │
    ├─► CosmosDb:ConnectionString
    ├─► CosmosDb:DatabaseName
    └─► CosmosDb:ContainerName
            │
            ├─► Injected into CosmosDbContext
            ├─► Injected into CosmosDbInitializer
            └─► Injected into SampleDataSeeder
```

## Dependency Injection Setup

```
DependencyInjection.cs (AddInfrastructure)
    │
    ├─► services.AddSingleton<CosmosDbContext>()
    │       └─► Lifetime: Singleton (one instance for app lifetime)
    │
    ├─► services.AddScoped<IProductRepository, ProductRepository>()
    │       └─► Lifetime: Scoped (one instance per request)
    │
    ├─► services.AddScoped<IProductService, ProductService>()
    │       └─► Lifetime: Scoped (one instance per request)
    │
    └─► services.AddScoped<IBlobService, BlobService>()
            └─► Lifetime: Scoped (one instance per request)
```

## Entity Model

```
ProductEntity
    │
    ├─► id: string (Partition Key)
    ├─► Name: string
    ├─► Category: string
    ├─► Price: decimal
    ├─► Stock: int
    └─► ImageUrl: string
```

## Key Features Implemented

### 1. SSL Certificate Handling

- **Problem:** Emulator uses self-signed certificate
- **Solution:** Custom HttpClientHandler with certificate validation bypass
- **Location:** CosmosDbContext.cs

### 2. Automatic Database Setup

- **Feature:** Auto-creates database and container on startup
- **Benefit:** No manual setup required
- **Location:** CosmosDbInitializer.cs

### 3. Sample Data Seeding

- **Feature:** Automatically seeds 5 sample products
- **Condition:** Only if container is empty
- **Location:** SampleDataSeeder.cs

### 4. Development-Only Initialization

- **Feature:** Initialization only runs in Development environment
- **Benefit:** Safe for production deployment
- **Location:** Program.cs

## Connection Settings

| Setting             | Development             | Production               |
| ------------------- | ----------------------- | ------------------------ |
| **Endpoint**        | https://localhost:8081  | Azure Cosmos DB endpoint |
| **Authentication**  | Well-known emulator key | Managed Identity / Key   |
| **SSL Validation**  | Bypassed                | Enforced                 |
| **Connection Mode** | Gateway                 | Direct (recommended)     |
| **Database**        | ECommerceDb             | ECommerceDb              |
| **Container**       | Products                | Products                 |
| **Partition Key**   | /id                     | /id                      |

## Security Considerations

### Development (Emulator)

- ✅ SSL validation bypassed (safe for local dev)
- ✅ Well-known key (public, safe for local dev)
- ✅ No network exposure (localhost only)

### Production (Azure Cosmos DB)

- ⚠️ Must use proper SSL validation
- ⚠️ Must use secure authentication (Managed Identity preferred)
- ⚠️ Must use connection string from Azure Key Vault
- ⚠️ Must enable firewall rules
- ⚠️ Must enable audit logging

## Performance Characteristics

### Emulator

- **Throughput:** Limited by local machine
- **Latency:** Very low (localhost)
- **Storage:** Limited by disk space
- **Cost:** Free

### Azure Cosmos DB

- **Throughput:** Configurable (400 - 1,000,000+ RU/s)
- **Latency:** Low (single-digit milliseconds)
- **Storage:** Unlimited (pay per GB)
- **Cost:** Pay per RU/s and storage

## Monitoring & Debugging

### Emulator Tools

1. **Data Explorer:** View and query data
2. **Metrics:** Monitor RU consumption
3. **Console Logs:** Application initialization messages

### Production Tools

1. **Azure Portal:** Metrics, logs, alerts
2. **Application Insights:** Distributed tracing
3. **Cosmos DB Insights:** Query performance, RU usage

## Next Steps

1. ✅ Emulator connected and configured
2. ✅ Database and container auto-created
3. ✅ Sample data seeded
4. 🔄 Build your API endpoints
5. 🔄 Add more CRUD operations
6. 🔄 Implement error handling
7. 🔄 Add validation
8. 🔄 Write tests
9. 🔄 Deploy to Azure

---

For detailed setup instructions, see [SETUP_SUMMARY.md](SETUP_SUMMARY.md)
