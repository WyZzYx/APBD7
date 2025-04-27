CREATE DATABASE APBD_DB;
GO
USE APBD_DB;
GO
CREATE TABLE Devices (
                         Id          NVARCHAR(50) PRIMARY KEY,
                         Name        NVARCHAR(100) NOT NULL,
                         IsEnabled   BIT NOT NULL,
                         DeviceType  NVARCHAR(20)  NOT NULL,
                         TypeValue   NVARCHAR(MAX) NULL
);