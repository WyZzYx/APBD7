USE APBD_DB;
GO
INSERT INTO Devices (Id, Name, IsEnabled, DeviceType, TypeValue)
VALUES
  (NEWID(), 'Office PC', 1, 'PC', '{"CanBeTurnedOn": true}'),
  (NEWID(), 'Sensor Node', 0, 'Embedded', '{"Ip": "192.168.0.10", "Network": "LAN1"}'),
  (NEWID(), 'Fitness Watch', 1, 'Smartwatch', '{"BatteryPercent": 75}');