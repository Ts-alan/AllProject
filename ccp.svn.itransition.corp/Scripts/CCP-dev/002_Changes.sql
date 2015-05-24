use [CCPDb-dev]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[MessageId] [bigint] IDENTITY(1,1) NOT NULL,
	[RecipientId] [bigint] NOT NULL,
	[SalesPersonId] [bigint] NULL,
	[ActionAuthorId] [bigint] NULL,
	[PreviousContractStatusId] [bigint] NULL,
	[CurrentContractStatusId] [bigint] NULL,
	[Text] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Recipient] FOREIGN KEY([RecipientId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Recipient]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_SalesPerson] FOREIGN KEY([SalesPersonId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_SalesPerson]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_ActionAuthor] FOREIGN KEY([ActionAuthorId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_ActionAuthor]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_PreviousContractStatus] FOREIGN KEY([PreviousContractStatusId])
REFERENCES [dbo].[ContractStatusType] ([ContractStatusId])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_PreviousContractStatus]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_CurrentContractStatus] FOREIGN KEY([CurrentContractStatusId])
REFERENCES [dbo].[ContractStatusType] ([ContractStatusId])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_CurrentContractStatus]
GO