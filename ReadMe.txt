
1) Swagger Is Enabled


2) Serilog And Http Logging Is Enabled


3) HTTPS Dockerize Is Enabled 

Test Docker With This APIs
- https://localhost:5013/Item/GetAllAsync
- https://localhost:5013/Item/GetAsync/{id}
- https://localhost:5013/Item/PostAsync

-------------------------------------------------------------------------------------------------------------------------------------------


 To Enable HTTPS Dockerize ==>

A) In Common Path PowerShell 
dotnet dev-certs https --clean
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\todo.pfx -p pa55w0rd
dotnet dev-certs https --trust


B) Right Click On Project ---> Choose Containers Orchestrator Support (By Linux Type)


C) In PowerShell Go To ToDoApp.API Directory ----> dotnet user-secrets set "Kestrel:Certificates:Development:Password" "pa55w0rd"

