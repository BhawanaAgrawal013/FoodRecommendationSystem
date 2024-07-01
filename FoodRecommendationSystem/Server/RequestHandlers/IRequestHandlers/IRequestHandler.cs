namespace Server.RequestHandlers
{
    public interface IRequestHandler<T>
    {
        string HandleRequest(string request);
    }
}