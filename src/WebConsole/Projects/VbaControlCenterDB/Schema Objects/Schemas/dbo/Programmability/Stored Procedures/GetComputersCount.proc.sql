﻿CREATE PROCEDURE [dbo].[GetComputersCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (c.[ID]) FROM Computers AS c
					INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query