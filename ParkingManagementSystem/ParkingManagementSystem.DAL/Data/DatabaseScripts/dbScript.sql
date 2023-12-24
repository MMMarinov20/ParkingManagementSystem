CREATE DATABASE ParkingManagementSystem;
USE ParkingManagementSystem;

CREATE TABLE Users (
    UserID INT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    [Password] VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
);

CREATE TABLE ParkingLots (
    LotID INT PRIMARY KEY,
    [Location] VARCHAR(100) NOT NULL,
    Capacity INT NOT NULL,
    CurrentAvailability INT NOT NULL,
);

CREATE TABLE Reservations (
    ReservationID INT PRIMARY KEY,
    UserID INT REFERENCES Users(UserID),
    LotID INT REFERENCES ParkingLots(LotID),
	CarPlate VARCHAR(10) NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    [Status] VARCHAR(20) NOT NULL, -- Confirmed/Pending
    CONSTRAINT chk_ReservationTime CHECK (StartTime < EndTime),
);

CREATE TABLE Feedback (
    FeedbackID INT PRIMARY KEY,
    ReservationID INT REFERENCES Reservations(ReservationID),
    Rating INT NOT NULL, -- Rating out of 5, for example
    Comment TEXT,
);
