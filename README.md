### Current learning path

* [Section 51](https://www.youtube.com/watch?v=qDUS8ocavBU)

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