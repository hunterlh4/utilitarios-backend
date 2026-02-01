using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IProductRepository
{
    Task<int> CreateProduct(Product item);
    Task<bool> UpdateProduct(Product item);
    Task<bool> UpdateManyItems(Product item);
    Task<Product?> GetProductById(int id);
    Task<Product?> GetProductBySku(string sku);
    Task<IEnumerable<Product>> GetAllProducts();
    Task<bool> CreateManyProductCategory(IEnumerable<ProductCategory> items);
    Task<bool> RemoveManyProductCategory(IEnumerable<ProductCategory> items);
    Task<IEnumerable<ProductCategory>> GetProductCategoriesByProductId(int productId);
    Task<int> GetTotalProducts();
}