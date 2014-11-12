ALTER TABLE [dbo].[DevicesPolicies]
	ADD CONSTRAINT [FK_DevicesPolicies_DevicePolicyStates] 
	FOREIGN KEY (DevicePolicyStateID)
	REFERENCES DeviceClassMode([ID])
	ON UPDATE CASCADE ON DELETE CASCADE