# BlogRepository

## Project Description

ASP.NET course Razor Pages application showing the repository changes and dependency injection. You can use either an in-memory RAM repo or a JSON repo. JSON persists after application is stopped.


## Switching Repositories

The repository is configured through dependency injection in Program.cs. Instructions to switch between the two are as follows:

To use RAM repo enable (and comment out JSON line below): `builder.Services.AddSingleton<IBlogRepository, InMemoryBlogRepository>();`

To use the JSON repo enable (and comment out RAM line above): `builder.Services.AddSingleton<IBlogRepository, JsonBlogRepository>();`

## Screenshots

### RAM In-Memory Repo

![In-memory repository](screenshots/saved-RAM.png)

### Posts in JSON Repo before new post created

![JSON repository home page](screenshots/json-home.png)

### New Post to JSON Repo

![New JSON post](screenshots/new-json.png)

### JSON Repository - Persisted Data

![Persisted JSON data](screenshots/json-persisted.png)

