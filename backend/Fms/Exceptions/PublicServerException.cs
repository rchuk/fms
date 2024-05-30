namespace Fms.Exceptions;

[Serializable]
public class PublicServerException : PublicException
{
    public PublicServerException() {}
    public PublicServerException(string description) : base(description) {} 
}