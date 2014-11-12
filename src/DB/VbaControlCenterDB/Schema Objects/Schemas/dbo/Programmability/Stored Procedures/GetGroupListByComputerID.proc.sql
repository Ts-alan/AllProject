CREATE PROCEDURE [dbo].[GetGroupListByComputerID]
	@ComputerID smallint

WITH ENCRYPTION
AS	
		DECLARE @GroupListTemp TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] int NOT NULL,
			[GroupName] nvarchar(128) NOT NULL,
			[ParentID] int NULL,
			[GroupComment] nvarchar(128) NULL
		)

		DECLARE @GroupID int
		SET @GroupID = (SELECT gt.[ID] FROM [GroupTypes] AS gt
							INNER JOIN [Groups] AS g ON g.[GroupID] = gt.[ID]
							INNER JOIN [Computers] AS c ON c.[ID] = g.[ComputerID]
							WHERE c.[ID] = @ComputerID)

		WHILE (@GroupID IS NOT NULL)
		BEGIN
			INSERT INTO @GroupListTemp ([ID], [GroupName], [ParentID], [GroupComment])
			SELECT gt.[ID], gt.[GroupName], gt.[ParentID], gt.[GroupComment] FROM GroupTypes AS gt WHERE gt.[ID] = @GroupID
	
			SET @GroupID = 	(SELECT g.[ParentID] FROM GroupTypes AS g WHERE g.[ID] = @GroupID)
		END

		SELECT [ID], [GroupName], [ParentID], [GroupComment] FROM @GroupListTemp