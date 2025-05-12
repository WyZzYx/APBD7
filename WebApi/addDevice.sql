CREATE PROCEDURE AddDevice
    @Name       NVARCHAR(100),
    @IsEnabled  BIT,
    @DeviceType NVARCHAR(50),
    @TypeValue  NVARCHAR(MAX),
    @NewId      UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @NewId = NEWID();
    INSERT INTO Devices (Id, Name, IsEnabled, DeviceType, TypeValue)
    VALUES (@NewId, @Name, @IsEnabled, @DeviceType, @TypeValue);
END
