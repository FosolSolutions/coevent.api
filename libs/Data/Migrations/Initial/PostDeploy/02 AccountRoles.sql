PRINT 'INSERT [AccountRoles]'

SET IDENTITY_INSERT dbo.[AccountRoles] ON

INSERT INTO dbo.[AccountRoles] (
	[Id]
	, [Name]
	, [Description]
	, [State]
	, [AccountId]
	, [Privileges]
	, [AddedById]
	, [AddedOn]
) VALUES (
	1
	, 'Administrator'
	, 'The administrator of the account.'
	, 1
	, 1
	, 1
	, 1
	, GETUTCDATE()
)

SET IDENTITY_INSERT dbo.[AccountRoles] OFF
