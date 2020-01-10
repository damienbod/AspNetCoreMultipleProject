# ASP.NET Core MVC with Entity Framework Core


https://github.com/damienbod/AspNetCoreMultipleProject


|                           | Build                                                                                                                                                             |       
| ------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| .NET Core                 | [![Build status](https://ci.appveyor.com/api/projects/status/97eaoaw0sw5lpefs?svg=true)](https://ci.appveyor.com/project/damienbod/aspnetcoremultipleproject)  |



<strong>Posts in this series:</strong>
<ul>	
    <li><a href="https://damienbod.com/2016/01/07/experiments-with-entity-framework-7-and-asp-net-5-mvc-6/">Experiments with Entity Framework Core and ASP.NET Core MVC</a></li>
	<li><a href="https://damienbod.com/2016/01/11/asp-net-5-with-postgresql-and-entity-framework-7/">ASP.NET Core with PostgreSQL and Entity Framework Core</a></li>
	<li><a href="https://damienbod.com/2016/08/26/asp-net-core-1-0-with-mysql-and-entity-framework-core/">ASP.NET Core with MySQL and Entity Framework Core</a></li>
</ul>


## History

2020-01-10 Updated to .NET Core 3.1

2019-03-03 Updated NuGet packages

2018-12-04 Updated to .NET Core 2.2

2018-11-28 Updated to .NET Core 2.2 preview

2018-11-25 Updated NuGet packages, async everything, added swagger

2018-06-16 Updated to .NET Core 2.1

## Setup

### MS SQL Server

dotnet restore

dotnet ef migrations add microsoftSql --context DomainModelMsSqlServerContext

dotnet ef database update --context DomainModelMsSqlServerContext

-or-

Add-Migration "microsoftSql" -c DomainModelMsSqlServerContext  

Update-Database -c DomainModelMsSqlServerContext

### SQLite 

dotnet restore

dotnet ef migrations add sqliteMigration --context DomainModelSqliteContext

dotnet ef database update --context DomainModelSqliteContext

-or-

Add-Migration "sqliteMigration" -c DomainModelSqliteContext  

Update-Database -c DomainModelSqliteContext

### PostGreSQL 

dotnet restore

dotnet ef migrations add postgresqlMigration --context DomainModelPostgreSqlContext

dotnet ef database update --context DomainModelPostgreSqlContext

-or-

Add-Migration "postgresqlMigration" -c DomainModelPostgreSqlContext  

Update-Database -c DomainModelPostgreSqlContext

### MySQL 

dotnet restore

dotnet ef migrations add mySqlMigration --context DomainModelMySqlContext

dotnet ef database update --context DomainModelMySqlContext

## Testing

https://localhost:44388/api/dataeventrecords
Context-Type: application/json

{
  "DataEventRecordId":0,
  "Name":"Funny data more",
  "Description":"no",
  "Timestamp":"2015-12-27T08:31:35Z",
  "SourceInfo":
  { 
    "SourceInfoId":0,
    "Name":"Beauty",
    "Description":"first Source",
    "Timestamp":"2015-12-23T08:31:35+01:00",
    "DataEventRecords":[]
  }, 
  "SourceInfoId": 0
}

### GET

http://localhost:44388/api/dataeventrecords/SourceInfos?withChildren=true