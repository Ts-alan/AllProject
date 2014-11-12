CREATE TABLE [dbo].[ScanningComments] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,	
	[Comment] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL
)