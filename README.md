# DziennikBaltycki

Used technologies: c#, .net core, postman

## Need to

It is necessary to instal all requirement libraries 

You should create your own database in SQL Server and change appsettings.json

```
"ConnectionStrings": {
    "Sql": "Data Source =.\\SQLEXPRESS; Initial Catalog = catalog; Integrated Security = True"
  },
  "GoogleAuthentication": {
    "ClientID": "client",
    "ClientSecret": "secret"
  }
  ```
  
## Postman test collection

I added sample postman requests into PostmanRequestCollection for simple testing

## Short Presentation

* Response include the best offers prices per meter from 9 cities

![alt text](https://media.giphy.com/media/Y1pIb3UDxf9N6XpnTW/giphy.gif)

* Request return all entries from database

![alt text](https://media.giphy.com/media/mEyvZyuZrZCMX5iqLk/giphy.gif)

* Requset returns limited records 

![alt text](https://media.giphy.com/media/jSWUKL7vRlJX8DCQZ1/giphy.gif)

* Request load page into database

![alt text](https://media.giphy.com/media/KG5KcoWbUKM78WD53f/giphy.gif)

* Request return single entry

![alt text](https://media.giphy.com/media/da6MmVryQMtuDJRzFN/giphy.gif)

* Request try update record in database but User need permission to do it by google authentication so response contain google authentication page

![alt text](https://media.giphy.com/media/cM22NDe3CDJSQhm3jv/giphy.gif)

* After this request log is added to database with current time and content 

![alt text](https://media.giphy.com/media/f4DY7UNr5Xb3LkWOQ8/giphy.gif)



