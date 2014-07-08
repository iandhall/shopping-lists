declare @userId0 nvarchar(128) = 'cfbf134b-705e-4cde-947b-d1c5d6c32062';
declare @userId1 nvarchar(128) = 'e6db5c8b-9793-4a2e-8d11-eccf177af70e';
declare @userId2 nvarchar(128) = '6dfd3e83-0000-421a-b104-216d4b208ef3';
declare @userId3 nvarchar(128) = '9zfd3e83-0000-421a-b104-216d4b208ez9';

begin
	insert into Users(Id, Username, Discriminator) values
		(@userId0, 'TestSlRepoUser0', 'ApplicationUser'),
		(@userId1, 'TestSlRepoUser1', 'ApplicationUser'),
		(@userId2, 'TestSlRepoUser2', 'ApplicationUser'),
		(@userId3, 'TestSlRepoUser3', 'ApplicationUser');

	declare @username0 nvarchar(max) = (select Username from Users where Id = @userId0);
	declare @username1 nvarchar(max) = (select Username from Users where Id = @userId1);
	declare @username2 nvarchar(max) = (select Username from Users where Id = @userId2);
	declare @username3 nvarchar(max) = (select Username from Users where Id = @userId3);
end

begin
	insert into ShoppingLists(Title, CreatorId, CreatedDate) values
		('SlRepo - Test list to get', @userId0, SYSDATETIME());

	declare @shoppingListGetId bigint = SCOPE_IDENTITY(); 

	insert into ListItems([Description], ShoppingListId, StatusId, CreatorId, CreatedDate, Quantity) values
		('SlRepo - Test ListItem #1', @shoppingListGetId, 0, @userId0, SYSDATETIME(), 1),
		('SlRepo - Test ListItem #2', @shoppingListGetId, 0, @userId0, SYSDATETIME(), 1);

	insert into ShoppingListPermissions(PermissionTypeId, UserId, ShoppingListId, CreatorId, CreatedDate) values
		({{Permissions.View}}, @userId0, @shoppingListGetId, @userId0, SYSDATETIME()),
		({{Permissions.EditListItems}}, @userId0, @shoppingListGetId, @userId0, SYSDATETIME()),
		({{Permissions.Share}}, @userId0, @shoppingListGetId, @userId0, SYSDATETIME()),
		({{Permissions.View}}, @userId1, @shoppingListGetId, @userId0, SYSDATETIME()),
		({{Permissions.EditListItems}}, @userId1, @shoppingListGetId, @userId0, SYSDATETIME()),
		({{Permissions.View}}, @userId2, @shoppingListGetId, @userId0, SYSDATETIME());
end

begin
	insert into ShoppingLists(Title, CreatorId, CreatedDate) values
		('SlRepo - Test list to delete', @userId0, SYSDATETIME());

	declare @shoppingListDeleteId bigint = SCOPE_IDENTITY(); 

	insert into ListItems([Description], ShoppingListId, StatusId, CreatorId, CreatedDate, Quantity) values
		('SlRepo - Test delete ListItem #1', @shoppingListDeleteId, 0, @userId0, SYSDATETIME(), 1),
		('SlRepo - Test delete ListItem #2', @shoppingListDeleteId, 0, @userId0, SYSDATETIME(), 1);
		
	insert into ShoppingListPermissions(PermissionTypeId, UserId, ShoppingListId, CreatorId, CreatedDate) values
		({{Permissions.View}}, @userId0, @shoppingListDeleteId, @userId0, SYSDATETIME()),
		({{Permissions.PickOrUnpickListItems}}, @userId0, @shoppingListDeleteId, @userId0, SYSDATETIME());
end

begin
	insert into ShoppingLists(Title, CreatorId, CreatedDate) values
		('SlRepo - Test list to update', @userId0, SYSDATETIME());

	declare @shoppingListUpdateId bigint = SCOPE_IDENTITY(); 
end

insert into ShoppingLists(Title, CreatorId, CreatedDate) values
	('SlRepo - Many per user a', @userId1, SYSDATETIME()),
	('SlRepo - Many per user 1', @userId1, SYSDATETIME()),
	('SlRepo - Many per user 2', @userId2, SYSDATETIME()),
	('SlRepo - Many per users', @userId1, SYSDATETIME()),
	('SlRepo - Many per user user', @userId1, SYSDATETIME()),
	('SlRepo - Many per user 10', @userId1, SYSDATETIME());

insert into ShoppingLists(Title, CreatorId, CreatedDate) values
	('SlRepo - FindAllForUser3#1', @userId3, SYSDATETIME()),
	('SlRepo - FindAllForUser3#2', @userId3, SYSDATETIME()),
	('SlRepo - FindAllForUser3#3', @userId3, SYSDATETIME());

select
	@userId0 userId0,
	@userId1 userId1,
	@userId2 userId2,
	@userId3 userId3,
	@username0 username0,
	@username1 username1,
	@username2 username2,
	@username3 username3,
	@shoppingListGetId shoppingListGetId,
	@shoppingListUpdateId shoppingListUpdateId,
	@shoppingListDeleteId shoppingListDeleteId;
