# JSON To Template Test
## About the Project
This project is the unit test component of the JsonTranform Solution.  The test assumes that the **Assets** folder under is alway included/created on the output directory.
The **Assets** directory contains **template.html** file and **TestRecords.json**

## Building the Test Component 
-Open *Visual Studio Command Prompt* with Administrator Privilage (to ensure you wont have issues)
-On the Command Prompt, change your current drive by typeing the driver letter and press enter
-Change directory by usine "**cd <codepath>\JsonTransformTest**", if the code was place in "code" directory then you must type "**cd \code\JsonTransformTest**".  You should see that the prompt will also shows directory your on.
-type "dotnet build --configuration Release" and press enter.  If you have encounter an "Invalid Commandline" error, you probably are using an ordinary command prompt, please retry all steps again

## Running the Test Component
-Open *Visual Studio Command Prompt* with Administrator Privilage (to ensure you wont have issues)
-On the Command Prompt, change your current drive by typeing the driver letter and press enter
-Change directory by usine "**cd <codepath>\JsonTransformTest\bin\Release\net8.0**", if the code was place in "code" directory then you must type "**cd \code\JsonTransformTest**".  You should see that the prompt will also shows directory your on.
-Type "vstest.console.exe jsontransformtest.dll" to run the test.  Test results will automatically be displayed as the test runs.

## Design Decisions
To facilitate 1 time loading of the json and template file, a **Setup** procedure was created to load both files.  Transformation was also done on the procedure to align the json array with the API json array.


## About The Author
Name: ** Basti Sebastian **
Email: **jhune_sebastian@hotmail.com

