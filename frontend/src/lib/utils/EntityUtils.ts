
export type EntityId = number | string;

export interface BaseEntity<IdT extends EntityId> {
  id: IdT
}

export function findEntity<EntityT extends BaseEntity<IdT>, IdT extends EntityId>
(list: EntityT[] | null, id: IdT): EntityT | null {
  return list?.find(entity => entity.id == id) ?? null;
}

export interface ListResponse<EntityT> {
  totalCount: number,
  items: EntityT[]
}
