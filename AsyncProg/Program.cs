using System;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncProg
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var productInfo = await GetProductInfoAsync(1);
            Console.WriteLine("Product Info: \nId: {0}\nPrice: {1}\nStock: {2}", productInfo.Id, productInfo.Price, productInfo.Stock);
        }

        public static Task<ProductInfo> GetProductInfoAsync(int productId)
        {
            //var price = await GetProductPriceAsync(productId);
            //var stock = await GetProductStockAsync(productId);

            // on low scale doing Parallel.ForEach maybe faster but most of the time this will work better as Parallel.ForEach on a large scale can cause issues
            // waitall blocks UI thread which may or may not be acceptable 
            // note: im not checking for default so user output could potentially be wrong if this was wanted
            var concurrent = Task.WhenAll(GetProductPriceAsync(productId), GetProductStockAsync(productId));
            return Task.FromResult(new ProductInfo
            {
                Id = productId,
                Price = concurrent.Result[0],
                Stock = concurrent.Result[1],
            });
        }

        public static async Task<int> GetProductPriceAsync(int productId)
        {
            // Do some async things
            await Task.Run(() => DoSomething());
            
            var _context = new ProductInfoConfiguration();
            return _context.ProductInfo.FirstOrDefault(pi => pi.Id == productId).Price;
        }

        public static async Task<int> GetProductStockAsync(int productId)
        {
            // Do some async things
            await Task.Run(() => DoSomething());

            var _context = new ProductInfoConfiguration();
            return _context.ProductInfo.FirstOrDefault(pi => pi.Id == productId).Stock;
        }

        public static void DoSomething()
        {
            ;
        }
    }
}
