Shopping Lists
==============

A real-time shopping lists web application.

Share a shopping list with friends then go shopping. Work together while collecting items from the list. This application lets you know in real-time which items you or your friends have already picked (placed in your real world basket or cart) and which items are still outstanding.

Designed for use on mobile devices.

![alt tag](https://raw.github.com/iandhall/shopping-lists/master/ShoppingLists.Web/Content/Images/shoppinglist.jpg)

Features
--------
Real-time updates when a user picks, unpicks, adds or removes items.

Permissions management: User can state which actions other users can perform on the shared shopping list.

Real-time display of the other users that are viewing the shared shopping list.

Requirements
------------
__Client:__

* HTML5 capable browser (WebSockets support recommended).

__Server:__

* .NET Framework 4.5.1
* IIS or IIS Express (WebSockets support recommended).
* SQL Server 2012 Express LocalDB.

__Dev Environment:__

* Visual Studio 2013.

Starting the Application
------------------------
* Build the solution to add NuGet packages.
* For best performance, run the project without debugging and using the Release configuration.
* Display the main page by going to the following URL in a browser:

	[http://localhost:49171/](http://localhost:49171/)

* Register an account then create a shopping list!
	
Technologies Used
-----------------
ASP.NET MVC 5

SignalR

Entity Framework

SQL Server Data Tools (Used in preference to EF Code First Migrations)

Bootstrap

Knockout

LightInject

Visual Studio Testing Tools

Moq

Dapper.NET (Used in unit tests)

Architectural Features
----------------------
Layered design separating the concerns of data access, business rules and UI presentation:

* Data Access Layer - Accesses the database. Provides Repository classes for the Business layer to use.

* Business Layer - Applies business transactional rules. Unconcerned with how the data gets persisted. Provides Service classes for the Web UI to use.

* Web UI - Only concerned with web presentational logic. Converts entities to view models.

MVVM: JSON view models. Knockout.js. AJAX and SignalR.

Bootstrap modal popup forms.

Responsive display adjusts to small screens.

Custom user account persistence.

Knockout form validation.

Service error reporting on the client.

Transactions and database connections handled by UnitOfWork.

Attributes used to apply UnitOfWork to hub methods and controller actions.