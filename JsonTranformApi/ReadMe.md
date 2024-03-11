# JSON To Template API
## About the Project
This project is the Web API component of the JsonTranform Solution.  Compose of a single controller injected with tranformation component.  This can be run under docker linux distro and has no database dependency.

The API require both the template parameter and a JSON array in order to work properly. Otherwise respose will show error messages.  Please check the swagger for further information.

## Building the Test Component 
-Open *Visual Studio Command Prompt* with Administrator Privilage (to ensure you wont have issues)
-On the Command Prompt, change your current drive by typeing the driver letter and press enter
-Change directory by usine "**cd <codepath>\JsonTranformApi**", if the code was place in "code" directory then you must type "**cd \code\JsonTranformApi**".  You should see that the prompt will also shows directory your on.
-type "dotnet build --configuration Release" and press enter.  If you have encounter an "Invalid Commandline" error, you probably are using an ordinary command prompt, please retry all steps again

## Building API Container
A Docker file was created by Visual studio to facilitate the creation of the docker container.
-Open *Visual Studio Command Prompt* with Administrator Privilage (to ensure you wont have issues)
-On the Command Prompt, change your current drive by typeing the driver letter and press enter
-Change directory by usine "**cd <codepath>\JsonTranformApi**", if the code was place in "code" directory then you must type "**cd \code\JsonTranformApi**".  You should see that the prompt will also shows directory your on.
-type "docker build -t **<tag>** ." and press enter.  If you have encounter an "Invalid Commandline" error, you probably are using an ordinary command prompt, please retry all steps again.  

## Design Decisions
The service was created to contain all the processing required to transform a json element to a template formated string.

The service is automatically injected by the ASP.NET platform when the api controller is called.  Only 2 methods can be accessed outside the library and via an instance. 

All private functions is just rethrowing the error which is then capture by the main function. An **Application Error** type is produced with the **Inner Exception** set to have the original error.
The error is then thrown up to the Api for further processing.  Error logging is being handled by the Api Component for centralize configuration.

## About The Author
Name: ** Basti Sebastian **
Email: **jhune_sebastian@hotmail.com