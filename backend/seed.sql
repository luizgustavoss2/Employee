USE [DB_FUNCIONARIO]
GO

IF NOT EXISTS (SELECT * FROM  [dbo].[Permission])
BEGIN
	INSERT [dbo].[Permission] ([Description], [CreatedOn]) VALUES ('Listar', getdate());
	INSERT [dbo].[Permission] ([Description], [CreatedOn]) VALUES ('Inserir', getdate());
	INSERT [dbo].[Permission] ([Description], [CreatedOn]) VALUES ('Alterar', getdate());
	INSERT [dbo].[Permission] ([Description], [CreatedOn]) VALUES ('Excluir', getdate());
END

IF NOT EXISTS (SELECT * FROM  [dbo].[User])
BEGIN
	INSERT [dbo].[User] ([Id], [FirstName], [LastName], [Email], [Document], [BirthDate], [Password], [CreatedOn]) VALUES ('e35e04c3-a97f-4da5-9f45-7296929488e0', 'Admin', 'Master', 'admin@admin.com', '123.456.789-01', CAST(N'1980-04-11' AS Date),'OZd1pXH7aew=', getdate());
END

IF NOT EXISTS (SELECT * FROM  [dbo].[UserPermission])
BEGIN
	INSERT [dbo].[UserPermission] ([UserId], [PermissionId], [CreatedOn]) VALUES ((SELECT top 1 ID FROM  [dbo].[User]), 1, getdate());
	INSERT [dbo].[UserPermission] ([UserId], [PermissionId], [CreatedOn]) VALUES ((SELECT top 1 ID FROM  [dbo].[User]), 2, getdate());
	INSERT [dbo].[UserPermission] ([UserId], [PermissionId], [CreatedOn]) VALUES ((SELECT top 1 ID FROM  [dbo].[User]), 3, getdate());
	INSERT [dbo].[UserPermission] ([UserId], [PermissionId], [CreatedOn]) VALUES ((SELECT top 1 ID FROM  [dbo].[User]), 4, getdate());
END

IF NOT EXISTS (SELECT * FROM  [dbo].[UserPhone])
BEGIN
	INSERT [dbo].[UserPhone] ([UserId], [Phone], [CreatedOn]) VALUES ((SELECT top 1 ID FROM  [dbo].[User]), N'(11)98765-4321', getdate());
	INSERT [dbo].[UserPhone] ([UserId], [Phone], [CreatedOn]) VALUES ((SELECT top 1 ID FROM  [dbo].[User]), N'(11)98765-4322', getdate());
END

