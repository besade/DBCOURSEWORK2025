# **Документація складних запитів**

### **Запит 1: Отримати замовлення користувача**

SQL-запит:

```
public async Task<List<Order>> GetUserOrdersAsync(int customerId)
{
    return await _db.Orders
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
        .Where(o => o.CustomerId == customerId)
        .OrderByDescending(o => o.OrderDate)
        .ToListAsync();
}
```

Пояснення:

- JOIN таблиць Order_Table, Order_Item і Product

- Фільтрація за ідентифікатором користувача

- Сортування результатів хронологічно

### **Запит 2: Отримати топ продажів по категоріям**

**Бізнес-питання:**

Які категорії товарів є найдохіднішими?

SQL-запит:

```
public async Task<IEnumerable<CategorySalesDto>> GetSalesByCategoryAsync()
{
    return await _db.OrderItems
        .Include(oi => oi.Product)
        .ThenInclude(p => p.Category)
        .Include(oi => oi.Orders)
        .Where(oi => oi.Orders.OrderStatus == Status.Success)
        .GroupBy(oi => oi.Product.Category.CategoryName)
        .Select(g => new CategorySalesDto
        {
            CategoryName = g.Key,
            TotalQuantitySold = g.Sum(x => x.Quantity),
            TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
        })
        .OrderByDescending(x => x.TotalRevenue)
        .ToListAsync();
}
```

Пояснення:

- JOIN таблиць Order_Item, Product, Customer і Order_Table

- Фільтрація тільки за успішними замовленнями

- Групування за категоріями

- Сума продажів товарів за всією категорією

- Сортування від найдохіднішої категорії до найменш дохідної

### **Запит 3: Отримати топ користувачів за сумою замовлень**

**Бізнес-питання:**

Які користувачі є найактивнішими на нашому сайті?

SQL-запит:

```
    public async Task<IEnumerable<CustomerSpendingDto>> GetTopSpendingCustomersAsync()
    {
        return await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Where(o => o.OrderStatus == Status.Success)
            .GroupBy(o => o.Customer.Email)
            .Select(g => new CustomerSpendingDto
            {
                Email = g.Key,
                OrdersCount = g.Count(),
                TotalSpent = g.Sum(o => o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice))
            })
            .Where(x => x.TotalSpent > 1000)
            .OrderByDescending(x => x.TotalSpent)
            .ToListAsync();
    }
}
```

Пояснення:

- JOIN таблиць Order_Table, Customer і Order_Item

- Фільтрація тільки за успішними замовленнями

- Групування за логінами (email) користувачів

- Сума всіх замовлень

- Фільтрація за користувачами, у яких сума замовлень більша 1000 грн

- Сортування від найактивнішого користувача до найменш активного