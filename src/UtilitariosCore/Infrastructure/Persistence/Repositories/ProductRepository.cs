using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using Dapper;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class ProductRepository(MssqlContext context) : IProductRepository
{
    public async Task<int> CreateProduct(Product item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Products
        (
            Name,
            Sku,
            ProductGroupId,
            PresentationUnit,
            PresentationSize,
            Unit,
            Size,
            ProductType,
            Description,
            MinimumStock,
            GroupStatus,
            CreatedAt
        )
        VALUES
        (
            @Name,
            @Sku,
            @ProductGroupId,
            @PresentationUnit,
            @PresentationSize,
            @Unit,
            @Size,
            @ProductType,
            @Description,
            @MinimumStock,
            @GroupStatus,
            @CreatedAt
        )

        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);

        return result;
    }

    public async Task<bool> UpdateProduct(Product item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Products
        SET
            Sku = @Sku,
            Name = @Name,
            ProductGroupId = @ProductGroupId,
            PresentationUnit = @PresentationUnit,
            PresentationSize = @PresentationSize,
            Unit = @Unit,
            Size = @Size,
            ProductType = @ProductType,
            Description = @Description,
            MinimumStock = @MinimumStock,
            GroupStatus = @GroupStatus,
            UpdatedAt = @UpdatedAt
        WHERE
            Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);

        return result > 0;
    }

    public async Task<bool> UpdateManyItems(Product item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE InspectionItems
        SET
	        Sku = @Sku,
	        Name = @Name
        WHERE
	        ItemId = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);

        return result > 0;
    }

    public async Task<Product?> GetProductById(int id)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            Id,
            Sku,
            Name,
            ProductGroupId,
            PresentationUnit,
            PresentationSize,
            Unit,
            Size,
            ProductType,
            Description,
            MinimumStock,
            GroupStatus,
            CreatedAt,
            UpdatedAt
        FROM Products
        WHERE
            Id = @Id
        ";

        var result = await db.QueryFirstOrDefaultAsync<Product>(sql, new
        {
            Id = id
        });

        return result;
    }

    public async Task<Product?> GetProductBySku(string sku)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            Id,
            Sku,
            Name,
            ProductGroupId,
            PresentationUnit,
            PresentationSize,
            Unit,
            Size,
            ProductType,
            Description,
            MinimumStock,
            GroupStatus,
            CreatedAt,
            UpdatedAt
        FROM Products
        WHERE
            Sku = @Sku
        ";

        var result = await db.QueryFirstOrDefaultAsync<Product>(sql, new
        {
            Sku = sku
        });

        return result;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            Id,
            Sku,
            Name,
            ProductGroupId,
            PresentationUnit,
            PresentationSize,
            Unit,
            Size,
            ProductType,
            Description,
            MinimumStock,
            GroupStatus,
            CreatedAt,
            UpdatedAt
        FROM Products
        ";

        var result = await db.QueryAsync<Product>(sql);

        return result;
    }

    public async Task<bool> CreateManyProductCategory(IEnumerable<ProductCategory> items)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO ProductCategories
        (
            ProductId,
            CategoryId,
            CreatedAt
        )
        VALUES
        (
            @ProductId,
            @CategoryId,
            @CreatedAt
        )
        ";

        var result = await db.ExecuteAsync(sql, items);

        return result > 0;
    }

    public async Task<bool> RemoveManyProductCategory(IEnumerable<ProductCategory> items)
    {
        var db = context.CreateDefaultConnection();

        string sql = "DELETE FROM ProductCategories WHERE ProductId = @ProductId AND CategoryId = @CategoryId";

        var result = await db.ExecuteAsync(sql, items);

        return result > 0;
    }

    public async Task<IEnumerable<ProductCategory>> GetProductCategoriesByProductId(int productId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            ProductId,
            CategoryId,
            CreatedAt
        FROM ProductCategories
        WHERE
            ProductId = @ProductId
        ";

        var result = await db.QueryAsync<ProductCategory>(sql, new
        {
            ProductId = productId
        });

        return result;
    }

    public async Task<int> GetTotalProducts()
    {
        var db = context.CreateDefaultConnection();

        string sql = "SELECT COUNT(Id) FROM Products";

        var result = await db.ExecuteScalarAsync<int>(sql);

        return result;
    }
}