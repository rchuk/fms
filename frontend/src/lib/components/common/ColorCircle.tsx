import CircleIcon from '@mui/icons-material/Circle';

type ColorCircleProps = {
  color?: string | null
}

export default function ColorCircle(props: ColorCircleProps) {
  const defaultColor: string = "#AAAAAA";

  return <CircleIcon
    style={{ color: props.color != null ? `#${props.color}` : defaultColor }}
  />;
}
