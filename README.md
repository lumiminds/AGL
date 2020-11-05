![Build](https://github.com/lumiminds/AGL/workflows/Build%20Pipeline/badge.svg?branch=master)

# AGL Programming Challenge

A complete sample application for AGL programming challenge. 

## Requirement

An application that consumes data from an API endpoint and output a list of all the cats in alphabetical order under a heading of the gender of their owner. [More details](http://agl-developer-test.azurewebsites.net/)

### Example

Male

 * Angel
 * Molly
 * Tigger

Female

 * Gizmo
 * Jasper
 
### Prerequisites
 - [Visual Studio 2019 for Mac](https://docs.microsoft.com/en-us/visualstudio/mac/installation?view=vsmac-2019)

### Build and Test
- Download the copy of the project or clone this repo.
- Open the solution file in your preferred IDE or in Visual Studio for Mac 2019
- Please run `dotnet restore` command from Package Manager Console or Terminal if you are opening in your own IDE.
- Please run `dotnet test` command to run the test cases from Package Manager Console or Terminal.

### Running locally
The solution can be run locally for development purposes. 
- Open the `AGL.sln` file and set `AGL.App` as startup project.
- Hit `F5` to debug it locally.
- Browse to the Swagger UI page for Web App to test the functionality.
```
https://localhost:<port>/swagger
```

## Resources

* Swagger : https://swagger.io/
* XUnit : https://xunit.net/

## License ##
Code released under the <a href="https://github.com/lumiminds/AGL/blob/master/LICENSE" target="_blank"> MIT license</a>
