using Fms.Entities.Common;
using Fms.Entities.Enums;

namespace Fms.Entities;

public class OrganizationRoleEntity : EnumEntity<OrganizationRole>
{
    public OrganizationRoleEntity() {}
    public OrganizationRoleEntity(OrganizationRole enumVariant) : base(enumVariant) {}
}