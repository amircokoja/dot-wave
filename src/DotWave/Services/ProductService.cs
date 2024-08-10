using DotWave.Domain;

namespace DotWave.Services
{
    public class ProductService
    {
        private static readonly List<Product> ProductRepository = [];

        public void Create(Product product)
        {
            ProductRepository.Add(product);
        }

        public Product? Get(Guid productId)
        {
            return ProductRepository.FirstOrDefault(p => p.Id == productId);
        }
    }
}
