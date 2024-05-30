namespace Fms.Exceptions;

[Serializable]
public class PublicClientException : PublicException
{
    public PublicClientException() {}
    public PublicClientException(string description) : base(description) {} 
}