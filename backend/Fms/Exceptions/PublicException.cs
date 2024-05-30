namespace Fms.Exceptions;

[Serializable]
public abstract class PublicException : Exception
{
    public string Description { get; } = "";
    
    protected PublicException() {}

    protected PublicException(string description) : base(description)
    {
        Description = description;
    }
}
