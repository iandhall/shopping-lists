declare @userId0 nvarchar(128) = '63C9D959-112A-4A25-9298-AE0D3CBAF4F6';
declare @userId1 nvarchar(128) = 'B9193A5A-AC2D-4228-8C56-6E26BD7DE057';
declare @userId2 nvarchar(128) = '992BB2DA-A95E-4C31-A29C-ED12EEF50C7F';
declare @userId3 nvarchar(128) = '671425EE-9586-4579-84B2-61DE7C1784D8';

insert into Users(Id, Username, Discriminator) values
	(@userId0, 'TestUserRepoUser0', 'ApplicationUser'),
	(@userId1, 'TestUserRepoUser1', 'ApplicationUser'),
	(@userId2, 'TestUserRepoUser2', 'ApplicationUser'),
	(@userId3, 'TestUserRepoUser3', 'ApplicationUser');

insert into ShoppingLists(Title, CreatorId, CreatedDate) values
	('UserRepo - Test list to get', @userId0, SYSDATETIME());

declare @shoppingListGetId bigint = SCOPE_IDENTITY(); 

insert into ShoppingLists(Title, CreatorId, CreatedDate) values
	('UserRepo - Test list another one for TestUserRepoUser3', @userId3, SYSDATETIME());

declare @shoppingListOtherId bigint = SCOPE_IDENTITY(); 

insert into ShoppingListPermissions(PermissionTypeId, UserId, ShoppingListId, CreatorId, CreatedDate) values
	({{Permissions.View}}, @userId0, @shoppingListOtherId, @userId0, SYSDATETIME()),
	({{Permissions.View}}, @userId0, @shoppingListGetId, @userId0, SYSDATETIME()),
	({{Permissions.EditListItems}}, @userId0, @shoppingListGetId, @userId0, SYSDATETIME()),
	({{Permissions.Share}}, @userId0, @shoppingListGetId, @userId0, SYSDATETIME()),
	({{Permissions.View}}, @userId1, @shoppingListGetId, @userId0, SYSDATETIME()),
	({{Permissions.EditListItems}}, @userId1, @shoppingListGetId, @userId0, SYSDATETIME()),
	({{Permissions.View}}, @userId2, @shoppingListGetId, @userId0, SYSDATETIME()),
	({{Permissions.View}}, @userId3, @shoppingListGetId, @userId0, SYSDATETIME()),
	({{Permissions.View}}, @userId3, @shoppingListOtherId, @userId0, SYSDATETIME()),
	({{Permissions.Share}}, @userId3, @shoppingListOtherId, @userId0, SYSDATETIME());

select @userId0 userId0, @userId1 userId1, @userId2 userId2, @userId3 userId3, @shoppingListGetId shoppingListGetId;
