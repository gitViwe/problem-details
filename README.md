# Problem Details for HTTP APIs
For communicating the errors and exceptions to our API clients, we should specify a response format. In some cases, we would also like to let our users know what actually happened when something went wrong, instead of just telling them it was a 404 or 500 error.
If multiple clients consume our API, or if we need to use a selection of someone else’s APIs, it saves a lot of headaches to have this communication standardized.

## Getting Started

Get up and running quickly with docker
```
docker run -d -p 5299:80 --env ASPNETCORE_ENVIRONMENT=Development hubviwe/problem-detail.api:1.0.1
```
Then navigate to [http://localhost:5299/swagger](http://localhost:5299/swagger)
