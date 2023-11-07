
--H�mta alla produkter med deras namn, pris och kategori namn. Sortera p� kategori namn och sen produkt namn
SELECT ProductName, UnitPrice, CategoryName FROM Products
JOIN Categories ON Products.CategoryID = Categories.CategoryID
ORDER BY CategoryName, ProductName

--H�mta alla kunder och antal ordrar de gjort. Sortera fallande p� antal ordrar.
SELECT CompanyName, COUNT(OrderID) AS Orders FROM Customers
JOIN Orders ON Customers.CustomerID = Orders.CustomerID
GROUP BY CompanyName
ORDER BY Orders DESC

--H�mta alla anst�llda tillsammans med territorie de har hand om (EmployeeTerritories och Territories tabellerna)
SELECT FirstName + ' ' + LastName AS Employees, TerritoryDescription AS Territory FROM Employees
JOIN EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID
JOIN Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID



