use master;
DROP DATABASE ParkingManagementSystem;
CREATE DATABASE ParkingManagementSystem;
USE ParkingManagementSystem;


CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    [Password] VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
	isAdmin BIT NOT NULL DEFAULT 0
);

CREATE TABLE ParkingLots (
    LotID INT PRIMARY KEY,
	LotName VARCHAR(50) NOT NULL,
    [Location] VARCHAR(100) NOT NULL,
    Capacity INT NOT NULL,
    CurrentAvailability INT NOT NULL,
);

CREATE TABLE Reservations (
    ReservationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT REFERENCES Users(UserID),
    LotID INT REFERENCES ParkingLots(LotID),
	CarPlate VARCHAR(10) NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    [Status] VARCHAR(20) NOT NULL, -- Pending/Active
    CONSTRAINT chk_ReservationTime CHECK (StartTime < EndTime),
);

CREATE TABLE Feedback (
    FeedbackID INT PRIMARY KEY,
    ReservationID INT REFERENCES Reservations(ReservationID),
    Rating INT NOT NULL, -- Rating out of 5, for example
    Comment TEXT,
);

--INSERT INTO Users (FirstName, LastName, Email, [Password], Phone) VALUES ('admin', 'admin', 'example@', 'admin', '1234567890');
INSERT INTO ParkingLots (LotID, LotName, [Location], Capacity, CurrentAvailability) VALUES (1, 'Sea Garden Parking', 'Sea Garden', 220, 220);
INSERT INTO ParkingLots (LotID, LotName, [Location], Capacity, CurrentAvailability) VALUES (2, 'Grand Mall Parking', 'Grand Mall', 500, 500);
INSERT INTO ParkingLots (LotID, LotName, [Location], Capacity, CurrentAvailability) VALUES (3, 'West Airport Parking', 'West Airport Parking', 700, 700);
INSERT INTO ParkingLots (LotID, LotName, [Location], Capacity, CurrentAvailability) VALUES (4, 'Central Stadium Parking', 'Central Stadium', 100, 100);