using Microsoft.EntityFrameworkCore;
using QueryEfficiency.Configurations;
using QueryEfficiency.Entities;

namespace QueryEfficiency
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var dbContext = new DBContext();

            // clear the db table
            dbContext.Database.ExecuteSqlRaw("DELETE FROM OrderLine");

            var cartItems = dbContext.CartItems.ToList();
            var doStuff2 = await DoQueryOptimizedAsync(dbContext, cartItems);
            var doStuff1 = await DoQueryAsync(dbContext, cartItems);

            Console.Write("Done");
        }

        #region Prompt
        // changed cartItem.Number to cartItem.ProductNumber because that made sense to me. Otherwise what is Number vs ProductNumber in this case?
        // removed priceQuerier wasnt needed to implement for this test
        // customer number = 1 (our first customer!)
        static async Task<Order> DoQueryAsync(DBContext dbContext, List<CartItem> cartItems)
        {
            Console.WriteLine("Start " + DateTime.Now.Second.ToString());

            var order = new Order();
            var lastOrder = dbContext.Orders.ToList();
            if(lastOrder.Any())
                order.Id = lastOrder.Max(o => o.Id) + 1;
            else
                order.Id = 1;

            foreach (var cartItem in cartItems)
            {
                var matchingProduct = await dbContext
                    .Products
                    .SingleOrDefaultAsync(product => product.Number == cartItem.ProductNumber); 
                order.Lines.Add(new OrderLine
                {
                    OrderId = order.Id,
                    ProductId = matchingProduct!.Id, // assume for this exam itll never be null
                    ProductNumber = cartItem.ProductNumber,
                    Quantity = cartItem.Quantity,
                    UnitPrice = await GetCustomerPriceAsync(1, cartItem.ProductNumber),
                });
            }
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("End " + DateTime.Now.Second.ToString());
            // clear the db table
            dbContext.Database.ExecuteSqlRaw("DELETE FROM OrderLine");
            return order;
        }
        #endregion

        // Optimized
        // using dictionary because the lookup is fast and in the prompt we never needed more than just the key value
        // old school for loops with an array are some amounts (grand scheme of things marginally) faster than foreach and using lists
        static async Task<Order> DoQueryOptimizedAsync(DBContext dbContext, List<CartItem> cartItems)
        {
            Console.WriteLine("Start " + DateTime.Now.Second.ToString());
            var order = new Order();
            var lastOrder = dbContext.Orders.ToList();
            if (lastOrder.Any())
                order.Id = lastOrder.Max(o => o.Id) + 1;
            else
                order.Id = 1;

            // got products 
            var products = dbContext.Products.Where(p => cartItems.Select(ci => ci.ProductNumber).Contains(p.Number)).ToDictionary(d => d.Number, d => d.Id);
            // pretend we only have 1 customer 
            var prices = dbContext.CustomerPrices.Where(cp => cp.Id == 1 && products.Keys.Contains(cp.ProductNumber)).ToDictionary(d => d.ProductNumber, d => d.UnitPrice);
            var orderLines = new List<OrderLine>();
            var cartItemsArr = cartItems.ToArray();
            for(int i = 0; i < cartItems.Count(); i++)
                orderLines.Add(new OrderLine
                {
                    OrderId = order.Id,
                    ProductId = products[cartItems[i].ProductNumber],
                    ProductNumber = cartItems[i].ProductNumber,
                    Quantity = cartItems[i].Quantity,
                    UnitPrice = prices[cartItems[i].ProductNumber],
                });

            // for testing
            await Task.Run(() => Console.Write(""));
            order.Lines = orderLines.ToList();
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("End " + DateTime.Now.Second.ToString());
            return order;
        }


        public static async Task<int> GetCustomerPriceAsync(int customerNumber, int ProductNumber)
        {
            var dbContext = new DBContext();

            // do some async things
            await Task.Run(() => Console.Write(""));

            // pretend that thisll never not be null for the exam
            var price = dbContext.CustomerPrices.FirstOrDefault(cp 
                => cp.Id ==  customerNumber
                && cp.ProductNumber == ProductNumber);
            return price!.UnitPrice;
        }
    }
}