# Installation Manual
## Prerequisites
Visual Studio 2017 with ASP.NET Core 2.0 or higher.

Current version of Windows/Linux/macOS.
## Installation
Before you can successfully run the applications, you need to set up the database.
The first thing you need to do is to decompress the zip file provided,
and then open our project solution in Visual Studio. When inside Visual Studio,
open Tools -> NuGet Package Manager -> Package Manager Console.
From within this console, you need to set the default project to RowcallBackend,
which will contain the migration files needed. When the default project is set,
run the following command: “update-database”, which will create the tables needed
 for the applications.



To populate the database with students, you need to go the the AdminWebApplication project and locate the startup.cs file. In the last method “Configure”, outcomment the last line of code, which will create some users in the database when you run the program. The users login will be userN@user.com, where the N will be a number from 0-49, and there will therefore be 50 students in the database. 

Once the database and its table have been created, go to “Solution -> Properties”,
 and a the properties window will pop up.



Make sure that the “Multiple startup projects” is checked, and put the action to start for all of the projects (except the RowcallBackend)
. This is important to do before running it, because the API’s needs to be
 runned before the first web application, since it will use the API’s. After this,
  you can now click on the green run button or press CTRL+F5 to run all of the
   applications. 

