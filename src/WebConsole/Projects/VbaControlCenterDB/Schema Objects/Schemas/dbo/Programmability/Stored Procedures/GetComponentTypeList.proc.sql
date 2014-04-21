CREATE PROCEDURE [GetComponentTypeList]
WITH ENCRYPTION
AS
	SELECT [ComponentName] FROM [ComponentTypes]