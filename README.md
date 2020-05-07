### Current learning path

* [Section 51](https://www.youtube.com/watch?v=qDUS8ocavBU)

### Database for Docker setup

- Pull && run Mssql image

      $ docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD=Strong@@PWD -p 1401:1433 --name mssql -d mcr.microsoft.com/mssql/server:2017-latest

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