# Shopping List 4 
![ShoppingList4](https://github.com/szotrab1337/ShoppingList4/actions/workflows/ci.yml/badge.svg) ![ShoppingList4](https://github.com/szotrab1337/ShoppingList4/actions/workflows/cd.yml/badge.svg)

A mobile application created for the mobile environment, allowing you to easily manage your shopping lists.

## General Information
- Backend - REST API made in .NET 6
- Frontend - app made in MAUI .NET 6 
- Web Api and database were published on Azure
- CI and CD pipelines were used in the repository

## Technologies Used
### Web API
- Auto Mapper
- Fluent Validation
- Entity Framework
- Serilog
- Swagger

### MAUI App
- Community Toolkit MVVM
- Community Toolkit MAUI
- Newtonsoft.Json

## Features

- Authorization using JWT
- Adding / editing shopping lists
- Adding / editing items to buy in the selected shopping list
- Marking an item as purchased

## Screenshots
![List of shopping lists](https://github.com/szotrab1337/ShoppingList4/assets/61224345/fca8b4a1-3b27-4305-909c-778746778a7d)
![New shopping list](https://github.com/szotrab1337/ShoppingList4/assets/61224345/67265c6d-705f-4a60-b888-dbbdf683b734)
![List of shopping lists 2](https://github.com/szotrab1337/ShoppingList4/assets/61224345/6c384744-9f4b-4e7e-bf3c-f70453f62374)
![List of shopping lists 3](https://github.com/szotrab1337/ShoppingList4/assets/61224345/cbdb0c25-c19b-4e44-b1df-a712106e42db)
![Empty list of entries](https://github.com/szotrab1337/ShoppingList4/assets/61224345/8d936f98-66e1-450d-b7ef-0b899606a903)
![Empty list of entries 2](https://github.com/szotrab1337/ShoppingList4/assets/61224345/568d971a-f12d-44d3-a9db-25ba90bdd5e3)
![Empty list of entries 3](https://github.com/szotrab1337/ShoppingList4/assets/61224345/b129b0bb-f4a5-408d-a704-054c42f6b87e)
![Empty list of entries 4](https://github.com/szotrab1337/ShoppingList4/assets/61224345/da2ef064-9318-459e-a087-59eb0274a44c)
![Login page](https://github.com/szotrab1337/ShoppingList4/assets/61224345/f2fae44a-45b4-41de-a7dc-c6067a2c5feb)

## Setup
Login to the mobile application is not available to third parties. This is justified by limiting the consumption of resources by the database and API.

## Room for Improvement
- Unit tests for both applications need to be added to the project.
- The application can be published in the Google Store. You can then also add automatic application publishing using GitHub Actions.
