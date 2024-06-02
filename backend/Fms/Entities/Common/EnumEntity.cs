using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fms.Entities.Common;

public abstract class EnumEntity<TEnum>
    where TEnum : struct, Enum
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required, MaxLength(127)]
    public string Name { get; set; } = null!;

    protected EnumEntity()
    {
        
    }

    public static IEnumerable<TSelf> GetVariants<TSelf>() 
        where TSelf : EnumEntity<TEnum>, new()
    {
        return Enum.GetValues<TEnum>()
            .Select(e => new TSelf {
                Name = e.ToString().ToUpper()
            });
    }

    public EnumEntity(TEnum enumVariant)
    {
        Name = enumVariant.ToString().ToUpper();
    }
    
    public TEnum ToEnum()
    {
        return Enum.Parse<TEnum>(Name, true);
    }
}
