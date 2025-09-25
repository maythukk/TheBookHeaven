"TheBookHeaven" is a fully functional online bookstore built using ASP.NET Core MVC, Entity Framework Core, and Bootstrap. It provides a seamless experience for customers to browse and purchase books, while giving admins a complete management dashboard for users, books, and orders.
Note: This project uses an Azure SQL Database (12-month free trial). The database connection may not work in the future if the trial expires. Please configure your own Azure SQL database to run the project locally.

Features:
User Authentication & Profiles:
Secure registration and login with role-based access (Admin / Customer).
Customers can update profiles and change passwords.
Admin dashboard with management features.

Book Management (Admin):
Add, edit, and delete books.
Manage stock quantities (In Stock, Low Stock, Out of Stock).
Display stock status and recommend similar books when out of stock.

Shopping & Orders:
Customers can browse books, add to cart, and checkout.
Cart persists across login/logout sessions.
Customers can cancel orders in “Processing” status.
Admins can view/update order statuses and handle cancellations.

Search & Filters:
Search books by title, category, or price range.
Filter by Fiction / Non-Fiction.

Responsive UI:
Built with Bootstrap for mobile-friendly design.
Dynamic cart badge, inline username edit, and live username availability check.

Tech Stack:
Framework: ASP.NET Core MVC
Database: Azure SQL Database (Entity Framework Core)
Frontend: Bootstrap, JavaScript, Razor Views
Hosting: Azure Cloud (trial version during development)

Setup Instructions:
Clone the repo:
git clone https://github.com/your-username/TheBookHeaven.git
cd TheBookHeaven
Set up the database:
Create an Azure SQL Database (or use SQL Server locally).
Update your connection string in appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=your-db;User Id=your-username;Password=your-password;"
}

Do not commit your credentials to GitHub. Use user secrets or environment variables for sensitive data.
Apply migrations & run:
dotnet ef database update
dotnet run

License:
This project was created for educational purposes as part of the Web Programming (INFT3050) course at the University of Newcastle.
