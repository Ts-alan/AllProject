CREATE PROCEDURE [dbo].[GetListGroups]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
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
			[ParentName] nvarchar(128) NULL,
			[TotalCount] int NOT NULL,
			[ActiveCount] int NOT NULL
		)
	
		INSERT INTO @ListGroups([GroupID], [GroupName], [GroupComment], [ParentName], [TotalCount], [ActiveCount])
		SELECT	gt.[ID], gt.[GroupName], gt.[GroupComment], gt_temp.[GroupName], (SELECT COUNT(gr.[ID]) FROM Groups AS gr WHERE gr.[GroupID] = gt.[ID] ),
		(SELECT COUNT(*) FROM Groups AS g
			INNER JOIN Computers AS c ON c.[ID] = g.[ComputerID]
			WHERE g.[GroupID] = gt.[ID] AND DATEDIFF(minute, [RecentActive], GetDate()) <= 120)
		FROM GroupTypes as gt
		LEFT JOIN GroupTypes AS gt_temp ON gt_temp.[ID] = gt.[ParentID]

		DECLARE @ListGroups1 TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[GroupID] int,			
			[GroupName] nvarchar(128) NOT NULL,
			[GroupComment] nvarchar(128) NULL,
			[ParentName] nvarchar(128) NULL,
			[TotalCount] int NOT NULL,
			[ActiveCount] int NOT NULL
		)

		INSERT INTO @ListGroups1([GroupID], [GroupName], [GroupComment], [ParentName], [TotalCount], [ActiveCount])
		SELECT [GroupID], [GroupName], [GroupComment], [ParentName], [TotalCount], [ActiveCount]
		FROM @ListGroups'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + ' ORDER BY ' + @OrderBy
	
	SET @Query = @Query + N';
		SELECT [GroupID], [GroupName], [GroupComment], [ParentName], [TotalCount], [ActiveCount]
		FROM @ListGroups1 WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query