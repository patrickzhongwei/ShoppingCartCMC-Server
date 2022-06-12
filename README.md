# ShoppingCartCMC-Server

The ShoppingCartCMC project contains two parts: client side and server side. This is server side. The client side can be find from [ShoppingCartCMC-Client](https://github.com/patrickzhongwei/ShoppingCartCMC-Web). The server side project is supported by Visual Studio 2022 and .NET5.
This is a startup ASP.NET CORE (cross-platform) project template with security token service (STS) implementation.

## This application consists of:

* RESTful API Backend using ASP.NET Core 5 MVC Web API
* Database using Entity Framework Core
* Authentication based on OpenID Connect - IdentityServer4
* API Documentation using Swagger
* Client-side pages using Angular 12.2 and TypeScript
* Server-side data cache using Redis
* Real-time appliction with SignalR (websockedt)

## Installation

 Clone the Git Repository and edit with Visual Studio 2022. Double-click `ShoppingCartCMC-Server.sln` will open up a solution. Check the output window or status bar to know that the package/dependencies restore process is complete before launching your program for the first time.

## Database setup and configuration 
Find out `ShoppingCartCMC.Server.Shared\DB\ShoppingCartCmc.Identity.sql` and `ShoppingCartCmc.Trading.sql`, execute those two script files at Microsoft SQL Management Studio to create database.
Change connection string accordingly to point to right database at `appsettings.json`, for both project `ShoppingCartCMC.WebApi` and `ShoppingCartCMC.STS`

## Documentation

* `ShoppingCartCMC.WebApi`: RESTful API project
* `ShoppingCartCMC.STS`: security token service project
* `ShoppingCartCMC.Test`: unit test proeject
* `ShoppingCartCMC.Server.Shared`: server-side shared class library
* `ShoppingCartCMC.Shared`: client/server-side shared library


