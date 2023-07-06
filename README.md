# CraftDemoV2

This console application is used to create/update a FreshDesk contact from an imputted GitHub username using their respective APIs. 



# How to Run

1. Clone the repository or download the source code.

2. Open the solution in Visual Studio or navigate to the project directory using the command line.

3. The application uses Enviromental variables for the FreshDesk API key, GitHub Token and FreshDesk Domain. The domain is entered from the console and it is set as an Enviromental variable as well.

4. For the application to run you will have to have a valid FreshDesk API key and FreshDesk Domain. If you don't use a GitHub Token the applicaiton will still work by getting only the public information from the Git User.

5. Currently the application works with a SQL database. If you wish for it to work you will need to go to the CraftDemoV2.Data project/Configation folder/DbContextConfigarion class and change the connection string

6. If you wish to run the application without a database you will need to comment out line 61(AddUser function) nad 70(UpdateUser function) in the MainService class in the CraftDemoV2.Services porject

7. After you decide how to use the application you can build it and run it.

8. You will be prompted to enter a FreshDesk domain name and a GitHub username.

9. The application will retrieve the GitHub user information, create a FreshDesk contact, and update the contact if it already exists.

10. If any errors occurenred you will be able to see them in the console 


# Project Structure

- `CraftDemoV2`: This is the StartUp project. It contains only one class called "StartUp", in it you enter the needed variables to run the bussiness logic of the application.

- `CraftDemoV2.API.RequestModels`: This porject contains the data models(classes) that are used to create the bodies for API request.

- `CraftDemoV2.API.ResponseModels`: This project contains the data models(classes) that are used to created object from the JSON strings that we get from API calls.

- `CraftDemoV2.Common`: This project contains static information that can be used in all other porjects in the application. Currently it contains onlt the API limits and the database validations

- `CraftDemoV2.Data`: This project contains the configuations settings for the database and the DbContext used to create it. 

- `CraftDemoV2.Data.Models`: This project is used to create database models(classes) for the object structure used to send and receive data from the database.

- `CraftDemoV2.Services`: This project contains the bussiness logic of the application. Here you can add diffrent services for part of the application. Current they are 3 main service types:

1. API Services: Contains the services used for the FreshDesk and GitHub API logic. 

2. Main Service: Contains the logic that combines the services from the freshdesk and API logic

3. Database Services: Contains the logic used to manupulate and fetch data from the database

This project also contains the connfiguartion class used for the DI(Dependency Injection) used for the application.


# Unit Tests

 The units tests are seperated into 2 folders
 
 1. `GitHub Unit tests`: Here you can find the unit tests for the functions used in the GitHubAPiService class.
 
 2. `FreshDesk Unit tests`: Here you can find the unit tests for the function used in the FreshDeskAPiService class.
	
	
The unit tests are created with NUnit and Mock. They cover only the functionality of the functions in the GitHub and FreshDesk API services. No integrations tests have been created for the MainService logic.
You can run all tests from IDE you are using.

#Contributing

Contributions to CraftDemoV2 are welcome! If you'd like to contribute, please follow these steps:

    Fork the repository.

    Create a new branch for your feature or bug fix:

   `bash`: git checkout -b feature/your-feature-name

    Make your changes and commit them.

    Push your changes to your forked repository.

    Submit a pull request to the main branch of the original repository.

Please ensure that your code adheres to the existing code style and conventions. Include tests if applicable and provide a clear description of your changes in the pull request.


