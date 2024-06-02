namespace Fms.Exceptions;

[Serializable]
public class PublicForbiddenException : PublicException
{
    public PublicForbiddenException() {}
    public PublicForbiddenException(string description) : base(description) {} 
}