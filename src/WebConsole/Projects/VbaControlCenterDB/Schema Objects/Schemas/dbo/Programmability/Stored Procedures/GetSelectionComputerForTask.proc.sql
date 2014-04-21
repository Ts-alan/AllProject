CREATE PROCEDURE [dbo].[GetSelectionComputerForTask]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ComputersPage TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ComputerName] nvarchar(64) NOT NULL,
			[IPAddress] nvarchar(16) NOT NULL			
		)
	
		INSERT INTO @ComputersPage([ComputerName], [IPAddress])
				SELECT	 c.[ComputerName], c.[IPAddress] 
				FROM Computers AS c'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N';
		SELECT [ComputerName], [IPAddress]
		FROM @ComputersPage '
	EXEC sp_executesql @Query