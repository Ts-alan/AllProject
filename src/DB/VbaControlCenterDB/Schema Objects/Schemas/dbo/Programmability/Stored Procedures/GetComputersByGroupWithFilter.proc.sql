﻿CREATE PROCEDURE [dbo].[GetComputersByGroupWithFilter]
	@GroupID int,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT	* FROM ComputersExtended
		WHERE [GroupID] = ' + CAST(@GroupID AS nvarchar(32));
	
	IF @Where IS NOT NULL
		SET @Query = @Query + ' AND (' + @Where + ')'
	
	SET @Query = @Query + N' ORDER BY [ComputerName] ASC'

	EXEC sp_executesql @Query