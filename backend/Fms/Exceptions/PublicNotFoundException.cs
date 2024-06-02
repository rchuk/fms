namespace Fms.Exceptions;

[Serializable]
public class PublicNotFoundException : PublicException
{
    public PublicNotFoundException() {}
    public PublicNotFoundException(string description) : base(description) {} 
}