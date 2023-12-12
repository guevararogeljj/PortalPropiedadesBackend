namespace Contracts.Request;

public class PropertiesFilterDto
{
    public int pageNumber { get; set; }
	public int pageSize { get; set; }
    public int? propertyType { get; set; } = null;
    public int? state { get; set; } = null;
    public int? municipality { get; set; } = null;
    public decimal?  price { get; set; } = null;
    public int? rooms { get; set; } = null;
    public int? bathrooms { get; set; } = null;
    public int? proceduralStage { get; set; } = null;
    public bool oportunity { get; set; }
    public bool isCarrusel { get; set; }
}