﻿CREATE PROCEDURE [GetOSTypesList]
WITH ENCRYPTION
AS
	SELECT [ID],[OSName] FROM [OSTypes]