"use client";

type WorkspaceListCardProps = {
  name: string
}

export default function WorkspaceListCard(props: WorkspaceListCardProps) {
  return (
    <div>
      {props.name}
    </div>
  );
}
