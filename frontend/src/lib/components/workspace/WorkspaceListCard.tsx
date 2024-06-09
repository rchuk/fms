"use client";

type WorkspaceListCardProps = {
  id: number,
  name: string
}

export default function WorkspaceListCard(props: WorkspaceListCardProps) {
  return (
    <div key={props.id}>
      {props.name}
    </div>
  );
}
