# DziennikBaltycki

It is necessary to instal HtmlAgilityPack

Dziennik Baltycki is my school project. It was created in three parts. First was DziennikBaltycki parser which collect data from https://dziennikbaltycki.pl/ogloszenia/12261,933,fm,pk.html and serialize them into json file. Second was DatabaseConnectiona and IntegrationApi. DatabaseConnection connects to database and create there two entities during migration , it also has databaseservice which returns data depends on user requests. IntegrationApi get request from user and return response to local provider like postman. It use rest style and   authorize policy to content access. Third part was UnitTest, I created them for DziennikBaltycki and Occasion. I used mocq to imitate object and checked both class by unit test.
