# Finance Management System

## About

**FMS** is a web application developed as a university project to manage budgets.
Its aim is to provide users with tools to track income and expenses with ability to
filter by time period, user, etc. and categorize transactions. All data should also
be presented as graphs.

Backend is implemented using C# and ASP.NET Core,
while frontend uses TypeScript and React (Next.js).
Project also uses [openapi-generator](https://github.com/OpenAPITools/openapi-generator) to generate TS code from the
openapi specification file.

## Usage

### Development
- Install [Docker](https://www.docker.com/)
- Clone the repository using `git clone https://github.com/rchuk/fms.git`
- Run `docker compose up`

Website will be avaiable at `localhost:3000`.

## Screenshots

![Pie chart grouped by category](screenshots/pie_chart.png)
![Welcome page](screenshots/welcome_page.png)
![Pie chart grouped by user](screenshots/pie_chart_by_user.png)
![Workspace](screenshots/workspace.png)
![Transaction categories](screenshots/transaction_categories.png)
