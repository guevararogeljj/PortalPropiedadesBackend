namespace BusinessLogic.Models;
public class ResponseBase<T>
{
    public T data { get; set; }
    public int pageSize { get; set; }
    public int pageNumber { get; set; }
    public int totalPage { get; set; }
    public int totalRecords { get; set; }
    public bool Succes { get; set; }
    public ErrorBase Error { get; set; }
}
