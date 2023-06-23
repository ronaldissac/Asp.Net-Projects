-----------------database----------------------------------------------------
Create database Omegadatabase

----------------------Create the ExportTable------------------------------------------
CREATE TABLE Export
(
    ID INT IDENTITY,
    ProductName VARCHAR(30),
    Quantity INT,
    FromLocation VARCHAR(MAX),
    ToLocation VARCHAR(MAX),
    FileData VARBINARY(MAX),
    FileName NVARCHAR(30),
    CreatedDate DATETIME DEFAULT GETDATE(),
	Track NVARCHAR(16) CONSTRAINT PK PRIMARY KEY,
	Location NVARCHAR(30),
	Estimation int,
	Status NVARCHAR(40),
	BLnumber NVARCHAR(100),
	CustomerName NVARCHAR(40),
	Payment NVARCHAR(40),
	Amount int,
	Vessel NVARCHAR(30)
);
select * from Export
------------------------------INSERT EXPORT(Procedure)--------------------------
CREATE PROCEDURE AddExport
    @ProductName VARCHAR(30),
    @Quantity INT,
    @FromLocation VARCHAR(MAX),
    @ToLocation VARCHAR(MAX),
    @FileData VARBINARY(MAX),
    @FileName NVARCHAR(30),
	@Track NVARCHAR(30)
AS
BEGIN
    INSERT INTO Export (ProductName, Quantity, FromLocation, ToLocation, FileData, FileName,Track)
    VALUES (@ProductName, @Quantity, @FromLocation, @ToLocation, @FileData, @FileName,@Track);
END;
------------------------------------DISPLAY EXPORT(Procedure)----------------------
CREATE PROCEDURE GetExportData
    @TrackID VARCHAR(50)
AS
BEGIN
    SELECT ProductName, Quantity, FromLocation, ToLocation, FileData,CreatedDate
    FROM export
    WHERE track = @TrackID;
END;
select * from Export
--------------------------------------------------Create Table Export_Status--------------------------------------------------------------------
CREATE TABLE Export_Status (
  FromLocation NVARCHAR(MAX),
  ToLocation NVARCHAR(MAX),
  Payment INT,
  Location NVARCHAR(50),
  Estimation INT,
  Track NVARCHAR(16),
  FOREIGN KEY (Track) REFERENCES Export(Track),
  Status NVARCHAR(30),
  vessel Nvarchar(30),
  
);

-------------------Login Cerdentials------------------------------------------------------------------

CREATE TABLE AdminTable (
    UserID NVARCHAR(50) PRIMARY KEY,
    Password VARCHAR(50)
);

insert into AdminTable  values ('ron@16', 2002)
select * FROM admintable

CREATE TABLE UserTable (
    UserID NVARCHAR(50) PRIMARY KEY,
    Password VARCHAR(50)
);

insert into UserTable values ('ronald',200216)

CREATE PROCEDURE ValidateLogin
    @UserType VARCHAR(50),
    @UserID VARCHAR(50),
    @Password VARCHAR(50),
    @IsValid BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @UserType = 'Admin'
    BEGIN
        IF EXISTS (SELECT 1 FROM AdminTable WHERE UserID = @UserID AND Password = @Password)
            SET @IsValid = 1;
        ELSE
            SET @IsValid = 0;
    END
    ELSE IF @UserType = 'User'
    BEGIN
        IF EXISTS (SELECT 1 FROM UserTable WHERE UserID = @UserID AND Password = @Password)
            SET @IsValid = 1;
        ELSE
            SET @IsValid = 0;
    END
    ELSE
    BEGIN
        SET @IsValid = 0;
    END
END

