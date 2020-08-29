PRINT 'INSERT [Accounts]'

SET IDENTITY_INSERT dbo.[Accounts] ON

INSERT INTO dbo.[Accounts] (
	[Id]
	, [Key]
	, [OwnerId]
	, [State]
	, [Kind]
	, [SubscriptionId]
	, [AddedById]
	, [AddedOn]
) VALUES (
	1
	, NEWID()
	, 1
	, 1
	, 0
	, 1
	, 1
	, GETUTCDATE()
)

SET IDENTITY_INSERT dbo.[Accounts] OFF
