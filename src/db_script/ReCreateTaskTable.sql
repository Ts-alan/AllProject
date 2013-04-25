--!!! Необходим только при переходе с версий ниже 3.12.7.0 на более позднюю версию !!!


-- Get Username
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetUsername]')
					   AND OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
DROP FUNCTION [dbo].[GetUsername]
GO

CREATE FUNCTION [dbo].[GetUsername]  (@param ntext)
RETURNS nvarchar(128)
AS
BEGIN   
	DECLARE @startIndex smallint
	DECLARE @endIndex smallint
	
	SET @startIndex = PATINDEX('%<Vba32CCUser>%', @param) + 13
	SET @endIndex = PATINDEX('%</Vba32CCUser>%', @param)
	
	RETURN SUBSTRING(@param, @startIndex, @endIndex - @startIndex)
END

------------------------------------------------------------------------------------
CREATE TABLE [dbo].[TasksTemp] (
	[ID] bigint IDENTITY(1, 1) NOT NULL,
	[TaskID] smallint NOT NULL,
	[ComputerID] smallint NOT NULL,
	[StateID] smallint NOT NULL,
	[DateIssued] datetime NOT NULL,
	[DateComplete] datetime NULL,
	[DateUpdated] datetime NOT NULL,
	[TaskParams] ntext COLLATE Cyrillic_General_CI_AS NULL,
	[TaskUser] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL
)
GO
	
INSERT INTO TasksTemp([TaskID],[ComputerID],[StateID],[DateIssued],
					[DateComplete],[DateUpdated],[TaskParams],[TaskUser])
SELECT	t.[TaskID], t.[ComputerID], t.[StateID],
		t.[DateIssued], t.[DateComplete], t.[DateUpdated], t.[TaskParams], dbo.GetUsername(t.[TaskParams])
FROM Tasks AS t
GO

DROP TABLE [dbo].[Tasks]
GO

CREATE TABLE [dbo].[Tasks] (
	[ID] bigint IDENTITY(1, 1) NOT NULL,
	[TaskID] smallint NOT NULL,
	[ComputerID] smallint NOT NULL,
	[StateID] smallint NOT NULL,
	[DateIssued] datetime NOT NULL,
	[DateComplete] datetime NULL,
	[DateUpdated] datetime NOT NULL,
	[TaskParams] ntext COLLATE Cyrillic_General_CI_AS NULL,
	[TaskUser] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_Tasks]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [FK_Tasks_TaskTypes]
		FOREIGN KEY (TaskID) REFERENCES TaskTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Tasks_TaskStates]
		FOREIGN KEY (StateID) REFERENCES TaskStates([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Tasks_Computers]
		FOREIGN KEY (ComputerID) REFERENCES Computers([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
)
GO
INSERT INTO Tasks([TaskID],[ComputerID],[StateID],[DateIssued],[DateComplete],[DateUpdated],[TaskParams],[TaskUser])
		SELECT	t.[TaskID], t.[ComputerID], t.[StateID],t.[DateIssued], t.[DateComplete], t.[DateUpdated], t.[TaskParams], t.[TaskUser]
		FROM TasksTemp AS t
GO

DROP TABLE [dbo].[TasksTemp]
GO
