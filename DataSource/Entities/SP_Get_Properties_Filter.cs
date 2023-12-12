namespace DataSource.Entities;

public class SP_Get_Properties_Filter
{
    public Int64 RowNum { get; set; }
    public string Title { get; set; }
    public string Id { get; set; }
    public int CreditNumber { get; set; }
    public string Rooms { get; set; }
    public string Bathrooms { get; set; }
    public double ConstructionSize { get; set; }
    public double LotSize { get; set; }
    public decimal Price { get; set; }
    public string ParkingSpaces { get; set; }
    public string Settlement { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string pathFile { get; set; }
    public int TotalRecords { get; set; }
}