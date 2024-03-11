# JSON To Template Library
## About the Project
This project is the Libary component of the JsonTranform Solution.  It house a single service and its interface.  The service contains all the functions and procedures that transform a JsonElement to a specified template string.
The code requires that the Template has a the **<template-row>** start tags and the **</template-row>** tag and field position are indicated by **<field-__fieldname__>** tag without any closing tag.

## Building the Test Component 
-Open *Visual Studio Command Prompt* with Administrator Privilage (to ensure you wont have issues)
-On the Command Prompt, change your current drive by typeing the driver letter and press enter
-Change directory by usine "**cd <codepath>\JsonTransformLibrary**", if the code was place in "code" directory then you must type "**cd \code\JsonTransformLibrary**".  You should see that the prompt will also shows directory your on.
-type "dotnet build --configuration Release" and press enter.  If you have encounter an "Invalid Commandline" error, you probably are using an ordinary command prompt, please retry all steps again

## Running the Test Component
This component requires the Test or the API component to run.

## Design Decisions
The service was created to contain all the processing required to transform a json element to a template formated string.

The service is automatically injected by the ASP.NET platform when the api controller is called.  Only 2 methods can be accessed outside the library and via an instance. 

All private functions is just rethrowing the error which is then capture by the main function. An **Application Error** type is produced with the **Inner Exception** set to have the original error.
The error is then thrown up to the Api for further processing.  Error logging is being handled by the Api Component for centralize configuration.




## About The Author
Name: ** Basti Sebastian **
Email: **jhune_sebastian@hotmail.com