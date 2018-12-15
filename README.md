# TaxiBookingSystem
Experimental Code for Taxi Booking System API 

Taxi Booking System API enables customer to books cabs via api calls, for details of available endpoints http://<hostname>:8080/swagger/#/

#How to use API - find demo video by name TaxiBookingSystemDemo.mp4

#How to run Application

	#Using DotnetCore SDK


		1. DotnetCoreSdk Installation

			Follow instructions here to install dotnetCore sdk https://www.microsoft.com/net/learn/get-started/linux/centos

			Once you have dotnetCore sdk installed, go to the baseDirectory run following commands

		2. Running dotnetcore commands

				dotnet restore - to restore nuget packages
				dotnet build - to build the solution
				dotnet test - to run test cases
				dotnet run - For ease of testing I am running my application on port 64000 , 
							 in case you want run on different port please change baseDirectory/TaxiBookingSystem/Properties/launchSettings.json
				For accessing endpoints using swagger http://<hostname>:64000/swagger/#/

	#Publishing application and using existing server for reverse proxy
	
		Here is an example of Hosting ASP.NET CORE with Linux on Apache https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-2.1&tabs=aspnetcore1x

		Copy publish aritfacts from folder baseDirectory/TaxiBookingSystem/bin/Release/netcoreapp1.1

		In above mentioned article instead of hellomvc.dll use TaxiBookingSystem.dll

		You can configure apache server to forward request on port 80/8080 to 64000.
		
#Packages used

	1. Kestrel for hosting asp.net application
	2. Microsoft.AspNetCore
	3. Microsoft.AspNetCore.Mvc
	4. Microsoft.Extensions.Logging
	5. Swashbuckle.AspNetCore - For api documentation
	6. Microsoft.NET.Test.Sdk - For testing framework
	