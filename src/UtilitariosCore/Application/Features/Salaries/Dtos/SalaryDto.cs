namespace UtilitariosCore.Application.Features.Salaries.Dtos;

public class SalaryDto
{
    public int Id { get; set; }
    public decimal? CurrentMoney { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal AfpDiscount { get; set; }
    public decimal FirstFortnightNet { get; set; }
    public decimal SecondFortnightNet { get; set; }
    public decimal? Cts { get; set; }
    public decimal? Bonus { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSalaryDto
{
    public decimal? CurrentMoney { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal AfpDiscount { get; set; }
    public decimal FirstFortnightNet { get; set; }
    public decimal SecondFortnightNet { get; set; }
    public decimal? Cts { get; set; }
    public decimal? Bonus { get; set; }
}

public class UpdateSalaryDto
{
    public decimal? CurrentMoney { get; set; }
    public decimal? GrossSalary { get; set; }
    public decimal? AfpDiscount { get; set; }
    public decimal? FirstFortnightNet { get; set; }
    public decimal? SecondFortnightNet { get; set; }
    public decimal? Cts { get; set; }
    public decimal? Bonus { get; set; }
}
