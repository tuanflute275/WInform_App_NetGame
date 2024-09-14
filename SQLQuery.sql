	CREATE DATABASE DB_Winform
GO

USE DB_Winform
GO

CREATE TABLE Account
(
	Id int PRIMARY KEY identity,
	UserName NVARCHAR(100) NOT NULL,	
	Password NVARCHAR(1000) NOT NULL DEFAULT 0,
	Status int default 1,
	Price float default 0,
	Type INT NOT NULL  DEFAULT 0 -- 1: admin && 0: staff
)
GO

CREATE TABLE Computer (
    ComputerID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Status BIT NOT NULL -- 0: Trống, 1: Đang sử dụng
);
GO

CREATE TABLE DrinkSnack (
    DrinkSnackID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);
GO

CREATE TABLE UsageLog (
    UsageLogID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT FOREIGN KEY REFERENCES Account(Id),
    ComputerID INT NOT NULL FOREIGN KEY REFERENCES Computer(ComputerID),
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL
);
GO

CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UsageLogID INT NOT NULL FOREIGN KEY REFERENCES UsageLog(UsageLogID),
    DrinkSnackID INT NOT NULL FOREIGN KEY REFERENCES DrinkSnack(DrinkSnackID),
    Quantity INT NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL -- Số tiền của từng món đã gọi
);
GO


INSERT INTO dbo.Account
        ( UserName ,
          PassWord ,
          Type
        )
VALUES  ( N'Admin' , -- UserName - nvarchar(100)
          N'12345678' , -- PassWord - nvarchar(1000)
          1  -- Type - int
        )
INSERT INTO dbo.Account
        ( UserName ,
          PassWord ,
          Type
        )
VALUES  ( N'User' , -- UserName - nvarchar(100)
          N'12345678' , -- PassWord - nvarchar(1000)
          0  -- Type - int
        )
GO

-- INSERT Account
DECLARE @i INT = 1
WHILE @i <= 24
BEGIN   
	INSERT INTO dbo.Account
			( UserName ,
			  PassWord ,
			  Type
			)
	VALUES  ( N'User ' + CAST(@i AS nvarchar(100)), 
			  N'12345678' , 
			  0  
			)
	SET @i = @i + 1
END
GO

-- INSERT COMPUTER
DECLARE @i INT = 1
WHILE @i <= 20
BEGIN
	INSERT INTO Computer (Name, Status) VALUES ('Máy '+ CAST(@i AS nvarchar(100)), 0)
	SET @i = @i + 1
END
GO

-- INSERT DrinkSnack
INSERT INTO DrinkSnack (Name, Price)
VALUES 
-- thêm nước uống
(N'Trà sữa', 25000),
(N'Cafe', 20000),
(N'Trà xanh không độ', 10000),
(N'Bò húc', 15000),
(N'Cocacola', 10000),
(N'Pepsi', 10000),
(N'Sting', 12000),
-- thêm đồ ăn
(N'Cơm rang thập cẩm', 35000),
(N'Cơm rang gà', 45000),
(N'Bánh mì', 20000),
(N'Mì tôm xúc xích', 15000),
(N'Mì tôm trứng', 20000),
(N'Gà rán', 35000),
(N'Khoai tây chiên', 20000),
(N'pizza', 100000),
(N'bim bim', 5000),
(N'đậu phộng', 8000),
(N'bắp rang bơ', 22000);
GO

-- Thêm dữ liệu ghi nhận sử dụng máy tính
INSERT INTO UsageLog (UserID,ComputerID, StartTime, EndTime, TotalAmount)
VALUES 
(1,1, '2024-09-10 08:00:00', '2024-09-10 10:00:00', 70000), -- Máy 1 sử dụng trong 2 giờ, tổng cộng 70.000đ
(2,2, '2024-12-09 09:30:00', '2024-12-09 12:00:00', 120000), -- Máy 2 sử dụng trong 2,5 giờ, tổng cộng 120.000đ
(3,3, '2024-08-10 13:00:00', '2024-08-10 15:30:00', 90000), -- Máy 3 sử dụng trong 2,5 giờ, tổng cộng 90.000đ
(4,4, '2024-10-11 08:00:00', '2024-10-11 15:30:00', 100000),
(5,5, '2024-12-11 09:00:00', '2024-12-11 12:30:00', 70000);
GO

