USE [master]
GO
/****** Object:  Database [CCPDb-test]    Script Date: 10/15/2014 6:40:11 PM ******/
CREATE DATABASE [CCPDb-test]
GO
ALTER DATABASE [CCPDb-test] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CCPDb-test].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CCPDb-test] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CCPDb-test] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CCPDb-test] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CCPDb-test] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CCPDb-test] SET ARITHABORT OFF 
GO
ALTER DATABASE [CCPDb-test] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [CCPDb-test] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [CCPDb-test] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CCPDb-test] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CCPDb-test] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CCPDb-test] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CCPDb-test] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CCPDb-test] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CCPDb-test] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CCPDb-test] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CCPDb-test] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CCPDb-test] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CCPDb-test] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CCPDb-test] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CCPDb-test] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CCPDb-test] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CCPDb-test] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [CCPDb-test] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CCPDb-test] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CCPDb-test] SET  MULTI_USER 
GO
ALTER DATABASE [CCPDb-test] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CCPDb-test] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CCPDb-test] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CCPDb-test] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [CCPDb-test]
GO
/****** Object:  User [Administrator]    Script Date: 10/15/2014 6:40:11 PM ******/
CREATE USER [Administrator] FOR LOGIN [WS2008R2SP1\Administrator] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [Admin]
GO
ALTER ROLE [db_datareader] ADD MEMBER [Admin]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [Admin]
GO
/****** Object:  Table [dbo].[ApproverTier]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApproverTier](
	[ApproverTierId] [bigint] IDENTITY(1,1) NOT NULL,
	[SalesPersonId] [bigint] NOT NULL,
	[ApproverId] [bigint] NOT NULL,
	[TierId] [bigint] NOT NULL,
 CONSTRAINT [PK_Approvers] PRIMARY KEY CLUSTERED 
(
	[ApproverTierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApproveStatus]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApproveStatus](
	[ApproverStatusId] [bigint] IDENTITY(1,1) NOT NULL,
	[ContractId] [bigint] NOT NULL,
	[ApproverTierId] [bigint] NULL,
	[StatusId] [bigint] NULL,
 CONSTRAINT [PK_ApproveStatus] PRIMARY KEY CLUSTERED 
(
	[ApproverStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApproveStatusType]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApproveStatusType](
	[ApproverStatusId] [bigint] IDENTITY(1,1) NOT NULL,
	[ApproverStatusName] [nvarchar](max) NOT NULL,
	[ApproverStatusTag] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ApproveStatusType] PRIMARY KEY CLUSTERED 
(
	[ApproverStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Area]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Area](
	[AreaId] [bigint] IDENTITY(1,1) NOT NULL,
	[AreaName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Area] PRIMARY KEY CLUSTERED 
(
	[AreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AreaRole]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AreaRole](
	[AreaRoleId] [bigint] IDENTITY(1,1) NOT NULL,
	[AreaId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[PermissionId] [bigint] NULL,
 CONSTRAINT [PK_AreaRole] PRIMARY KEY CLUSTERED 
(
	[AreaRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Client]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[Id] [nvarchar](100) NOT NULL,
	[Secret] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ApplicationType] [int] NULL,
	[Active] [bit] NOT NULL,
	[RefreshTokenLifeTime] [int] NOT NULL,
	[AllowedOrigin] [nvarchar](100) NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contract]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contract](
	[ContractId] [bigint] IDENTITY(1,1) NOT NULL,
	[CPRNumber] [nvarchar](max) NULL,
	[Summary] [nvarchar](max) NULL,
	[TDGA] [int] NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[CustomerId] [bigint] NULL,
	[EndUserId] [bigint] NULL,
	[SalesPersonId] [bigint] NULL,
	[StatusId] [bigint] NULL,
 CONSTRAINT [PK_dbo.Contracts] PRIMARY KEY CLUSTERED 
(
	[ContractId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContractStatusType]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractStatusType](
	[ContractStatusId] [bigint] IDENTITY(1,1) NOT NULL,
	[ContractStatusName] [nvarchar](max) NOT NULL,
	[ContractStatusTag] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[ContractStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](max) NULL,
	[InternalCustomerId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EndUser]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EndUser](
	[EndUserId] [bigint] IDENTITY(1,1) NOT NULL,
	[EndUserName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.EndUsers] PRIMARY KEY CLUSTERED 
(
	[EndUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permission]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[PermissionId] [bigint] IDENTITY(1,1) NOT NULL,
	[PermissionName] [nvarchar](50) NOT NULL,
	[PermissionTag] [nchar](50) NOT NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RefreshToken]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshToken](
	[Id] [nvarchar](100) NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[ClientId] [nvarchar](50) NOT NULL,
	[IssuedUtc] [datetime2](7) NOT NULL,
	[ExpiresUtc] [datetime2](7) NOT NULL,
	[ProtectedTicket] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [bigint] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SalesPerson]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesPerson](
	[SalesPersonId] [bigint] IDENTITY(1,1) NOT NULL,
	[SalesPersonName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Region] [nvarchar](max) NULL,
	[SalesPersonNumber] [bigint] NOT NULL,
	[Status] [nvarchar](max) NULL,
	[ContractPrefix] [nvarchar](max) NULL,
	[UserId] [bigint] NOT NULL,
	[InternalSalesPersonId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.SalesPersons] PRIMARY KEY CLUSTERED 
(
	[SalesPersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tier]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tier](
	[TierId] [bigint] IDENTITY(1,1) NOT NULL,
	[ApproverLevel] [bigint] NOT NULL,
	[TDGAMinValue] [bigint] NOT NULL,
 CONSTRAINT [PK_Tier] PRIMARY KEY CLUSTERED 
(
	[TierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[RoleId] [bigint] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[InternalUserId] [bigint] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_User] UNIQUE NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [dbo].[AreaRoleView]    Script Date: 10/15/2014 6:40:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--USE [CCPDb-test]

create view  [dbo].[AreaRoleView]
as
SELECT DISTINCT
    --1 AS [C1], 
    --[Extent1].[AreaRoleId] AS [Id], 
    --[Extent1].[AreaId] AS [AreaId], 
    --[Extent1].[RoleId] AS [RoleId], 
	-- [Extent1].[PermissionId] AS [PermissionId], 
    --[Extent2].[RoleId] AS [RoleId1], 
    [Extent2].[RoleName] AS [RoleName], 
    --[Extent3].[AreaId] AS [AreaId1], 
    [Extent3].[AreaName] AS [AreaName]
    FROM   [dbo].[AreaRole] AS [Extent1]
    INNER JOIN [dbo].[Role] AS [Extent2] ON [Extent1].[RoleId] = [Extent2].[RoleId]
    INNER JOIN [dbo].[Area] AS [Extent3] ON [Extent1].[AreaId] = [Extent3].[AreaId]




GO
/****** Object:  Index [IX_Customer_Id]    Script Date: 10/15/2014 6:40:11 PM ******/
CREATE NONCLUSTERED INDEX [IX_Customer_Id] ON [dbo].[Contract]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EndUser_Id]    Script Date: 10/15/2014 6:40:11 PM ******/
CREATE NONCLUSTERED INDEX [IX_EndUser_Id] ON [dbo].[Contract]
(
	[EndUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_SalesPerson_Id]    Script Date: 10/15/2014 6:40:11 PM ******/
CREATE NONCLUSTERED INDEX [IX_SalesPerson_Id] ON [dbo].[Contract]
(
	[SalesPersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApproverTier]  WITH CHECK ADD  CONSTRAINT [FK_ApproverTier_Approver] FOREIGN KEY([ApproverId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[ApproverTier] CHECK CONSTRAINT [FK_ApproverTier_Approver]
GO
ALTER TABLE [dbo].[ApproverTier]  WITH CHECK ADD  CONSTRAINT [FK_ApproverTier_SalesPerson] FOREIGN KEY([SalesPersonId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[ApproverTier] CHECK CONSTRAINT [FK_ApproverTier_SalesPerson]
GO
ALTER TABLE [dbo].[ApproverTier]  WITH CHECK ADD  CONSTRAINT [FK_ApproverTier_Tier] FOREIGN KEY([TierId])
REFERENCES [dbo].[Tier] ([TierId])
GO
ALTER TABLE [dbo].[ApproverTier] CHECK CONSTRAINT [FK_ApproverTier_Tier]
GO
ALTER TABLE [dbo].[ApproveStatus]  WITH CHECK ADD  CONSTRAINT [FK_ApproveStatus_ApproverTier] FOREIGN KEY([ApproverTierId])
REFERENCES [dbo].[ApproverTier] ([ApproverTierId])
GO
ALTER TABLE [dbo].[ApproveStatus] CHECK CONSTRAINT [FK_ApproveStatus_ApproverTier]
GO
ALTER TABLE [dbo].[ApproveStatus]  WITH CHECK ADD  CONSTRAINT [FK_ApproveStatus_ApproveStatusType] FOREIGN KEY([StatusId])
REFERENCES [dbo].[ApproveStatusType] ([ApproverStatusId])
GO
ALTER TABLE [dbo].[ApproveStatus] CHECK CONSTRAINT [FK_ApproveStatus_ApproveStatusType]
GO
ALTER TABLE [dbo].[ApproveStatus]  WITH CHECK ADD  CONSTRAINT [FK_ApproveStatus_Contract] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
GO
ALTER TABLE [dbo].[ApproveStatus] CHECK CONSTRAINT [FK_ApproveStatus_Contract]
GO
ALTER TABLE [dbo].[AreaRole]  WITH CHECK ADD  CONSTRAINT [FK_AreaRole_Area] FOREIGN KEY([AreaId])
REFERENCES [dbo].[Area] ([AreaId])
GO
ALTER TABLE [dbo].[AreaRole] CHECK CONSTRAINT [FK_AreaRole_Area]
GO
ALTER TABLE [dbo].[AreaRole]  WITH CHECK ADD  CONSTRAINT [FK_AreaRole_Permission] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permission] ([PermissionId])
GO
ALTER TABLE [dbo].[AreaRole] CHECK CONSTRAINT [FK_AreaRole_Permission]
GO
ALTER TABLE [dbo].[AreaRole]  WITH CHECK ADD  CONSTRAINT [FK_AreaRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[AreaRole] CHECK CONSTRAINT [FK_AreaRole_Role]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK_Contract_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[ContractStatusType] ([ContractStatusId])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK_Contract_Status]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK_Contract_User] FOREIGN KEY([SalesPersonId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK_Contract_User]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Contracts_dbo.Customers_Customer_Id] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK_dbo.Contracts_dbo.Customers_Customer_Id]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Contracts_dbo.EndUsers_EndUser_Id] FOREIGN KEY([EndUserId])
REFERENCES [dbo].[EndUser] ([EndUserId])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK_dbo.Contracts_dbo.EndUsers_EndUser_Id]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
USE [master]
GO
ALTER DATABASE [CCPDb-test] SET  READ_WRITE 
GO
