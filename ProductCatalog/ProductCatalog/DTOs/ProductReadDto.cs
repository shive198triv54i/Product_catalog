namespace ProductCatalog.Api.DTOs
{
    public class ProductReadDto : ProductUpdateDto
    {
        public string? CategoryName { get; set; }
    }
}
