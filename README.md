# PeopleSearch
People Search is a sample ASP.NET MVC and Entity Framework application to search people.

**PeopleSearch is a prototype solely designed for the purpose of demonstration. It is not intended to be used as a complete application. However, feel free to dig into the code and take some pieces for your project!**

## Hosted Demo
You can view and test the application [here](http://peoplesearch.usacloud.net/). The provided demo is hosted on SQL SERVER 2014 and a shared IIS 7 hosting server.

## Running The Code
To run the code, simply restore all packages in Visual Studio 2015 and hit run! The source code is using Entity Framework code first approach to generate the database on VS2015 LocalDb. If you wish to change the database server, you can modify web.config file.
The first time you run the application, EF seeds some sample records to display the purpose of the application. You are able to add more sample records. However, all of the new rows will be discarded after 12 hours.

## Dependencies
* Visual Studio 2015
* Microsoft.Net.Compilers version="1.3.2"
* EntityFramework version="6.1.3"
* LightInject version="4.1.5"
* LightInject.Mvc version="1.1.0"
* LightInject.Web version="1.1.0"
* AutoMapper version="5.2.0"
* Newtonsoft.Json version="9.0.1"
* Microsoft.AspNet.Mvc version="5.2.3"
* Microsoft.AspNet.Razor version="3.2.3"
* Microsoft.AspNet.Web.Optimization version="1.1.3"
* Microsoft.AspNet.WebPages version="3.2.3"
* Microsoft.Web.Infrastructure version="1.0.0.0"
* WebGrease version="1.6.0"s

-------

* jQuery version="3.1.1"
* jQuery.Validation version="1.16.0"
* Microsoft.jQuery.Unobtrusive.Validation version="3.2.3"
* bootstrap version="3.3.7
* Bootstrap.Datepicker version="1.6.4"
* FontAwesome version="4.7.0"
* datatables.net.bootstrap version="1.10.12"
* datatables.net.core version="1.10.12
* AlertifyJS version="1.8.0"
