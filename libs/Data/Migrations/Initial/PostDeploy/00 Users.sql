PRINT 'INSERT [Users]'

SET IDENTITY_INSERT dbo.[Users] ON

INSERT INTO dbo.[Users] (
	[Id]
	, [Key]
	, [Username]
	, [Email]
	, [State]
	, [AddedOn]
) VALUES (
	1
	, NEWID()
	, 'admin@fosol.ca'
	, 'admin@fosol.ca'
	, 1
	, GETUTCDATE()
)

INSERT INTO dbo.[UserInfo] (
	[UserId]
	, [FirstName]
	, [LastName]
	, [AddedById]
	, [AddedOn]
) VALUES (
	1
	, 'Administrator'
	, 'Administrator'
	, 1
	, GETUTCDATE()
)

UPDATE dbo.[Subscriptions]
SET AddedById = 1

SET IDENTITY_INSERT dbo.[Users] OFF
