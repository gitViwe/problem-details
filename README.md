<!-- ABOUT THE PROJECT -->
# Problem Details for HTTP APIs

For communicating the errors and exceptions to our API clients, we should specify a response format. In some cases, we would also like to let our users know what actually happened when something went wrong, instead of just telling them it was a 404 or 500 error.
If multiple clients consume our API, or if we need to use a selection of someone else’s APIs, it saves a lot of headaches to have this communication standardized.


<!-- GETTING STARTED -->
## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

### Prerequisites

Things you need to use the software and how to install them.
* [Visual Studio / Visual Studio Code](https://visualstudio.microsoft.com/)
* [.NET 7](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7/)
* [Docker](https://www.docker.com/)

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/gitViwe/problem-details.git
   ```
2. Run via Docker
   ```
   cd problem-details
   docker compose up -d
   ```
2. Run via Docker hub
   ```
   docker run -d -p 5299:80 --env ASPNETCORE_ENVIRONMENT=Development hubviwe/problem-detail.api:1.0.1
   ```
3. Build and run the API
   ```
   cd problem-details/src/api
   dotnet run
   ```

### Then navigate to [http://localhost:5299/swagger](http://localhost:5299/swagger)
