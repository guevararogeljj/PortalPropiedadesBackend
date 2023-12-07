namespace DataSource.Contracts
{
    public interface IParametersRepository
    {
        T GetParameter<T>(string groupname, string name);
        Dictionary<string, object> GetParameters(string groupname);

        List<T> GetParameters<T>(string groupname, string name);
    }
}
