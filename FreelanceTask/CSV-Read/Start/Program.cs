using File_IO;

string reportFile = "report.txt";
List<string> reportData = new List<string> { };

string errorFile = "error.log";
List<string> errorData = new List<string> { };


FilesWork result = new FilesWork();
string[] data = await result.BasicFileIO();

List<Order> res = new List<Order> {};


if (data == null || data.Length == 0)
{
    Console.WriteLine("Файл пуст");
}
else
{
    for (int i = 1; i < data.Length; i++)
    {
        var parts = data[i].Split(';');
        if (parts.Length == 4)
        {
            Order currentOrder = new Order();
            var product = parts[1].Trim();
            if ((DateTime.TryParse(parts[0].Trim(), out DateTime date) && int.TryParse(parts[2].Trim(), out int qty) && double.TryParse(parts[3].Trim(), out double price) && !string.IsNullOrWhiteSpace(product)) == true)
            {
                currentOrder.Date = date;
                currentOrder.Product = product;
                currentOrder.Quantity = qty;
                currentOrder.Price = price;
                res.Add(currentOrder);
            }
            else
            {
                errorData.Add($"Ошибка парсинга в строке {i}: {data[i]}");
                continue;
            }
        }
    }

    var groups = res.GroupBy(o => o.Product)
                       .Select(g => new
                       {
                           Key = g.Key,
                           TotalSales = g.Sum(o => o.Quantity * o.Price)
                       })
                       .ToList();
    double MaxSales = groups.Max(x => x.TotalSales);

    foreach (var group in groups)
    {
        Console.WriteLine($"Товар: {group.Key}, Общая выручка: {group.TotalSales}");
        reportData.Add($"Товар:{group.Key}, Общая выручка: {group.TotalSales}");
    }
    Console.WriteLine($"Максимальная выручка {MaxSales}");
    reportData.Add($"Максимальная выручка {MaxSales}");

    await result.ReportFileIO(reportFile, reportData);
    await result.ErrorFileIO(errorFile, errorData);
}





class Order
{
    public DateTime Date;
    public string Product;
    public int Quantity;
    public double Price;
    public Order(){}
    public Order(DateTime date, string product, int quantity, double price)
    {
        Date = date;
        Product = product;
        Quantity = quantity;
        Price = price;
    }
}
