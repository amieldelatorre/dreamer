# dreamer

## Creating and Applying Database Migrations
May need to add an entry to the `backend/Dreamer.Api/Dreamer.Api.csproj`
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.6">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  ...
</ItemGroup>
```


```bash
cd backend/Dreamer.DataAccess
dotnet ef migrations add InitialMigration --project ./Dreamer.DataAccess.csproj --startup-project ../Dreamer.Api/Dreamer.Api.csproj
dotnet ef database update --project ./Dreamer.DataAccess.csproj --startup-project ../Dreamer.Api/Dreamer.Api.csproj
```