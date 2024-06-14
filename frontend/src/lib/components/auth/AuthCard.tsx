import { Box, Card, Link, Typography } from "@mui/material";

type AuthCardProps = {
  children: React.ReactNode
  header: string,
  otherOptionText: string,
  otherOptionLinkText: string,
  otherOptionLink: string
}

export default function AuthCard({
  children,
  header,
  otherOptionText,
  otherOptionLinkText,
  otherOptionLink
}: AuthCardProps) {
  return (
    <Card
        sx={{
          padding: "2rem 0",
          borderRadius: "2rem",
          display: "flex",
          flexDirection: "column",
          justifyContent: "space-evenly",
          alignItems: "center",
          width: "30vw",
          "@media (max-width: 600px)": {
            width: "80vw"
          }
        }}
      >
      <Typography 
        variant="h3"
      >
        {header}
      </Typography>
      <Box
        mt={6}
        pl={6}
        pr={6}
        width={"100%"}
        display={"flex"}
        flexDirection={"column"}
        alignItems={"center"}
      >
        {children}
      </Box>
      <Typography 
        mt={2}
        variant="body1"
      >
        {otherOptionText} <Link href={otherOptionLink}>{otherOptionLinkText}</Link>
      </Typography>
    </Card>
  )
}