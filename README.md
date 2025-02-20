
# FirstStep | Job Matching Platform

FirstStep is a job matching platform built with ASP.NET Core 8.0, utilizing Entity Framework for data management. This platform provides an efficient way for job seekers to find relevant jobs and for companies to manage applications. The system offers various features such as job posting, job search, CV uploads, user and company profile management, interview scheduling, and job suggestions based on skills and distance.

## Table of Contents

-   [Features](#features)
-   [Technologies Used](#technologies-used)
-   [Requirements](#requirements)
-   [Getting Started](#getting-started)
    -   [Clone the Repository](#clone-the-repository)
    -   [Build and Run](#build-and-run)
-   [Azure Services](#azure-services)
-   [License](#license)

## Features

-   Post and apply for jobs
-   Upload CVs and maintain user profiles
-   Create and manage company profiles with sub-user roles
-   Job search with filtering and distance calculation
-   Interview scheduling with available time slots for both companies and job seekers
-   Job recommendations based on user skills and proximity to the job location

## Technologies Used

-   ASP.NET Core 8.0
-   Entity Framework Core
-   MS Azure (Azure Blob Storage, Email Service, Azure App Service)
-   Visual Studio
-   MS SQL Server

## Requirements

-   Visual Studio 2022 or later
-   .NET 8.0 SDK
-   MS SQL Server or SQL Express
-   Azure Storage Account for Blob Storage
-   Azure App Service for deployment
-   Azure Email Service for notifications
-   Git

## Getting Started

### Clone the Repository

1.  Open a terminal window or Git Bash.
    
2.  Run the following command to clone the repository:
    
    `git clone https://github.com/your-username/firststep-job-matching-platform.git` 
    
3.  Navigate into the project folder:
    
    `cd firststep-job-matching-platform` 
    

### Build and Run

1.  Open the solution in **Visual Studio**.
    
2.  Restore the NuGet packages by right-clicking on the solution and selecting **Restore NuGet Packages**.
    
3.  Update the **appsettings.json** file to include your database connection strings and Azure service credentials:
  
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FirstStepDB;Trusted_Connection=True;"
  },
  "AzureBlobStorage": {
    "BlobServiceEndpoint": "https://your-storage-account.blob.core.windows.net/",
    "StorageAccountName": "your-storage-account",
    "StorageAccountKey": "your-storage-account-key"
  },
  "AzureEmailService": {
    "ApiKey": "your-azure-email-service-api-key"
  }
}
```
    
4.  Build the solution by selecting **Build > Build Solution** or pressing `Ctrl + Shift + B`.
    
5.  Run the application by pressing `F5` or selecting **Debug > Start Debugging**.
    

## Azure Services

This project uses the following Azure services:

-   **Azure Blob Storage** is used to store job posts and CVs.
-   **Email Service** for sending notifications regarding job applications and interview scheduling.
-   **Azure App Service** is used to host the application in a scalable environment.

Ensure you have the necessary Azure resources and update the `appsettings.json` file with your credentials.

## License

This project is licensed under the MIT License.
Developed by Team-Do while UOM
