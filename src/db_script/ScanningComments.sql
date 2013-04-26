IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[ScanningComments]')
					   AND OBJECTPROPERTY(id, N'IsTable') = 1)
DROP TABLE [dbo].[ScanningComments]
GO

CREATE TABLE [dbo].[ScanningComments] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,	
	[Comment] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_ScanningComments]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_ScanningComments_IP]
		UNIQUE NONCLUSTERED ([IPAddress])
)
GO

-- Get comment by IPAddress
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetCommentByIP]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetCommentByIP]
GO

CREATE PROCEDURE [GetCommentByIP]
	@IP nvarchar(16)
WITH ENCRYPTION
AS
BEGIN	
	SELECT [Comment] FROM [ScanningComments] WHERE [IPAddress] = @IP
END
GO

-- Add comment by IPAdress
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddCommentByIP]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddCommentByIP]
GO

CREATE PROCEDURE [AddCommentByIP]
	@IP nvarchar(16),
	@Comment nvarchar(128)
WITH ENCRYPTION
AS
BEGIN
	DELETE FROM [ScanningComments] WHERE [IPAddress]=@IP

	INSERT INTO [ScanningComments] ([IPAddress], [Comment])
	VALUES(@IP, @Comment)
END
GO

-- Delete comment by IPAddress
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeleteCommentByIP]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteCommentByIP]
GO

CREATE PROCEDURE [DeleteCommentByIP]
	@IP nvarchar(16)
WITH ENCRYPTION
AS
BEGIN	
	DELETE FROM [ScanningComments] WHERE [IPAddress] = @IP
END
GO