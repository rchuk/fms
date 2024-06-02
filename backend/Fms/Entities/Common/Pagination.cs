namespace Fms.Entities.Common;

public readonly struct Pagination(int? offset, int? limit)
{
    public int Offset { get; } = offset ?? 0;
    public int Limit { get; } = limit ?? 10;
}