using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public class BulkImportJavItem
{
    public string? Code { get; set; }
    public List<string> Actresses { get; set; } = [];
}

public record BulkImportJavCommand : IRequest<Result<BulkImportJavResult>>
{
    public List<BulkImportJavItem> Items { get; set; } = [];

    internal sealed class Handler(
        IJavRepository javRepository,
        IActressJavRepository actressJavRepository)
        : IRequestHandler<BulkImportJavCommand, Result<BulkImportJavResult>>
    {
        public async Task<Result<BulkImportJavResult>> Handle(BulkImportJavCommand request, CancellationToken cancellationToken)
        {
            var result = new BulkImportJavResult();

            foreach (var item in request.Items)
            {
                var code = string.IsNullOrWhiteSpace(item.Code)
                    ? null
                    : item.Code.Trim().ToUpper();

                var actressNames = item.Actresses
                    .Select(a => a.Trim())
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .Select(a => StringNormalizer.ToTitleCase(a))
                    .ToList();

                int? javId = null;

                // Registrar JAV si hay código
                if (!string.IsNullOrWhiteSpace(code))
                {
                    var existingJav = await javRepository.GetJavByCode(code);
                    if (existingJav != null)
                    {
                        javId = existingJav.Id;
                    }
                    else
                    {
                        javId = await javRepository.CreateJav(new Jav
                        {
                            Code = code,
                            Image = string.Empty,
                            Status = ContentStatus.Pendiente,
                            CreatedAt = DateTime.UtcNow
                        });
                        result.JavsCreated++;
                    }
                }

                // Registrar actrices y vincularlas al JAV
                foreach (var actressName in actressNames)
                {
                    var existingActress = await actressJavRepository.GetActressByName(actressName);

                    int actressId;
                    if (existingActress != null)
                    {
                        actressId = existingActress.Id;
                    }
                    else
                    {
                        actressId = await actressJavRepository.CreateActress(new ActressJav
                        {
                            Name = actressName,
                            CreatedAt = DateTime.UtcNow
                        });
                        result.ActressesCreated++;
                    }

                    if (javId.HasValue)
                    {
                        await javRepository.AddActressToJav(javId.Value, actressId);
                        result.LinksCreated++;
                    }
                }
            }

            return result;
        }
    }
}

public class BulkImportJavResult
{
    public int JavsCreated { get; set; }
    public int ActressesCreated { get; set; }
    public int LinksCreated { get; set; }
}
