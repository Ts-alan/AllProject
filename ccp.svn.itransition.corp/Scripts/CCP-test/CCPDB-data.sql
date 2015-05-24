use [CCPDb-test]

SET IDENTITY_INSERT [dbo].[ContractStatusType] ON 
GO
INSERT [dbo].[ContractStatusType] ([ContractStatusId], [ContractStatusName], [ContractStatusTag]) VALUES (1, N'Draft', N'Dr')
GO
INSERT [dbo].[ContractStatusType] ([ContractStatusId], [ContractStatusName], [ContractStatusTag]) VALUES (2, N'Pending', N'Pen')
GO
INSERT [dbo].[ContractStatusType] ([ContractStatusId], [ContractStatusName], [ContractStatusTag]) VALUES (3, N'Approved', N'App')
GO
INSERT [dbo].[ContractStatusType] ([ContractStatusId], [ContractStatusName], [ContractStatusTag]) VALUES (4, N'Rejected', N'Rej')
GO
INSERT [dbo].[ContractStatusType] ([ContractStatusId], [ContractStatusName], [ContractStatusTag]) VALUES (5, N'Returned', N'Ret')
GO
INSERT [dbo].[ContractStatusType] ([ContractStatusId], [ContractStatusName], [ContractStatusTag]) VALUES (6, N'Cancelled', N'Can')
GO
SET IDENTITY_INSERT [dbo].[ContractStatusType] OFF
GO



SET IDENTITY_INSERT [dbo].[EndUser] ON 
GO
INSERT [dbo].[EndUser] ([EndUserId], [EndUserName]) VALUES (1, N'Semen')
GO
INSERT [dbo].[EndUser] ([EndUserId], [EndUserName]) VALUES (2, N'Mihail')
GO
INSERT [dbo].[EndUser] ([EndUserId], [EndUserName]) VALUES (3, N'Egor')
GO
SET IDENTITY_INSERT [dbo].[EndUser] OFF
GO


SET IDENTITY_INSERT [dbo].[Customer] ON 
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (1, N'Valentina', 1)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (2, N'Vladinir', 2)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (3, N'Pavel', 3)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (4, N'Dmitry', 4)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (5, N'ABC Company', 5)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (6, N'Samsung', 6)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (7, N'Apple', 7)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (8, N'Sony', 8)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (9, N'Nokia', 9)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (10, N'HTC', 11)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (11, N'LG', 22)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (12, N'Philips', 33)
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [InternalCustomerId]) VALUES (13, N'Ren', 44)
GO
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO




SET IDENTITY_INSERT [dbo].[Tier] ON 
GO
INSERT [dbo].[Tier] ([TierId], [ApproverLevel], [TDGAMinValue]) VALUES (1, 1, 2000)
GO
INSERT [dbo].[Tier] ([TierId], [ApproverLevel], [TDGAMinValue]) VALUES (2, 2, 6000)
GO
INSERT [dbo].[Tier] ([TierId], [ApproverLevel], [TDGAMinValue]) VALUES (3, 3, 30000)
GO
SET IDENTITY_INSERT [dbo].[Tier] OFF
GO


SET IDENTITY_INSERT [dbo].[ApproveStatusType] ON 

GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (1, N'Assigned  ', N'Ass')
GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (2, N'Pending   ', N'Pen')
GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (3, N'Approved  ', N'App')
GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (4, N'Rejected  ', N'Rej')
GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (5, N'Returned  ', N'Ret')
GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (6, N'Canceled', N'Can')
GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (7, N'Skipped', N'Skip')
GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (8, N'Forced Reject', N'ForcR')
GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (9, N'Forced Approve', N'ForcA')
GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (10, N'Not Required', N'NotR')
GO
INSERT [dbo].[ApproveStatusType] ([ApproverStatusId], [ApproverStatusName], [ApproverStatusTag]) VALUES (11, N'Forced Skip', N'ForceS')
GO
SET IDENTITY_INSERT [dbo].[ApproveStatusType] OFF
GO



SET IDENTITY_INSERT [dbo].[Area] ON 

GO
INSERT [dbo].[Area] ([AreaId], [AreaName]) VALUES (1, N'CPRs')
GO
INSERT [dbo].[Area] ([AreaId], [AreaName]) VALUES (2, N'DataAdmin')
GO
INSERT [dbo].[Area] ([AreaId], [AreaName]) VALUES (3, N'ApprovalDashboard')
GO
INSERT [dbo].[Area] ([AreaId], [AreaName]) VALUES (4, N'MyCPRs')
GO
INSERT [dbo].[Area] ([AreaId], [AreaName]) VALUES (5, N'CPR')
GO
SET IDENTITY_INSERT [dbo].[Area] OFF
GO




SET IDENTITY_INSERT [dbo].[Role] ON 

GO
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (1, N'Initiator')
GO
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (2, N'Approver')
GO
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (3, N'Admin')
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO




SET IDENTITY_INSERT [dbo].[Permission] ON 

GO
INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionTag]) VALUES (1, N'Read', N'rd')
GO
INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionTag]) VALUES (2, N'Edit', N'edt')
GO
INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionTag]) VALUES (3, N'Approve', N'apr')
GO
SET IDENTITY_INSERT [dbo].[Permission] OFF
GO


SET IDENTITY_INSERT [dbo].[AreaRole] ON 

GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (1, 1, 1, NULL)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (2, 1, 2, NULL)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (3, 1, 3, NULL)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (4, 2, 3, NULL)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (5, 3, 2, NULL)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (6, 4, 1, NULL)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (7, 5, 1, 1)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (9, 5, 3, 1)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (17, 5, 2, 1)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (19, 5, 1, 2)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (20, 5, 3, 2)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (21, 5, 3, 3)
GO
INSERT [dbo].[AreaRole] ([AreaRoleId], [AreaId], [RoleId], [PermissionId]) VALUES (22, 5, 2, 3)
GO
SET IDENTITY_INSERT [dbo].[AreaRole] OFF
GO




SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (1, N'Michael', N'Jordan', N'Jordan@skf.com', 2, N'AF8aGzS0+IXeLdXPyaUX3KosGLeS2zqq4dbbznQkg1TZZAHGDG3Y1QZj/ULJawn5Aw==', 1233)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (2, N'Elvis', N'Presley', N'elvis@skf.com', 2, N'AH/jXkXxpuoBgNF14aXxB0IIGaveNmHBBGlu8ysnCN2UZIx2ElEKIyjSOz636XEavg==', 777)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (3, N'Lebron', N'James', N'Lebron@skf.com', 1, N'AF8aGzS0+IXeLdXPyaUX3KosGLeS2zqq4dbbznQkg1TZZAHGDG3Y1QZj/ULJawn5Aw==', 1231)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (4, N'Chuck', N'Norris', N'admin@skf.com', 3, N'AEe4BPxE1vapJT6rh3XcjpGW2c494EOY4T12ohAbyFb1s+T6Miu+wbZNWeH/AnZL/w==', 222)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (5, N'Semen', N'Muhin', N'Semen@skf.com', 1, N'ANqDbPFja930DWczuM9sG9tKiDlWVO9mEGxt0cey0sXfh5tL8uIqxwXQZ46G9bvVOw==', 231)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (6, N'Clark', N'Kent', N'superman@skf.com', 1, N'AO8dP6kPRUSLMtNO9GQQGrnOJm1nnD7whv6QOIxSg8kvSlLLuMg/Uiri5wOz0mM2Kg==', 111)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (7, N'Kate', N'Black', N'kate.black@skf.com', 2, N'AOoHbCQvIwJEa5Oc4XDaVO86vYushM//k3XAEkz3RrQ8i5OFnmC/n/aq83nyj3lQqw==', 123123)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (8, N'Tony', N'Stark', N'initiator@skf.com', 1, N'AGJcTNE7lj63Ogyxe17iGPi6ayOGYZY3ZvxAlGyWp9ZeNZlbjLmG85ETYWgvsaGWAw==', 123123)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (9, N'Bruce', N'Bane', N'admin1@skf.com', 3, N'AGTJJbk64Cwp37ee4R0Z8FAgHnLYE7Bcv4mvz5ApbCBeTlwdjJh6/Tw9lN0JT5COGw==', 87415)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (10, N'Luke', N'Skywalker', N'LowApprover@skf.com', 2, N'AEeXOvlOT08LrAgdge2E6h25uWFIwsGl05g+eIgIV5jHQpMgWCH8fDeQQfGOThqlzA==', 812144)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (11, N'Obi-Wan', N'Kenobi', N'MediumApprover@skf.com', 2, N'AA4KaSS4bffeuTcPQv7iuq0xluvO3JML7murxUrDwhbMIcctDPGh4xyjxz1FF+JsXA==', 893)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (12,  N'Dart', N'Wader', N'HighApprover@skf.com', 2, N'ANTj8cI+2SN8vasnuz0ZNzXgPrBCS2S5/cP44VMRsbwPHfMC1EIx9v9EzyxX4fGc4A==', 829)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (13, N'Peter', N'Blake', N'InitiationForEverybody@skf.com', 1, N'AL9aFV0tWMyfxwMsLKrJxPY+Fc8Jez3fJ6wvrkOoIMvo+6LeRSsDUrqdCpoYOr+XaQ==', 67390)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (14, N'Fedor', N'Ivanov', N'Fedor@skf.com', 2, N'ANjU2bWe11Vu8yQ0hPgCOxohXefSwnlk8nZOE1sluvi/MfAxXEfJsxBDjKHw3nvEQA==', 78426)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (15, N'Mihail', N'Govrilov', N'Mihail.govrilov@skf.com', 2, N'AGMtDxeV9lexUShbFChPej2cKQ/dQVRoY4soX7h8DbVQCxyOzT14ljUBcYfwtukx+A==', 123123)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (16, N'Artem', N'Petrov', N'petrov.a@skf.com', 1, N'AFd+pLr+94bjLoR8lQqgaKuGRFaInoI/xmWnWnsKCoMRU1CdPRfUByXfK9Q7rBNnxg==', 784923)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (17, N'Allen', N'Iverson', N'salesforeverybody@skf.com', 1, N'AKf2LJaDntEdv+XQRe2i18u+QXgNefdMO6e33ntEAQzvuaOmNixYhmCCTg5KRK1Kng==', 41656)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (18, N'Carl', N'Malone', N'carl@skf.com', 1, N'ADoesBVHAm97EB3gqKoIaYyXSR2EPoZJx3Nt3HWoRtad2YmAUUC5GguitVXJ0k8kTA==', 6784841)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (19, N'Nate', N'Robinson', N'Fima@skf.com', 1, N'AC1Dmc+2h1V9ZdO3YZJ3Tyua9+Ml/S4iXHTM7EHdEezv+l9YNeIwHcz2JasMHeeITg==', 7283902)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (24, N'Kevin', N'Love', N'initiator@skf.com', 1, N'ADFuyE9pLlS6hOGcuXdieF2t2xuak3USDKXZ6unZOtOcZ+pfxdz4Z825cSOxzvfnJA==', 747222)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (25, N'Shawn', N'Marion', N'Shawn@skf.com', 1, N'AJHyarwz5ukZskv6cSCxu3oFEgyeuPuUhxIpYnTXfvbS4S5kurldeJNraC2ZpL8JEA==', 1231348)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (26, N'Shaquil', N'Oneal', N'shaquil@skf.com', 1, N'APXQW/yvETYn/ZFoKEC3gsAUezRfc4Vv0akuq1nmKJwAus+ZmY74sXCfIPOwbfAL5A==', 12312384)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (27, N'Donald', N'Duck', N'donald@skf.com', 2, N'AOBYLMKD4gVRRAInQAkqMas8iAhXsu98G7RveInkV5ukKd6tpMWPHnyHEhc6Ti5C2g==', 756489)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (28, N'Mickey', N'Mouse', N'mickey@skf.com', 3, N'AENp2Kgli/5gq/8Pk4zg0AdWEWZsUIKgVzHClp1Fw/nTlu7sVDl0ByVKiSqc0PAiLA==', 4371432)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (37, N'Egor', N'Smirnov', N'smirnov_egor@skf.com', 1, N'ANYSTHdbbWgHzHbeTkRSEZwGsajVMbodbTIIsOgMc9TU3JDt9WanwwzxekT0WA0WKA==', 6789866)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (41, N'Blake', N'Griffin', N'blake_griffin@skf.com', 1, N'AMC9msf0rGBc2j9VJrZbAvvdWufQ5aauqq0xK8UmHHM3vutkRtdLTr4GZsE6GuvVLw==', 92929)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (43, N'Nikita', N'Visocky', N'nikitos@skf.com', 3, N'AI11cc7oaLGhLCOZmg0L7/FHrcJ/jBaCIAFEYyDncjmZP08fPPw5n1mDquSiThHG9Q==', 78945)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (44, N'Mihail', N'Potapov', N'potapov@skf.com', 1, N'AMGGwMWDiL9nVqn3mGZbs9mm2tLyQImaU/xBrtOSQ9VEpr6gMyLTQ64oFIQh33RbNg==', 1234)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (45, N'Viktor', N'Kuznecov', N'kuznec@skf.com', 2, N'AA6Kc4rXAXz96kdC9eqTTtiZ/JK1EhknGqsz8b/V1c4CLI50bMKDx4uSmLWCMSTPDg==', 1234)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (46, N'Petya', N'Medvedev', N'medved@skf.com', 1, N'AJ2dkVmufNZM7qsjldiV9PfinteNS4rqlVpYx9LD7lofoh+LJOjlXZr5g1qPK98iCg==', 2)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (47, N'Spider', N'Man', N'spiderman@skf.com', 1, N'AGpI9SsPKDy4wbcFustQaVElwr7cX7GL5Pskb9AT8K8sl8OgN/4BFbpbkjGHA2tRJw==', 1)
GO
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [RoleId], [PasswordHash], [InternalUserId]) VALUES (48, N'Kurt', N'Cobain', N'rewq@skf.com', 1, N'AFYZe5awWJDMSBso9+vSi5psLe4tNNTBj/gVq4OZeh2VzJgZxjygctfJIBTGhpPKrQ==', 1)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO




SET IDENTITY_INSERT [dbo].[ApproverTier] ON 

GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (7, 24, 10, 1)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (8, 24, 11, 2)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (9, 24, 12, 3)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (22, 25, 10, 1)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (23, 25, 11, 2)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (24, 25, 12, 3)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (25, 19, 2, 1)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (26, 19, 10, 2)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (27, 19, 11, 3)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (34, 5, 10, 1)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (35, 5, 11, 2)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (36, 5, 12, 3)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (37, 17, 2, 1)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (38, 17, 11, 2)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (39, 17, 12, 3)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (76, 44, 10, 1)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (77, 44, 11, 2)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (78, 44, 12, 3)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (79, 46, 10, 1)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (80, 46, 11, 2)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (81, 46, 12, 3)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (82, 47, 12, 1)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (83, 47, 14, 2)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (84, 47, 11, 3)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (85, 48, 2, 1)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (86, 48, 27, 2)
GO
INSERT [dbo].[ApproverTier] ([ApproverTierId], [SalesPersonId], [ApproverId], [TierId]) VALUES (87, 48, 10, 3)
GO
SET IDENTITY_INSERT [dbo].[ApproverTier] OFF
GO



SET IDENTITY_INSERT [dbo].[Contract] ON 

GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (70, N'70', N'Saling some tracktors', 2, CAST(0x07000000000004380B AS DateTime2), CAST(0x07000000000004390B AS DateTime2), NULL, NULL, 17, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (71, N'71', N'Not very expensive contract', 1000, CAST(0x07000000000009390B AS DateTime2), CAST(0x07000000000012390B AS DateTime2), NULL, NULL, 19, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (77, N'77', N'No approve required', 1000, CAST(0x07000000000002390B AS DateTime2), CAST(0x07000000000005390B AS DateTime2), NULL, NULL, 17, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (79, N'79', N'Tier 1 approve required', 3000, CAST(0x070000000000F4380B AS DateTime2), CAST(0x0700000000000C390B AS DateTime2), NULL, NULL, 25, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (81, N'81', N'Tier 1,2 approve required', 10000, CAST(0x070000000000F4380B AS DateTime2), CAST(0x070000000000FC380B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (82, N'82', N'Tiers 1,2,3 approve required', 40000, CAST(0x07000000000004390B AS DateTime2), CAST(0x0700000000009F390B AS DateTime2), NULL, NULL, 19, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (83, N'83', N'No Approval', 1000, CAST(0x070000000000F2380B AS DateTime2), CAST(0x0700000000000D390B AS DateTime2), NULL, NULL, 17, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (84, N'84', N'No appr', 1, CAST(0x070000000000F7380B AS DateTime2), CAST(0x070000000000FF380B AS DateTime2), NULL, NULL, 19, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (85, N'85', N'Coffee', 123123, CAST(0x07000000000004390B AS DateTime2), CAST(0x07000000000005390B AS DateTime2), NULL, NULL, 5, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (86, N'86', N'No approve', 1500, CAST(0x070000000000F8380B AS DateTime2), CAST(0x0700000000000D390B AS DateTime2), NULL, NULL, 24, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (87, N'87', N'First Tier Approve', 5000, CAST(0x070000000000FE380B AS DateTime2), CAST(0x07000000000005390B AS DateTime2), NULL, NULL, 24, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (88, N'88', N'Two levels required', 10000, CAST(0x070000000000F3380B AS DateTime2), CAST(0x07000000000010390B AS DateTime2), NULL, NULL, 25, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (89, N'89', N'Very expensive contract', 1000000, CAST(0x070000000000FE380B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 19, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (90, N'90', N'Bearrings', 5000, CAST(0x070000000000FF380B AS DateTime2), CAST(0x0700000000000D390B AS DateTime2), NULL, NULL, 5, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (91, N'91', N'Simple contract', 5000, CAST(0x070000000000FB380B AS DateTime2), CAST(0x0700000000000C390B AS DateTime2), NULL, NULL, 24, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (92, N'92', N'Cigarets', 29999, CAST(0x0700881C05B0FD380B AS DateTime2), CAST(0x0700881C05B00C390B AS DateTime2), NULL, NULL, 24, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (93, N'93', N'Milk', 50000, CAST(0x0700000000000A390B AS DateTime2), CAST(0x070000000000193A0B AS DateTime2), NULL, NULL, 5, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (94, N'94', N'Tables and chairs', 2001, CAST(0x0700000000000C390B AS DateTime2), CAST(0x07000000000001390B AS DateTime2), NULL, NULL, 5, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (95, N'95', N'Hard disks', 7000, CAST(0x07000000000074250B AS DateTime2), CAST(0x0700000000006C390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (96, N'96', N'Saling computers', 6500, CAST(0x070000000000E7080B AS DateTime2), CAST(0x0700000000000B390B AS DateTime2), NULL, NULL, 17, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (97, N'97', N'Hamburgers', 5000, CAST(0x07000000000099250B AS DateTime2), CAST(0x0700000000000C390B AS DateTime2), NULL, NULL, 19, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (99, N'99', N'Potato', 600000, CAST(0x070000000000F2380B AS DateTime2), CAST(0x07000000000013390B AS DateTime2), NULL, NULL, 5, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (101, N'101', N'Coca-cola', 2222, CAST(0x07000000000074250B AS DateTime2), CAST(0x070000000000FA380B AS DateTime2), NULL, NULL, 5, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (102, N'102', N'Vegetables', 12344321, CAST(0x07000000000001390B AS DateTime2), CAST(0x0700000000000A390B AS DateTime2), NULL, NULL, 25, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (103, N'103', N'Kids toys', 30003, CAST(0x070000000000FD380B AS DateTime2), CAST(0x07000000000010390B AS DateTime2), NULL, NULL, 5, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (104, N'104', N'Gifts', 100, CAST(0x070000000000FD380B AS DateTime2), CAST(0x07000000000010390B AS DateTime2), NULL, NULL, 5, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (107, N'107', N'Wood', 10000, CAST(0x07000000000017390B AS DateTime2), CAST(0x07000000000022390B AS DateTime2), NULL, NULL, 19, 2)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (108, N'108', N'Mouses', 12345, CAST(0x07000000000015390B AS DateTime2), CAST(0x07000000000022390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (110, N'110', N'Cups', 30000, CAST(0x0700000000001D390B AS DateTime2), CAST(0x0700000000002F390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (111, N'111', N'Plants', 123456, CAST(0x0700000000008B250B AS DateTime2), CAST(0x07000000000001270B AS DateTime2), NULL, NULL, 5, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (113, N'113', N'Pencils', 11000, CAST(0x0700000000001B390B AS DateTime2), CAST(0x07000000000022390B AS DateTime2), NULL, NULL, 17, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (114, N'114', N'Shoes', 3000000, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000012390B AS DateTime2), NULL, NULL, 5, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (115, N'115', N'Jeans', 70000, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000027390B AS DateTime2), NULL, NULL, 24, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (116, N'116', N'Doors', 300022, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000010390B AS DateTime2), NULL, NULL, 5, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (117, N'117', N'Monitors', 123555, CAST(0x07000000000029380B AS DateTime2), CAST(0x070000000000BF380B AS DateTime2), NULL, NULL, 5, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (118, N'118', N'Keyboards', 123, CAST(0x0700000000002A380B AS DateTime2), CAST(0x070000000000BF380B AS DateTime2), NULL, NULL, 17, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (119, N'119', N'Notebooks', 1234, CAST(0x07000000000018390B AS DateTime2), CAST(0x0700000000002A390B AS DateTime2), NULL, NULL, 17, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10119, N'10119', N'Phones', 100000, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000028390B AS DateTime2), NULL, NULL, 25, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10120, N'10120', N'Tablets', 222, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000013390B AS DateTime2), NULL, NULL, 5, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10121, N'10121', N'T-shirts', 30001, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000012390B AS DateTime2), NULL, NULL, 5, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10122, N'10122', N'Trouses', 30001, CAST(0x0700000000000E390B AS DateTime2), CAST(0x07000000000012390B AS DateTime2), NULL, NULL, 5, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10123, N'10123', N'Coats', 22222, CAST(0x0700000000000E390B AS DateTime2), CAST(0x0700000000000F390B AS DateTime2), NULL, NULL, 5, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10124, N'10124', N'Beds', 30001, CAST(0x0700000000001C390B AS DateTime2), CAST(0x07000000000029390B AS DateTime2), NULL, NULL, 19, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10125, N'10125', N'Horses', 30002, CAST(0x070000000000FA380B AS DateTime2), CAST(0x0700000000003D390B AS DateTime2), NULL, NULL, 5, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10126, N'10126', N'Pigs', 200, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000017390B AS DateTime2), NULL, NULL, 24, 6)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10127, N'10127', N'Cows', 22222, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000011390B AS DateTime2), NULL, NULL, 5, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10128, N'10128', N'Pool', 33300, CAST(0x0700000000000E390B AS DateTime2), CAST(0x07000000000011390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10129, N'10129', N'Sport goods', 25000, CAST(0x07000000000015390B AS DateTime2), CAST(0x07000000000061390B AS DateTime2), NULL, NULL, 19, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10130, N'10130', N'Trainers', 10010, CAST(0x070000000000DD380B AS DateTime2), CAST(0x0700000000001B390B AS DateTime2), NULL, NULL, 25, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10131, N'10131', N'Balls', 2134, CAST(0x07000000000015390B AS DateTime2), CAST(0x07000000000030390B AS DateTime2), NULL, NULL, 44, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10132, N'10132', N'Table tennis', 12343415, CAST(0x0700000000001B390B AS DateTime2), CAST(0x0700000000002D390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10133, N'10133', N'Chess', 30002, CAST(0x07000000000018390B AS DateTime2), CAST(0x07000000000019390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10134, N'10134', N'Food', 2000000, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B012390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10135, N'10135', N'Music', 100000000, CAST(0x07000000000010390B AS DateTime2), CAST(0x07000000000012390B AS DateTime2), NULL, NULL, 24, 2)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10136, N'10136', N'Video', 444444444, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 2)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10137, N'10137', N'Photo', 3333111, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000011390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10138, N'10138', N'Water', 1111111, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000013390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10139, N'10139', N'Cold water', 2000000, CAST(0x07000000000018390B AS DateTime2), CAST(0x0700000000001A390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10140, N'10140', N'Soda water', 30002, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10141, N'10141', N'Paper', 30001, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000013390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10142, N'10142', N'Cars', 30001, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000012390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10143, N'10143', N'Motorcycles', 340000, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000013390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10144, N'10144', N'Bullets', 333000, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000013390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10145, N'10145', N'Tears', 30001, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000013390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10146, N'10146', N'Racing cars', 20000011, CAST(0x07000000000017390B AS DateTime2), CAST(0x0700000000001A390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10147, N'10147', N'Skateboards', 30000000, CAST(0x07000000000010390B AS DateTime2), CAST(0x0700000000001B390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10148, N'10148', N'Guitars', 300001, CAST(0x0700000000001F390B AS DateTime2), CAST(0x07000000000022390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10149, N'10149', N'Bass guitars', 399999, CAST(0x07000000000017390B AS DateTime2), CAST(0x07000000000018390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10150, N'10150', N'Credit Cards', 30000, CAST(0x07000000000010390B AS DateTime2), CAST(0x07000000000011390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10151, N'10151', N'Bags', 30001, CAST(0x07000000000018390B AS DateTime2), CAST(0x0700000000001A390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10152, N'10152', N'Hats', 400000, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10153, N'10153', N'Jerseys', 30000, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000012390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10154, N'10154', N'Maps', 30001, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000013390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10155, N'10155', N'Trash', 30002, CAST(0x07000000000010390B AS DateTime2), CAST(0x07000000000013390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10156, N'10156', N'Magazines', 300000, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10157, N'10157', N'Buildings', 20000000, CAST(0x07000000000013390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10158, N'10158', N'Summary', 44444444, CAST(0x07000000000013390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10159, N'10159', N'Battareys', 333222, CAST(0x07000000000017390B AS DateTime2), CAST(0x0700000000001A390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10160, N'10160', N'Floopies', 2000000, CAST(0x07000000000013390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10161, N'10161', N'Chairs', 222222, CAST(0x07000000000011390B AS DateTime2), CAST(0x0700000000001C390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10162, N'10162', N'Tables', 123456, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000012390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10163, N'10163', N'Knowledges', 434343, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10164, N'10164', N'Headphones', 100000, CAST(0x07000000000013390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10165, N'10165', N'3000 bearrings', 300000, CAST(0x0700000000001F390B AS DateTime2), CAST(0x07000000000021390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10166, N'10166', N'Filters', 400000, CAST(0x07000000000013390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10167, N'10167', N'Keys', 40000, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10168, N'10168', N'Service', 40004, CAST(0x07000000000017390B AS DateTime2), CAST(0x07000000000019390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10169, N'10169', N'Cats', 40000, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10170, N'10170', N'Dogs', 300001, CAST(0x07000000000018390B AS DateTime2), CAST(0x0700000000001A390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10171, N'10171', N'Video Games', 123456, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000013390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10172, N'10172', N'Rabbits', 100022, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000022390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10173, N'10173', N'Usb kable', 44440000, CAST(0x07000000000010390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10174, N'10174', N'EVOD 2 Usb', 23334, CAST(0x07000000000017390B AS DateTime2), CAST(0x07000000000019390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10175, N'10175', N'Motherboards', 42000, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10176, N'10176', N'Carrot', 4433322, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10177, N'10177', N'Apples', 22224444, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10178, N'10178', N'iPhone', 200000, CAST(0x07000000000012390B AS DateTime2), CAST(0x0700000000001B390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10179, N'10179', N'Samsung', 100000, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10180, N'10180', N'HTC', 200001, CAST(0x07000000000013390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10181, N'10181', N'Lenovo', 23000000, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10182, N'10182', N'Nokia', 20000000, CAST(0x07000000000013390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10183, N'10183', N'Motorola', 22000, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10184, N'10184', N'Soney', 30003, CAST(0x07000000000010390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10185, N'10185', N'Soney Erricsson', 20000000, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10188, N'10188', N'Starships', 1000, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10189, N'10189', N'Guns', 1234, CAST(0x07000000000010390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10190, N'10190', N'Books', 20000, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10191, N'10191', N'Ships', 123, CAST(0x07000000000021390B AS DateTime2), CAST(0x0700000000002A390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10192, N'10192', N'Tanks', 2222, CAST(0x07000000000026390B AS DateTime2), CAST(0x07000000000031390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10193, N'10193', N'Weapons', 20000, CAST(0x07000000000010390B AS DateTime2), CAST(0x0700000000001B390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10196, N'10196', N'Nuclear bombs', 30000, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10197, N'10197', N'Liqud fire', 123456, CAST(0x0700000000000F390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10198, N'10198', N'Maches', 3000, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10203, N'10203', N'Paper for contracts', 1000, CAST(0x07000000000018390B AS DateTime2), CAST(0x07000000000019390B AS DateTime2), NULL, NULL, 19, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10204, N'10204', N'Hope', 2000, CAST(0x07000000000010390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10229, N'10229', N'Scissors', 40000, CAST(0x07000000000010390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10233, N'10233', N'TV sets', 3000, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10234, N'10234', N'DVD players', 121, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10235, N'10235', N'Fear', 34521, CAST(0x07000000000016390B AS DateTime2), CAST(0x0700000000002A390B AS DateTime2), NULL, NULL, 19, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10236, N'10236', N'Lamps', 2000000, CAST(0x07000000000019390B AS DateTime2), CAST(0x0700000000001C390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10237, N'10237', N'Bridges', 26788, CAST(0x0700000000001A390B AS DateTime2), CAST(0x0700000000001C390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10238, N'10238', N'Computer & Networking', 1234, CAST(0x07000000000016390B AS DateTime2), CAST(0x07000000000029390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10239, N'10239', N'Tablets', 500000, CAST(0x07000000000018390B AS DateTime2), CAST(0x07000000000022390B AS DateTime2), NULL, NULL, 24, 2)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10240, N'10240', N'Laptops', 200000, CAST(0x07000000000012390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 19, 2)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10241, N'10241', N'Desktops', 100000, CAST(0x07000000000011390B AS DateTime2), CAST(0x07000000000014390B AS DateTime2), NULL, NULL, 19, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10242, N'10242', N'Storage', 91, CAST(0x07000000000003390B AS DateTime2), CAST(0x07000000000015390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10243, N'10243', N'Networking', 123456, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 2)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10244, N'10244', N'Tablet Accessories', 123456, CAST(0x0700881C05B01E390B AS DateTime2), CAST(0x0700881C05B022390B AS DateTime2), NULL, NULL, 19, 2)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10245, N'10245', N'Laptop Accessories', 120000, CAST(0x0700881C05B00F390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 19, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10246, N'10246', N'Computer Peripherals', 20000, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 19, 2)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10247, N'10247', N'Computer Components', 1000000, CAST(0x0700881C05B010390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 19, 2)
GO																																												   
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10248, N'10248', N'Consumer Electronics', 12345, CAST(0x0700881C05B012390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10249, N'10249', N'Camera & Photography', 200011, CAST(0x0700881C05B00F390B AS DateTime2), CAST(0x0700881C05B018390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10250, N'10250', N'Home Audio & Video', 20000, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10251, N'10251', N'TV Stick', 20000, CAST(0x0700881C05B010390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10252, N'10252', N'Accessories & Parts', 5000, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10253, N'10253', N'Video Games', 30002, CAST(0x0700881C05B018390B AS DateTime2), CAST(0x0700881C05B022390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10254, N'10254', N'Portable Audio & Video', 30000, CAST(0x0700881C05B017390B AS DateTime2), CAST(0x0700881C05B01B390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10255, N'10255', N'Earphones & Headphones', 12345, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B01D390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10256, N'10256', N'Mini Camcorders', 123456, CAST(0x0700881C05B012390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10257, N'10257', N'Memory Cards', 2132432, CAST(0x0700881C05B016390B AS DateTime2), CAST(0x0700881C05B021390B AS DateTime2), NULL, NULL, 24, 4)
GO																																												   
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10258, N'10258', N'Phones & Accessories', 1234, CAST(0x07000000000016390B AS DateTime2), CAST(0x07000000000021390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10259, N'10259', N'Mobile Phones', 10000, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B012390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10260, N'10260', N'Bags & Cases', 123456, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10261, N'10261', N'Batteries', 112233, CAST(0x0700881C05B010390B AS DateTime2), CAST(0x0700881C05B013390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10262, N'10262', N'Chargers & Docks', 1, CAST(0x0700000000001D390B AS DateTime2), CAST(0x07000000000030390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10263, N'10263', N'Backup Powers', 222000, CAST(0x0700881C05B012390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10264, N'10264', N'Cables', 123456, CAST(0x0700881C05B012390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10265, N'10265', N'Lenses', 60000, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B013390B AS DateTime2), NULL, NULL, 19, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10266, N'10266', N'Parts', 50000, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10267, N'10267', N'LCDs', 123456, CAST(0x0700881C05B012390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10268, N'10268', N'Holders & Stands', 123456, CAST(0x0700881C05B012390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10275, N'10275', N'Stickers', 123456, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B013390B AS DateTime2), NULL, NULL, 24, 4)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10276, N'10276', N'Interior Lights', 1, CAST(0x0700881C05B00E390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 3)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10277, N'10277', N'Engine', 123444, CAST(0x0700881C05B010390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10278, N'10278', N'Fuel Injector', 1, CAST(0x0700000000001D390B AS DateTime2), CAST(0x0700000000002A390B AS DateTime2), NULL, NULL, 24, 1)
GO
INSERT [dbo].[Contract] ([ContractId], [CPRNumber], [Summary], [TDGA], [StartDate], [EndDate], [CustomerId], [EndUserId], [SalesPersonId], [StatusId]) VALUES (10279, N'10279', N'car accessories', 123456, CAST(0x0700881C05B011390B AS DateTime2), CAST(0x0700881C05B014390B AS DateTime2), NULL, NULL, 24, 1)
GO
SET IDENTITY_INSERT [dbo].[Contract] OFF
GO





SET IDENTITY_INSERT [dbo].[ApproveStatus] ON 

GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (6, 79, 22, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (8, 81, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (9, 81, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (10, 82, 25, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (11, 82, 26, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (12, 82, 27, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (13, 87, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (14, 88, 22, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (15, 88, 23, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (16, 89, 25, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (17, 89, 26, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (18, 89, 27, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (19, 91, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (25, 92, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (26, 92, 8, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (27, 93, 34, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (28, 93, 35, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (29, 93, 36, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (30, 94, 34, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (31, 85, 34, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (32, 85, 35, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (33, 85, 36, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (37, 96, 37, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (38, 96, 38, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (39, 97, 25, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (40, 102, 22, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (41, 102, 23, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (42, 102, 24, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (43, 103, 34, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (44, 103, 35, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (45, 103, 36, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (46, 107, 25, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (47, 107, 26, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (48, 111, 34, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (49, 111, 35, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (50, 111, 36, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (51, 114, 34, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (52, 114, 35, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (53, 114, 36, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (54, 113, 37, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (55, 113, 38, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (56, 115, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (57, 115, 8, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (58, 115, 9, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (59, 116, 34, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (60, 116, 35, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (61, 116, 36, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (62, 117, 34, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (63, 117, 35, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (64, 117, 36, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (65, 95, 7, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (66, 95, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (67, 10119, 22, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (68, 10119, 23, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (69, 10119, 24, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (70, 10121, 34, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (71, 10121, 35, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (72, 10121, 36, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (73, 10122, 34, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (74, 10122, 35, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (75, 10122, 36, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (76, 10123, 34, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (77, 10123, 35, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (78, 10125, 34, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (79, 10125, 35, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (80, 10125, 36, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (81, 10128, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (82, 10128, 8, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (83, 10128, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (84, 10133, 7, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (85, 10133, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (86, 10133, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (87, 10134, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (88, 10134, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (89, 10134, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (90, 10135, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (91, 10135, 8, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (92, 10135, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (93, 10136, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (94, 10136, 8, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (95, 10136, 9, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (96, 10137, 7, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (97, 10137, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (98, 10137, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (99, 10138, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (100, 10138, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (101, 10138, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (102, 10139, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (103, 10139, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (104, 10139, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (105, 10140, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (106, 10140, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (107, 10140, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (108, 10141, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (109, 10141, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (110, 10141, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (111, 10142, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (112, 10142, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (113, 10142, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (114, 10143, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (115, 10143, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (116, 10143, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (117, 10144, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (118, 10144, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (119, 10144, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (120, 10145, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (121, 10145, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (122, 10145, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (123, 10146, 7, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (124, 10146, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (125, 10146, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (126, 10147, 7, 5)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (127, 10147, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (128, 10147, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (129, 10148, 7, 5)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (130, 10148, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (131, 10148, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (132, 10149, 7, 5)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (133, 10149, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (134, 10149, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (135, 10150, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (136, 10150, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (137, 10150, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (138, 10151, 7, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (139, 10151, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (140, 10151, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (141, 10152, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (142, 10152, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (143, 10152, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (144, 10153, 7, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (145, 10153, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (146, 10153, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (147, 10154, 7, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (148, 10154, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (149, 10154, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (150, 10155, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (151, 10155, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (152, 10155, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (153, 10156, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (154, 10156, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (155, 10156, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (156, 10157, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (157, 10157, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (158, 10157, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (159, 10158, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (160, 10158, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (161, 10158, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (162, 10159, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (163, 10159, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (164, 10159, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (165, 10160, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (166, 10160, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (167, 10160, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (168, 10161, 7, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (169, 10161, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (170, 10161, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (171, 10162, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (172, 10162, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (173, 10162, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (174, 10163, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (175, 10163, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (176, 10163, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (177, 10164, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (178, 10164, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (179, 10164, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (180, 10165, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (181, 10165, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (182, 10165, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (183, 10166, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (184, 10166, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (185, 10166, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (186, 10167, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (187, 10167, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (188, 10167, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (189, 10168, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (190, 10168, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (191, 10168, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (192, 10169, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (193, 10169, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (194, 10169, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (195, 10170, 7, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (196, 10170, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (197, 10170, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (198, 10171, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (199, 10171, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (200, 10171, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (201, 10172, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (202, 10172, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (203, 10172, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (204, 10173, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (205, 10173, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (206, 10173, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (207, 10174, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (208, 10174, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (209, 10175, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (210, 10175, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (211, 10175, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (212, 10176, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (213, 10176, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (214, 10176, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (215, 10178, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (216, 10178, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (217, 10178, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (218, 10179, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (219, 10179, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (220, 10179, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (221, 10180, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (222, 10180, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (223, 10180, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (224, 10181, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (225, 10181, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (226, 10181, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (227, 10236, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (228, 10236, 8, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (229, 10236, 9, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (230, 10241, 25, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (231, 10241, 26, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (232, 10241, 27, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (233, 10240, 25, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (234, 10240, 26, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (235, 10240, 27, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (236, 10239, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (237, 10239, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (238, 10239, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (239, 10243, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (240, 10243, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (241, 10243, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (242, 10244, 25, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (243, 10244, 26, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (244, 10244, 27, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (245, 10246, 25, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (246, 10246, 26, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (247, 10247, 25, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (248, 10247, 26, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (249, 10247, 27, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (250, 10248, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (251, 10248, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (252, 10250, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (253, 10250, 8, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (254, 10249, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (255, 10249, 8, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (256, 10249, 9, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (257, 10251, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (258, 10251, 8, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (259, 10252, 7, 9)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (260, 10253, 7, 9)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (261, 10253, 8, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (262, 10253, 9, 9)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (263, 10254, 7, 9)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (264, 10254, 8, 9)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (265, 10254, 9, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (266, 10255, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (267, 10255, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (268, 10256, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (269, 10256, 8, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (270, 10256, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (271, 10257, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (272, 10257, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (273, 10257, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (274, 10259, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (275, 10259, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (276, 10260, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (277, 10260, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (278, 10260, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (279, 10261, 7, 2)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (280, 10261, 8, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (281, 10261, 9, 1)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (282, 10263, 7, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (283, 10263, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (284, 10263, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (285, 10264, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (286, 10264, 8, 8)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (287, 10264, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (288, 10265, 25, 9)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (289, 10265, 26, 9)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (290, 10265, 27, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (291, 10266, 7, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (292, 10266, 8, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (293, 10266, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (294, 10267, 7, 9)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (295, 10267, 8, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (296, 10267, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (297, 10275, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (298, 10275, 8, 4)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (299, 10275, 9, 7)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (300, 10268, 7, 3)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (301, 10268, 8, 9)
GO
INSERT [dbo].[ApproveStatus] ([ApproverStatusId], [ContractId], [ApproverTierId], [StatusId]) VALUES (302, 10268, 9, 8)
GO
SET IDENTITY_INSERT [dbo].[ApproveStatus] OFF
GO



INSERT [dbo].[Client] ([Id], [Secret], [Name], [ApplicationType], [Active], [RefreshTokenLifeTime], [AllowedOrigin]) VALUES (N'consoleApp', N'lCXDroz4HhR1EIx8qaz3C13z/quTXBkQ3Q5hj7Qx3aA=', N'Console Application', 1, 1, 14400, N'*')
GO
INSERT [dbo].[Client] ([Id], [Secret], [Name], [ApplicationType], [Active], [RefreshTokenLifeTime], [AllowedOrigin]) VALUES (N'ngAuthApp', N'5YV7M1r981yoGhELyB84aC+KiYksxZf1OY3++C1CtRM=', N'AngularJS front-end Application', 0, 1, 7200, N'*')
GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'/HxCGSSzugHkOHsPM6NcP0bjWrPYKVbVOl1DMKG2UP0=', N'Ololo@tut.tam', N'ngAuthApp', CAST(0x07D66189AD4417390B AS DateTime2), CAST(0x07D66189AD441C390B AS DateTime2), N'oh9zIHdb-iSyYJQPK-peRS7KpmIzLtUrLnbb_NHkOTAc6soNH8-g_uOWM8MtwuFSSRTDOWX0FZprv7C4jSHc4JC7v06dniXJO7AH3h6D86GG2dvOr4RCuMxvyKC8hcoXlc29qr3sPsEDaceeVJHaV6QQGOuDKzWV6HgJC7Btae6NDcD4dwyYuz1TNh_oEjnIW6YxDxeyG8brdXDLgiA_VxfKtusA7DTToU9HtF1GAMKOimXjZs7giXb_ZnMpzyCFl5s-yeRpRtk5gbNO35BATxOLpdNnehReMu-0-aa2prA6_t43IZ5e_4ItKCLwjkZPdPW5X3HFhjSvIJUGFc1xk7PYTnoG7u9aNltEpd_OUSEY9HSVO8ZoZBsEFGMiu_DvzhArKYiOo0SKKriivUtPT-1mMMbO28XY_1vVyj1PQwxZ5rO8l90g6ApeWYjQVpkI8fFBApTY5wSCKriSn1E4lWD7XdAK1FfhE-zdee2R2i_Ym89hfXOdjoiE8cfOS_6X')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'A5edUw7f2Mq9mTfMKjVkanHO5KSanb5tP0FCxCKfJ5I=', N'MediumApprover@approve.com', N'ngAuthApp', CAST(0x076F6F14EE871E390B AS DateTime2), CAST(0x076F6F14EE8723390B AS DateTime2), N'zwmWVH6iHhYWadbOIsM1knT9cp7_ecfeviwOuRDH9VpE-mjDHAaQTEakrTSWq8zcAjNdUV7hVNiGEEyDf68GNQImt5iARQ4-rEFndcmz2UKSYuItvLFBRS-skDlsXrAxKZh8JldhZxPWQf90TJBnJIv-Wdj0QGzk6lg5amaHqdnqj3sdD02N-_UxamOfLQf2LUhEgIo_r9xjy3aTlecR2W2-TdM_xU1X7PjABwOtSl-6zS60sBHwH0W6zn4WTMjO6G7QF7pNUYSR-NsbTH-l6TG2PnHJF-eCtN4kRbujQxpWgwCzC9Lha9eWRuiwFeeqYE1EwfYRclYelDxF8R17q0mzIX0ZI7ElGDI09whvGRjK5K5-JQSaxELNEc29wRiF5piP1AXAgYJScTRfi1bLQ-MM6P6XVdM5Fwyu7wIbrqUDjIVsZFy6mTCM-RBCTrcYkOA4JEpUhWr0DXqMYGtHg6YYnuD2KAbDKHxtKEZin0Y')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'asI4o1xdwUrLP/WO1YVDhDPseuvELf5MBVrGxD09/fA=', N'Fima@argh.ru', N'ngAuthApp', CAST(0x07C8238EE15714390B AS DateTime2), CAST(0x07C8238EE15719390B AS DateTime2), N'wIPeOxZyhpsW7Da42bFH968vWOmeFXtYJV5Gjm3RYuLm_6Zrnj-XOgR5mWZ-yHyK5a4iDvDyMQ8QPpIWiWnLXERPe5sZq1JBIBZa75YEy7y5LImYWMoaHi96GzPAc3Khv-W0npTeJxfH6knnEuzwinMsPpLzrB42PuAHV1LHF0PU17PC0o7veKEuYxz2quDf4vSiYnvAxRXT1PPVBhQ-j3BzjmgXpCcPnL4dgdUvBk2TNL9qMJLNhst96g-xaOjR0glF_5T3LQXj3mniSRVOGBgIstHmrDcLl9PvfRQKrJHTsXZJBsAta7LXhxW_2mqkUL7ssu6bmPHRyIqZKA3RfSUKNJ2sCDYuMFYfWeZ4SGFEJUkukXf7_GwNvxd48z0MD48nWLS9P_gKmFmZtMfcZJMvunqoIgf4DE0ZVJ9k17rUxGSLjwiHuF4FD2xOoTuWS2AvVjS3TmSNwmMtFdD7DbAkm4sQ_ImyzHHJYT6JFvI')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'BOQOqXTvu80BZ/GHwLTmFeHziidEMzcZbANj61Mb550=', N'salesforeverybody@bla.bla', N'ngAuthApp', CAST(0x07F8AD10EB5714390B AS DateTime2), CAST(0x07F8AD10EB5719390B AS DateTime2), N'bgOkGS49UBOlEy6OlsjrkxDAC_jw7fMM4D-26o45QcO8zwehLDBMiY_ujco496nz4yIcHQP9A21Lr7NNHB6WG5D34HOwk3jcGAW1It7YgL5Jia2aUlf9wkhAvF09ODK9aOCYAGPt5U7v74rfg24sLB7egUDq2_3yXnkUGji1Ubz-H5iJuhnPj9YZT1jK4NOEKHjFVwoThPraLSGMBJJ0gJehURllA2_hEVMjSmPLz1Nb_hfgRLPKQ2Thol6undeYx8Qi11bgIBjKlF2CqLOHtah-pTbmkxHN18O5HeVpUEwUd1OkKQZE5wFrnLv2HuhJpD7y6Xia5WSbdrooCD94aHhYfKVqipiD4XrspJOuQhSTnZWu5h11t6eNPcfaNgg7YsaOiely16H_itsxn00gPxkQvJzIJjorfa8AmrCtuwheyL6M631Lfy5gqQMUvNVWSv3ssyts-m5RbJo33jq2iSVi081gOLuuBQucnmSa9d0_RjKXR4Tj2yIHRCG95D1a')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'BxEvM4mD+3AkLAfPSCp3riTXrbhF9o3ZthkgiuM24m8=', N'LowApprover@approve.com', N'ngAuthApp', CAST(0x0793CD54BB6320390B AS DateTime2), CAST(0x0793CD54BB6325390B AS DateTime2), N'2jocRrdvI942Sh3lT0w43yeH-s3Nn_IKXFkXPrbmPJZHk_9526VSUSRA4BE6kyDA2ZeKQVAOfG78r_IZDfPg-vrs-zoIEFuVQq0SIf3M7Xl-sS_lLej5ScRzBsBvigkgFaUcKr-z12s92o5wuaw9n6LNvF4H10kkApcVyYwWhpDy_lY1tC2khhyFZWi0LIv5WIVkMDNSWVcrre_xC3hAZUZoICzIseeSegF-U3k5UYKa_2AO5KWePtUa2z6DYL57MOey38CW1KSWJMUUBtL4vDTDabPFbnKdK9KumqcQ_hvI70cr1GZdFHL02RKQd9_ZGASIf8sc2ndeZ99hcn9u0RdXfaq-f1HZOUgmUl0xe6SEHIWC2j00K38ZHDa5tRDScEqYDVSEgbjhUrf0oSjBYFqctjAHj6DaZUkdB0zgGDEC8prFNJ3zdRPrdOlBogiU50ZOFUDFn3HOMWYopt-9OdAE3TUalLNmnkpb9bezNBU')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'gHCEd1zg/UOTVlrs9eOhF628qAhDwYDAsP5CrMbKFB4=', N'initiator@tut.by', N'ngAuthApp', CAST(0x07183B1E4D8E20390B AS DateTime2), CAST(0x07183B1E4D8E25390B AS DateTime2), N'ARsxMVfCWqr7SCmpS0d52UvmbSQE6ucgkHgVuyrIki79NdJkkOT4GUU54ORtMWHCcc13npxEmi4EFco7T3i2Oo8_vXARTTlUsbMSRiluCGJ5_oOXnslCXsAdvlAa9eKqhK6PGJQsH7Pbha1DXikWToWiLiBpDFJGgXN5PoWwEpFB-AB42AEbg1xf7UN5lrYmiK4Pnt61EWIL2G1p-wNOvW2ozMeeEHantGM-YJo0fNviTL1ggWhgHORB7pYOG4jWEjvpsYWMSGE6GPTAH2fuUJebyUbIgAtdR9_i0MQ8wmi2O-6HcUQS5CfLscDoCCRscWa8PUoHLGRsxXTRxZ7IEt2boPcZ8SxvH1uC5NE1X_L2TgmziArtUxdXB_bd6JAdg_C4O8t2VSZvs8lAyk1zJ2a1RZ60B9k44fJbfHc3woHbPBSZSROd5n35cFSh58GOb5gUp0gn4z_w6SL_nqTcncbO0H1VvX2FlcXqpOKp9LQ')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'H2lxdOPamWhgDHAS/mOozyCOTCU2aBtCQZR2ornQMc8=', N'Sen9@gmail.com', N'ngAuthApp', CAST(0x07949E139F8517390B AS DateTime2), CAST(0x07949E139F851C390B AS DateTime2), N'Cv8dkQ_5R8vUNysQT_mXk3dc_BqZ0zE-zPljUbdWizDqCA02g1FhhlNR78ZjMJLs50B0IgWGCXWTdq0WlFXxBWsWPXnIoeiLCFdTS1Cuitj2rM9mvWCxtxC5xiRC1OZ4C-gXDiWlu_C2Ho5kxl8TqTSG1s4tKtDz5hHB_VEzf4HiqjRipDgHW0rRj525UZX9AM_oovnkAATJezH1kvaVtlEJL5Xg1EMoZSRQf_uGz4SWkJ34m5o53Fe5o_Jt9KM2-j3RVrwNs4Wa3N9nJpBfWu6smO7QTFlTEyhjtVSia3coLXjsJLTErPtZgf5SNrtgB--fGeXoKVulwTvfw9E8ozr8bY5v1F4zZ3Da2z4chSFBGCeuvfbwdAdCyXFkn3RLdJ5eF6jYauRc1TEgHvREu57vlJEWBo6FfEzv4SrO302aGFT-te8GwJeEoyTBBEzXKjHc2ch1ki9t1od-YWDtvPs4BHHJG1D5T9eCp8qi2KQ')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'Kza8HzzBmM/urBz7pLRr77lrxwMCTq76LkF2uqjg40U=', N'rettour@tut.by', N'ngAuthApp', CAST(0x07B30643514517390B AS DateTime2), CAST(0x07B3064351451C390B AS DateTime2), N'DuonZdzzVDT4NVC1zeWyed0XWij-ck7YZT0jvDhl2gCXp5TZb2ngBpfLEYd0EsfEGkUZPCJDaDxGNC79JeYH-irH-HbXD8QzWyyS-GS5d1eC8WvrXwdm8Jko-1rM3I2gkJdXTbgl2FTbIrOZWrCWHAJhR26Jg6d5_glOVKFHFJS_OLMqNE7RsNv7_5sZZfhWftZN-dqbo531M1CR2peJUyxfQMfEJlsdJA7cfqyM4TzV42Fu9MmBaWQHMuSfmiiFuBtpXaRrQkJPftgFW2bJTE2TbwNnhIvVRVR0mpB0T-LrT4qPgUzDeY6KKTKa6U8Q68R_WgJEq8R7mtersuCRMM5gMFvfuWBkvXE5bAGuNtwegluvW5aS2Vpr-KyJIFikjA6w0L9W1wdV6FlM3InZt87OladxZjGLGS1MlLtPUTkc4BWzFZgr5cjXZfV6UVDLvBO-QOtTSNxHte5Pm4o-LsnmywLR4d6dM-luIcziIcI')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'MYCedWsxmpGRl8oYQ/li4TEN3Id4QC/TrLh4yas+bnk=', N'ads@l.com', N'ngAuthApp', CAST(0x07EA6F6D2F6D12390B AS DateTime2), CAST(0x07EA6F6D2F6D17390B AS DateTime2), N'jUrM3hjtL7eofAb5EAecbCmNTeKVKbzWuWyLUJgS7uFYvhVuFkMizVV20nR0boVwIbQ7DSZV-om2WrsbK9LVWLxEmH-ElZgem3YHBydfuKCHCBT_CU0lJPVJTSsc3oqhum0UW8qOJnQw74ySnEO4NT33_mV4BNXtKSaSJeU0yz6P-RdovYt7K8Y-8eiEgSl0z4k70doTUzE-1DY6guIk-XKL0NC75tq9gQEY4ml-q2w-xg_yW1rPaL3YSFMEfPOkYc9a-OfXfKGo_8MhbETGAObPUboE4tsHmkkI4-TgtzEGR3yyqUmhGUI9YYWFUtg6nlnOqXBauWq3a-NUTgPB_eZjtnfzC-jWZ6Z8tCaNwJJiwNR8M2BiYH1ZShJrDnqpFBP3-dOBmcEQxOKUp4dbW1Q3Qw6wWMaOmPSMNiblskMFtClaY3NAJrwVhKZ-9EeM8TYK_BbLIkpIt27W3TIAcw')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'NvGX51q0zVuira3KfRXThB/ZlxlVzfzcUzR0LuturKU=', N'111@111.asd', N'ngAuthApp', CAST(0x0702244CF98511390B AS DateTime2), CAST(0x0702244CF98516390B AS DateTime2), N'ta-ZhmNzHuurCep-BzcxAU7dJyyziLa1qiVEXSUDWFTE1gu3tR27cQyDHr-tInBaCrgBvdYDOuApBJ6A8fXxfiSioigR-QuoHBYC5e_kuEwytncRwRtCrHnXD4nz9p3P-e92po_hIZbRl-PhhbKDTMwviGIirBe5cYEbQtGZeKT_FylgSWD8c5y5V4kiY1LYN-fnaTp_WxKFdCu8hDvBxl3Xzmqj55Byr85TBv2cqKoZ-tfE2qC014uQuu237YjshsA-ybL0k-yiaYxNxL8UvMXsw81SbcAHh0MtXcQwBaY_qDDubf41hToylDMCd1gRQ004Pf4zol7ytmYJUFOuEKDF6wnADifQ6MYHBRF3_So4yfp2uafUon2bQsTmrXLSjUUOhKfRu9lLPX18YI7uGkaDFa33Awsgjw01kicJO0vmgwNcuVeLlYkRim38mvahNzieTWUi6B0FzQcjlJUjDg')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'qN+gWuLoPhpFyYPQIg1tNIzTtuhofe+S/h7OwgG7Vss=', N'HighestApprover@approve.com', N'ngAuthApp', CAST(0x0728BB0AD3771C390B AS DateTime2), CAST(0x0728BB0AD37721390B AS DateTime2), N'hND9TpuFes4qCObB0rLbUVPEnSkVYJ2Djil4DIIWIjiAQawO-olxhmY3GHnCvVAH7mrY87FsLvt2k7G30dQK6D2geJN6KlkGOZ7kM_1761xtkyr-wEvZiuMrTJVf8DgFQOmsxP0pWMW2EJ9rpvitBCgd0zeDaBK4J-z2xjmTLzjdQbbrgtExZ5r9aEQb_cXxIARbUAtwPpzRaUTDt_XohPtWSkRycZ_kYWpQUOoJua5ZJdb16Bla3PL8kpT3pHa_wBJkO-0k9koezvqG9OI4LKOIipWW4K60dUJfapVcu_EXdBcePJBF_9fGx8JtMsNofOKloUIjTr6CJR9F5_Ra0mYvIAofq0BvV8QxO65Hfw3DTmvgF6m0ikfV0Ig1yW69XKSbiTOQ-nFYtUhNwA8roMWRE5hsOjVHxUloJa6EuwV8dyRJ5o-Yrs73yxNh14PwlXuIARhQ6YkWPsU_L3kXezdqjTsB9ceFjamWcJ4f3eo')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'rm8bz11Fa28+Ge8waA0wPDy6dq9RqH1+msh03p8ymu8=', N'aa@aa.aa', N'ngAuthApp', CAST(0x07B6E66AD16420390B AS DateTime2), CAST(0x07B6E66AD16425390B AS DateTime2), N'BuaIURHJzeeZUgEIbufloFDAgNA_PqyM68y62ZINFlcDVzWmVM9P3JuHh_Y0QXKo6-oH0aUDRy3aic7zSsBp70mKVdLjLZ9AJKDwyQKvj4-e2MqhJX7R8JGa2VQPF_VP6VlaVCGOi1oVkYTgbJrly-5cUMy5JS-z-uuPCQV86HuGbI-51R3st3WQxJkLFs-rB30PF_uE87V6OWNAX5_jfOtmmwAyEI4DeCK4DDPQTVwmrWq8LDO-ScDODda4RJNl5Ot7oJH6mWUXxqLYQIwb4TX7PeNtdDZPGT8Wa1nzTWmsC0q2Otfmez2vRbMDnb3G1zc7AK4jFgQXdomztePvXKCwC-nvfV_jC8eiTtxpsK_nTMO0Db9YoDaD8Ml-sltBBq4q5sJIDgLJmy-PMkEGy-B8AM_Iyd9kdB280aHuUmp93sY0nk2ULfr7xO9m8LB6Dtd87Ciyk0wc5ixUpDboB44WtjPBv6dkmtZpK-u3Sd4')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'SXtcgKdnI1e12zsNzHxRXpY/aPbfC/11WP2CotuxpO8=', N'pupkin@tut.tam', N'ngAuthApp', CAST(0x0788E71B994717390B AS DateTime2), CAST(0x0788E71B99471C390B AS DateTime2), N'3kuu0Uxfqi2o9iWB3aXNJt5PX6r0icupLng4Wj7IpWFb5EVnKMzr6-8xzCPnUUslvCoiLLlwpIcm1HyRwe10aWmz0hLfyavuvjoBO-wHrWzndf9Y9dJbRf9kFOKoU7m5kEm8rSciYhPtWBMZTnjFtdOBxBaecrV7WGtMSxhLG3wW1U-nHsqZdPdvKv9iqft3Knb3aWuBi3fujaOV07QHph34f-0iTkGMfkEIcb9z6QxwDeD8z8fvcfplOPdBPUulZKoGQFsTzQIMzuMc1PxMHfkHnpRP5aFka36kdK3IbBa9zSHwVHInc0-6QATqnZgJg6zv3Pv_DVg761Yij_D6I43GDmGPFiEE6BLA-e7Q-BitJdz641MXuiesaY3bT1lY-IP1d2A0JW2vc637ZaUOVIZ7An23yiBOhDgRiDpK--xdkHYy-NXFaO3s-7uII-OgtXgrH4XeGQXoA6uLpvon-Ep5WkYeAi8KkQQDrIt7Asc')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'vLt7o6e5tu+l/eDBYB5iivMnvTL7EmXz/tFKEtqTpv4=', N'Admin@tut.by', N'ngAuthApp', CAST(0x07F7C364558D20390B AS DateTime2), CAST(0x07F7C364558D25390B AS DateTime2), N'gwrmR7gIOaJvgRBPS-LDEWznZdfb1Lqix3p2BV0jHemlX8TYp8bvLminj7TlvsCVhniaPSG_Cmbvx8fYXZ4hUWyezYBrZdbKL4zzuPoJNFi-2QT9HNooZSX_Cnubg6dHPcTCkeYepBqq7SzLrH1AAQv1RgLe0jzObrn50ersOPVGYGeJ765KSjOOQDZgMxMB4g89-sbW-1ARMrLe0stTrFM0oAHhV9oP1qdLCGwl1ZomC7tsPHRzPgBI-ms-e7Kr9XyhfZk49uoq9aZtc7hjrgu-hoNfKoDWmdVHVoul-mgo_n6xIf2i4hsOgG0JPxF74zzNJ-0V2c5o0qK7hDxH31296mzY85iH9ePZ01jaBFQ3OeBYvTZk8yV-3kmBeYcne0SYneB43Q26mEHGJEtMYN-lx6WxRhD8R3Y8axeq1fTlToZZm4B1l7SU4KH7k_0NdeuWS2HknKFrXQAfbc_1NMobYpZgUQTDIfmhK5YCgzM')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'VZV573OaiHtPE24LMnYq6wzvvgxXzdd/o2yP8DndE8k=', N'admin@gmail.com', N'ngAuthApp', CAST(0x07C66BAA338020390B AS DateTime2), CAST(0x07C66BAA338025390B AS DateTime2), N'0nndwS0MOzshiv7W96wHgRqE5hFHyYRP2PdaLY7rcJqsLDsBKouecAW7IXnxaGlknKnaKd1paIZlZtNgKCtszHpInLtPUQgroD-l5C0Tg9pxiSgjs4HaKX2EGyxnTa3nv0mxboHaVXzD04xPCGJjH9vknIYZ6s6q_wf1JVq5TEaRE3FYXBgytb3fZ29BTG-B1E3z8XuXgXQe6CQ5oyOddeOcioZ5DD4BK2wU7iN7sPsMJwRsgOdR4kgz_9AMN7o_G7tdC4tPggvDk7qWw0YMDkzNri28wZGq30-uh84ZIcTmTr7-DzWRS9JHYOxQ5zcn2V3l_BqpHz7KBCtFz0-1Mb8QRPPkf2GAzR_Qrcbf-l7RDdu9yiI3chMjU54e7zcFmwqQ8XWLQR3Rk4-HXLMHNXwTWAAp1dCNNNeliHiTW--9w2sj-oQLUrfcwc8fK34jg3V33n8lZ-DgYCStPHRBQSnC5R3Duf5n-8Vb4GaAXS8')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'xcLRh84pRdRqcY81aUv7wsaiusp2wufnpybkUVjuqpg=', N'a@a.aa', N'ngAuthApp', CAST(0x07BDEF8201AA13390B AS DateTime2), CAST(0x07BDEF8201AA18390B AS DateTime2), N'xrLzegI0Ov41Xj1L290-f9xwSohOlIOWh9-qP4kqFKMYhDrSQe9T_UbSSUPQ7o4GBbipGTMEhaSovlyfjit_7Q92hqMu1HFIv4CffCCwEADQpT3JZGMvrcM9AQ_2Y3Uauw2CRqgGjkDmktVBvEP7URcpDNDo8ioTLlY-psvsLBGEFz4khLXp8sAco4FTXF76QMGt7HWoI6L1S9yJoFwmb4vL-UKUlN-dtRMFRL3UKvliOXffvB3cWECuR7J2Wv7jjYANwVm4wOVdhHKDbXoGn0OOlemI3GT-THwLbv7CeEIR7fikChmaoRaMFgE0evrcxnt2DicS7Zf0W6JqhBPeC0u68iG3RQeVEBd58IfNC-N4jf0NdhbmhtL-rUHs7Eb4gLYHTlg8SNkhT2G4X9vrdRj0nOGyeAOM77ANAeX0GGBzrAS2kQjBjFqG2SS-UYhZfp__iRMmcg0AqC-cYBflcJlPRiCDMDhGCEh4pkYd0hc')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'xuoMXVyMV1Rg6T1dRge9rEQraNlLV50UHTtgDUxXydA=', N'skovoroda@gmail.com', N'ngAuthApp', CAST(0x07E9FB018C5614390B AS DateTime2), CAST(0x07E9FB018C5619390B AS DateTime2), N'k1zv6b-fYgHVGkyPoVJHrqFqD60mvRoFUWq8h2Sj7Onbt_43UTHFN0yETbtcVij4XKL25-VI_bCRUQQTB71nx8tGkZQzoqLgT1B_nBcsRzYwWdrXFQy5ppnWZ8m9xMMZCSyBaDU48xhZ7Wspel2bSr9nFgiVmPm444xfO0bvlpu1Ui7dL8TAdazWdy8YpUSTx9Us2pFANRx-Qp0dNJwlHM4FhZe6RSyGc4jsX3IEDfkTtWORIBVrYsutT3FzlkjPXsTDsHu063z7wUhE4nFeUfUQgYRwz2Ab1_ANOhgRxnS7pe-kAKGmfFLe2qoTH9DqDiTYVCKweKKxE0x7QcVmjbb6RHew5EWaPZOh3wx2htPBnG2kRrEmTD0WeclNkalSDML79-Of9xfAlsdHt6yRsX2c4kpJjitBneUMn8f6dd5mkIgZSFpjRE4QkLJIpj4SjSRMdvlRPffZm0NVTYlntO7Q2XiCyIb3uQUzmboIvPhnKA3aWcALpjhQaiepkF3F')
--GO
--INSERT [dbo].[RefreshToken] ([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket]) VALUES (N'ywVRdncBOw8k/lTYoy9m0P9KT3NJiqxU2Uv7O8KM2r8=', N'Fed9@tut.by', N'ngAuthApp', CAST(0x07381421CF5814390B AS DateTime2), CAST(0x07381421CF5819390B AS DateTime2), N'-sh29NNvsdR3KjieAqdE_hN_7L-aorpgUrYuPBAsXbZmE7vv0mluskstUUA3WZ1ItFJyHSxpAOBoaoDVjIMLBpvVhRVQ0OzMdNA1YNbxNjRxIpkKwOIfwm8VbKuDBk3nFTPIyUvzI2abwqBJnq1b_JYLuB8THfLcezuHgEq5hky_eNTZiRln51I-nSHy2u3oQtGgD6M-Su7JSIvZ-rciDsYDkpMXRCb9aNaKZyBTIhk167-_KgW2M8Sw2BT23Zw15YJ1klChXZyaYlSMPp2L6iFIkhqTJuXCo1bUjKtskX7WEbTFZCKzyjaf9oip38q1TZOaWmZZ-NRi9_5T6HZQORy1sy-T4v3o7KJuNzaLvIuHY7ESMIgHcp1kIu7PZS2VfEtbYa2D4-aca0xvUTDugB2NMHjBS9eABukO1uA6vXyYgoyxqdUPV5CUyM7HgF2DT7h2ctGkctxUNqrjeW7ju8Qn_32vwai29eq0sAvtw6Q')
--GO
