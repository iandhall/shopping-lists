declare @userId0 nvarchar(128) = '6D46F8C3-463B-4490-B482-C4EFAA640798';

insert into Users(Id, Username, Discriminator) values
	(@userId0, 'TestUowUser0', 'ApplicationUser');

insert into ShoppingLists(Title, CreatorId, CreatedDate) values
	('Test UOW - Rollback test - Initial state', @userId0, SYSDATETIME());

declare @shoppingListRollbackId bigint = SCOPE_IDENTITY(); 

insert into ShoppingLists(Title, CreatorId, CreatedDate) values
	('Test UOW - Complete test - Initial state', @userId0, SYSDATETIME());

declare @shoppingListCompleteId bigint = SCOPE_IDENTITY(); 

select @userId0 userId0, @shoppingListRollbackId shoppingListRollbackId, @shoppingListCompleteId shoppingListCompleteId;
