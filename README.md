### Current learning path

* [Section 51](https://www.youtube.com/watch?v=qDUS8ocavBU)

### Database for Docker setup

- Pull && run Mssql image

      $ docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD=Admin@@1234 -p 1401:1433 --name mssql -v "d:/workspaces/database/data":/var/opt/mssql/data -d mcr.microsoft.com/mssql/server:2017-latest

- Check Mssql running status

      $ docker ps

- Set Connectionstrings

      server=127.0.0.1,1401;database=EmployeeDB;user id=sa;password=Strong@@PWD


### Database migration

- Migration requirements

        > nuget install: Microsoft.EntityFrameworkCore.Tools(2.2.0)

- Migration process

        > View > Other Windows > Package Manager Console

        > Provides entity framework core help

          $ Get-Help About_Entityframeworkcore

        > Adds a help with new migration

          $ Get-Help Add-Migration

          $ Add-Migration

            Name: > InitialMigration

        > Updates the database to a specified migration

          $ Update-Database

### Database Add seed

      > View > Other Windows > Package Manager Console

- Add Seed Data
 
      $ Add-Migration SeedEmployeesTable

      $ Update-Database

- Alter Seed Data

      $ Add-Migration AlterEmployeesSeedData

      $ Update-Database

- Add-Migration | Remove-Migration

  1. before Update-Database

      $ Remove-Migration

  2. After Update-Database

      $ Update-Database [__EFMigrationsHistory_[targetValue]]

      $ Remove-Migration

  3. see: https://www.youtube.com/watch?v=MhvOKHUWgiY

  ### Packages

  1. NLog.Web.AspNetCore 4.8.2

  2. Microsoft.EntityFrameworkCore.Tools 2.2.0

  3. 

  ### Refs

  1. [Logging for CreateDefaultBuilder #Line156](https://github.com/dotnet/aspnetcore/blob/master/src/DefaultBuilder/src/WebHost.cs)

  2. [ASP.NET core tutorial for beginners](https://www.youtube.com/playlist?list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU)

  3. [AspNetCore Github SourceCode](https://github.com/dotnet/aspnetcore)
