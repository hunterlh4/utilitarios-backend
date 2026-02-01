using UtilitariosCore.Application.Features.Products.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using MediatR;
using QRCoder;
using System.Text;
using System.Xml.Linq;

namespace UtilitariosCore.Application.Features.Products.Actions;

public record GenerateQrProductByIdQuery(int Id) : IRequest<Result<QrProductDto>>
{
    internal sealed class Handler(IProductRepository productRepository) : IRequestHandler<GenerateQrProductByIdQuery, Result<QrProductDto>>
    {
        public async Task<Result<QrProductDto>> Handle(GenerateQrProductByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await productRepository.GetProductById(request.Id);

            if (item == null)
            {
                return Errors.NotFound($"Product with Id {request.Id} not found.");
            }

            if (string.IsNullOrWhiteSpace(item.Sku))
            {
                return Errors.BadRequest($"Product with Id {request.Id} does not have a SKU.");
            }

            var baseSvg = GenerateQR(item.Sku, item.Sku);

            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(baseSvg));

            return new QrProductDto
            {
                FileName = $"{item.Sku}-qr.svg",
                Base64 = $"data:image/svg+xml;base64,{base64}"
            };
        }

        private static string GenerateQR(string payload, string? label = null)
        {
            using var qrCodeData = QRCodeGenerator.GenerateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

            using var svgRenderer = new SvgQRCode(qrCodeData);

            string svgContent = svgRenderer.GetGraphic();

            if (string.IsNullOrWhiteSpace(label))
            {
                return svgContent;
            }

            XDocument svgDoc = XDocument.Parse(svgContent);

            if (svgDoc.Root is null) return svgContent;

            XElement svgRoot = svgDoc.Root;

            if (svgRoot is null) return svgContent;

            XNamespace svgNs = svgRoot.Name.Namespace;

            string viewBox = svgRoot.Attribute("viewBox")?.Value ?? string.Empty;

            if (string.IsNullOrWhiteSpace(viewBox)) return svgContent;

            var parts = viewBox.Split(' ');

            if (parts.Length != 4) return svgContent;

            if (!double.TryParse(parts[2], out double qrWidth) || !double.TryParse(parts[3], out double qrHeight))
            {
                return svgContent;
            }

            double fontSize = 2.5;
            double textY = qrHeight - 1;
            double textX = qrWidth / 2;

            XElement textElement = new(svgNs + "text",
                new XAttribute("x", textX),
                new XAttribute("y", textY),
                new XAttribute("font-size", fontSize),
                new XAttribute("text-anchor", "middle"),
                new XAttribute("fill", "black"),
                new XAttribute("font-weight", "bold"),
                new XAttribute("font-family", "Arial, sans-serif"),
                label
            );

            svgRoot.Add(textElement);

            return svgDoc.ToString();
        }
    }
}