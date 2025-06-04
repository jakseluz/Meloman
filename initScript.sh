dotnet new mvc -o Meloman
cd Meloman
dotnet dev-certs https --trust
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator --version 7.0.1
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version 7.0.2
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.1
dotnet add package Microsoft.EntityFrameworkCore.SQLite --version 7.0.1
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 7.0.1
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.1

# for REST API swagger
dotnet add package Swashbuckle.AspNetCore

# for REST API JWT token authorization
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt


for model in "Artist" "Track" "User" "Category" "CategoryMark" "ArtistMark"
do
dotnet aspnet-codegenerator controller -name "${model}Controller" -m "$model" -dc Meloman.Data.MelomanContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries -sqlite
dotnet aspnet-codegenerator controller -name "${model}ApiController" -api -m "${model}" -dc MelomanContext --relativeFolderPath Controllers/Api -f
done

dotnet ef migrations add InitialCreate
dotnet ef database update