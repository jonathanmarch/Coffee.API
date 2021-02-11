## Setting Up The Database:

Create new SQL database called `Coffee-Api`

If you are not using the default data source `localhost\\SQLEXPRESS`
1. Edit `appsettings.json` -> `DefaultConnectionString` change to your desired default data source.

To create the tables using the SQL Server Data Tools (SSDT) Project

1. Right click on `Coffee-Api.Database` project.
2. Select `Publish` form the context menu.
3. Click `Edit` the buttom next to `Target database connection:` and select the database you created above.
4. Click `Publish`

This will the create all the required tables to run the API.

You *may* need to install SQL Server Data Tools (SSDT) on your Visual Studio.

Steps for Visual Studio 2019: https://i.stack.imgur.com/MgAtU.png

## Running The Integration Tests:

If you are not using the default data source `localhost\\SQLEXPRESS`
1. Edit `appsettings.json` -> `DefaultConnectionString` change to your desired default data source.

Use your favorite test runner :) I prefer ReSharper test runner.

Before every collection of tests are ran it will create a random database with a name such as `Coffee-Api-Test-lkldfjfdjs` and will automatically create the tables and seed the data.

After every collection of tests are completed it will clean up and drop the randomly generated database name.