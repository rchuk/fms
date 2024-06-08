using System.Runtime.Serialization;

namespace Fms.Dtos;

public enum SortDirectionDto
{
    [EnumMember(Value = "ASC")]
    Ascending,
    [EnumMember(Value = "DESC")]
    Descending
}