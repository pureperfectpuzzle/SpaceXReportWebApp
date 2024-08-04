# SpaceXReportWebApp

## Introduction

This repository is to implement an ASP.Net core web application. The appliation has two parts: in the first part I use GraphQL to retrieve data from [the API](https://studio.apollographql.com/public/SpaceX-pxxbxen/variant/current/explorer); I use Microsoft Identity package to implement authentication and authorization for the web application using in-memory database as data store. Also I implement a report system to allow user create, read, update and delete report.

## Solution Structure

The solution is composed of three projects: WebClient, Repository and Data:

#### 1. WebClient

This is a MVC/RazorView/RazorPage asp.net core project. It provides web usr interface for user to interact with the application.

#### 2. Repository

This is a class library to provide data retrieval and writing functionalities for the web client.

#### 3. Data

This is a class library to provide data entity types used by both Repository and WebClient.

## External NuGet Packages

The projects depend on the following NuGet packages:
1. GraphQL.Client
2. GraphQL.Client.Serializer.SystemTextJson
3. Microsoft.AspNetCore.Identity.EntityFrameworkCore
4. Microsoft.EntityFrameworkCore.InMemory
5. modernhttpclient-updated

## How To Run The Application

1. Clone the repository to a local computer with .Net core 8.0 and Visual Studio 2022 installed;

2. Open the solution with Visual Studio 2.22 and build the solution;

3. Run the web application in Visual Studio if you don't want to install it on a web server;

## Functionalities

#### Authentication and Authorization

User must login to access functionalities provided by the web application. If user does not login and clicks on any menu except "Home", (s)he is brought to Log In page. After usr logs in successfully, (s)he is taken to the page intended to go before login automatically. If session is inactive for more than 5 minutes, session expires and user needs to login again.

I use role based authorization. There are two stock roles: Admins and Users. As the web application starts, it reads "UserAccounts" section in appsettings.json and creates roles and users set by software user. Right now I put two roles in the configuration file: Admins and Users. I also put two users: admin and emusk. User admin is in role Admins and emusk is in role Users. You can find out password for the users added in the configuration file. 

(**Note**: this is not a good way to store private information, such as password etc. Secret is a much better way for this purpose. I use appsettings.json to demonstrating purposes only.)

All data about User and Role are stored in the tables whose schema is created by Microsoft. EF is used by Microsoft Identity package and in-memory database stores these tables.

User must login first before (s)he can use any meaningful functionality. If user is in the role Users, (s)he can do these tasks:  
1. Retrieve Capsules, Launches, LaunchPad,  Upcoming Launches and Reports etc.
2. Register as a new user in the system and then use the profile to login the system. In this way the new user is a member of the Users role. If (s)he wants to be a member of Admins role, (s)he needs help from any user in the Admins role.

But if user is in the role Admins, (s)he will have much more power:
1. View details about a report, modify a report and delete a report.
2. View a list of users in the system.
3. View a list of roles in the system and assign/remove a user to/from a role.

#### Retrieve Data from External GraphQL REST Web Api

Capsule, Launch, LaunchPad and Upcoming Launch are from an external source which exposes data through GraphQL Rest Web api. The web application uses the web api to retrieve data and displays them.

#### CRUD SpaceX Reports

SpaceX reports are stored in an in-memory database too through EF DbContext. A report consists of three parts: Basic, Investigation and QA. Basic part contains such fields as report Id, title, short description and creator's Id; Investigation part contains investigator's Id, her/his comments and proposed solution; QA part contains QA's Id and her/his comments.

## TO-DO List

There are still quite few more tasks that can be done for the projects. At least I think of the following:

1. Add dirty prompt for the objects;
2. Add visual cue for long running tasks;
3. Add test projects;
4. Definitely it needs more styling. 