using System.Linq;

class Program
{
    static List<Order> orderList = new List<Order>()
    {
        new Order(01, "Никита", 10, new DateTime(2026, 01, 08)),
        new Order(02, "Никита", 20, new DateTime(2026, 02, 08)),
        new Order(03, "Никита", 30, new DateTime(2026,03, 08)),
        new Order(04, "Илья", 40, new DateTime(2026, 01, 08)),
        new Order(05, "Илья", 10, new DateTime(2026, 02, 08)),
        new Order(06, "Владислав", 20, new DateTime(2026, 01, 08)),
        new Order(07, "Илья", 30, new DateTime(2026, 02, 08)),
        new Order(08, "Ярослав", 40, new DateTime(2026, 01, 08)),
        new Order(09, "Владислав", 10, new DateTime(2026, 02, 08)),
        new Order(10, "Владислав", 20, new DateTime(2026, 03, 08)),
        new Order(11, "Михаил", 30, new DateTime(2026, 01, 08)),
        new Order(12, "Михаил", 40, new DateTime(2026, 02, 08)),
    };


    public static void Main()
    {
        var group = orderList.GroupBy(x => x.Customer);
        foreach (var item in group)
        {
            string customer = item.Key;
            double total = item.Sum(x => x.Amount);
            double avg = item.Average(x => x.Amount);
            int count = item.Count();
            Console.WriteLine($"Заказчик: {customer}  Общая сумма заказов: {total}, Среднее число заказов: {avg:F2}   Общее количество заказов: {count}");
        }


        var sorted = group.OrderByDescending(x => x.Sum(y => y.Amount));

        int top = 1;
        foreach (var item in sorted.Take(3))
        {
            string customer = item.Key;
            double total = item.Sum(x => x.Amount);
            Console.WriteLine($"{top}. {customer}: {total}");
            top++;
        }
    }


}

class Order
{
    public int Id { get; set; }
    public string Customer { get; set; }
    public double Amount { get; set; }
    public DateTime Date { get; set; }
    public Order(int id, string customer, double amount, DateTime date)
    {
        Id = id;
        Customer = customer;
        Amount = amount;
        Date = date;
    }
}