@echo off
cd ..\TaskAPI.Data
CALL rmdir Migrations /s /Q
cd ..\TaskAPI
CALL dotnet ef database drop --force
CALL dotnet ef migrations add InitialCreate --project ..\TaskAPI.Data\TaskAPI.Data.csproj
CALL dotnet ef database update
cd ..