-- Thêm dữ liệu ghi nhận các món đồ uống và ăn nhẹ đã sử dụng cho từng phiên truy cập máy
INSERT INTO Orders (UsageLogID, DrinkSnackID, Quantity, Amount)
VALUES 
-- Ghi nhận cho UsageLogID 1 (Máy 1)
(1, 1, 2, 50000), -- 2 cốc trà sữa, mỗi cốc 25.000đ, tổng 50.000đ
(1, 2, 1, 20000), -- 1 cốc Cafe, 20.000đ

-- Ghi nhận cho UsageLogID 2 (Máy 2)
(2, 2, 3, 60000), -- 3 cốc Cafe, mỗi cốc 20.000đ, tổng 60.000đ
(2, 10, 2, 40000), -- 2 chiếc bánh mì, mỗi chiếc 20.000đ, tổng 40.000đ

-- Ghi nhận cho UsageLogID 3 (Máy 3)
(3, 2, 1, 20000), -- 1 cốc cà phê, 20.000đ
(3, 8, 2, 70000), -- 2 Cơm rang thập cẩm, mỗi suất 35.000đ, tổng 70.000đ

-- Ghi nhận cho UsageLogID 4 (Máy 4)
(4, 4, 2, 40000), -- 2 Bò húc, 20.000đ tổng 40.000 vnd
(4, 11, 1, 15000), -- 1 Mì tôm xúc xích, mỗi suất 15.000đ, tổng 15.000đ

-- Ghi nhận cho UsageLogID 5 (Máy 5)
(5, 2, 2, 40000), -- 2 cốc Cafe, mỗi cốc 20.000đ, tổng 40.000đ
(5, 18, 1, 22000); -- 1 pizza, tổng 22.000đ
GO

select * from Account;
SELECT * FROM UsageLog;
SELECT * FROM Orders;
select * from DrinkSnack;

-- join all
SELECT *
FROM UsageLog ul
JOIN Orders o ON o.UsageLogID = ul.UsageLogID
JOIN Computer c ON c.ComputerID = ul.ComputerID
JOIN DrinkSnack d ON o.DrinkSnackID = d.DrinkSnackID

SELECT ul.UsageLogID, c.Name AS ComputerName, d.Name AS DrinkSnackName, d.Price, o.Quantity, o.Amount, ul.StartTime, ul.EndTime
FROM UsageLog ul
JOIN Orders o ON o.UsageLogID = ul.UsageLogID
JOIN Computer c ON c.ComputerID = ul.ComputerID
JOIN DrinkSnack d ON o.DrinkSnackID = d.DrinkSnackID
GROUP BY ul.UsageLogID, c.Name, d.Name, d.Price, o.Quantity, o.Amount, ul.StartTime, ul.EndTime;


-- tính tổng thu nhập thống kê
SELECT ul.ComputerID, CAST(ul.EndTime AS DATE) AS Date, SUM(o.Amount) AS TotalIncome
FROM Orders o
JOIN UsageLog ul ON o.UsageLogID = ul.UsageLogID
JOIN Computer c ON c.ComputerID = ul.ComputerID
JOIN DrinkSnack d ON o.DrinkSnackID = d.DrinkSnackID
WHERE CAST(ul.EndTime AS DATE) = '2024-09-10'
GROUP BY ul.ComputerID, CAST(ul.EndTime AS DATE)

-- join log user
SELECT *
FROM UsageLog u
INNER JOIN Computer c ON u.ComputerID = c.ComputerID
INNER JOIN Account a ON u.UserID = a.Id

--Tính tổng thu nhập theo ngày
SELECT SUM(TotalAmount) AS TotalIncome
FROM UsageLog
WHERE CAST(EndTime AS DATE) = '2024-09-10';

-- Lấy báo cáo sử dụng theo từng máy trong một ngày
SELECT c.Name AS ComputerName, u.StartTime, u.EndTime, u.TotalAmount
FROM UsageLog u
INNER JOIN Computer c ON u.ComputerID = c.ComputerID
WHERE CAST(u.EndTime AS DATE) = '2024-09-10';

/*
DECLARE @i INT = 0
--   + CAST(@i AS nvarchar(100))
WHILE @i <= 10
BEGIN
	INSERT INTO DrinkSnack (Name, Price)
	VALUES 
	('Food and drink '+ CAST(@i AS nvarchar(100)), CAST(RAND() * 100000000 AS INT))
	SET @i = @i + 1
END
GO
*/