CREATE PROCEDURE [dbo].[GetListGroupsCount]
@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ListGroups TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[GroupID] int,			
			[GroupName] nvarchar(128) NOT NULL,
			[GroupComment] nvarchar(128) NULL,
			[ParentName] nvarchar(128) NULL
		)
	
		INSERT INTO @ListGroups([GroupID], [GroupName], [GroupComment], [ParentName])
		SELECT	gt.[ID], gt.[GroupName], gt.[GroupComment], gt_temp.[GroupName]		
		FROM GroupTypes as gt
		LEFT JOIN GroupTypes AS gt_temp ON gt_temp.[ID] = gt.[ParentID]

		DECLARE @ListGroups1 TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[GroupID] int,			
			[GroupName] nvarchar(128) NOT NULL,
			[GroupComment] nvarchar(128) NULL,
			[ParentName] nvarchar(128) NULL
		)

		INSERT INTO @ListGroups1([GroupID], [GroupName], [GroupComment], [ParentName])
		SELECT [GroupID], [GroupName], [GroupComment], [ParentName]
		FROM @ListGroups'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where		
	SET @Query = @Query + N';
		SELECT COUNT(*)
		FROM @ListGroups1'
	EXEC sp_executesql @Query