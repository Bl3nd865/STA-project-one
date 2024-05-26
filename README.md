PostDummyData.cs
Siguron të dhëna të simuluara Post për testet, duke garantuar skenarë të parashikueshëm dhe të përsëritshëm testimi.

MockDbContextFactory.cs
Krijon një AppDbContext në kujtesën e brendshme për testim, duke lejuar teste të shpejta dhe të izoluara pa një bazë të dhënash reale.

TestPostService
This file contains unit tests for the PostService class using xUnit and an in-memory database context provided by MockDbContextFactory. It verifies CRUD operations (AddAsync, EditAsync, DeleteAsync, GetByIdAsync, and GetAllAsync) for both success and failure scenarios, ensuring the service's functionality and error handling are correct.

