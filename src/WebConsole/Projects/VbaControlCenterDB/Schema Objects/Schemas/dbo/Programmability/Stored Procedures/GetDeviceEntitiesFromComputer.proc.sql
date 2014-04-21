﻿CREATE PROCEDURE [dbo].[GetDeviceEntitiesFromComputer]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	 SELECT dp.[ID], dp.[ComputerID], c.[ComputerName], dp.[DeviceID], dps.[StateName],
	 d.[SerialNo], dt.[TypeName], d.[Comment], dp.[LatestInsert]
	 FROM DevicesPolicies as dp
	 INNER JOIN Computers as c ON c.[ID] = dp.[ComputerID]
	 INNER JOIN Devices as d ON dp.[DeviceID] = d.[ID]
	 INNER JOIN DevicePolicyStates as dps ON dps.[ID] = dp.[DevicePolicyStateID]
	 INNER JOIN DeviceTypes as dt ON dt.[ID] = d.[DeviceTypeID]
	 WHERE c.[ComputerName] = @ComputerName