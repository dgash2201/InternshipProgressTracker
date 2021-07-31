# InternshipProgressTracker

1. To build and run the application, you will need to install:
	* .NET 5.0 SDK
	* Microsoft SQL Server

2. You can change default admin data in appsettings.json

3. You need to set following secrets by using "dotnet user-secrets":
	* Admin:Password - password of the default admin
	* ServiceApiKey - secret for generating JWT
	* APPINSIGHTS_CONNECTIONSTRING - connection string for Application Insights

4. Before running the application you need to create database and apply migrations by using following command:\
	"Update-Database -Project InternshipProgressTracker" command