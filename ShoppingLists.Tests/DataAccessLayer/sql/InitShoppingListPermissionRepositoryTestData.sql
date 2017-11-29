declare @userId0 nvarchar(128) = 'FD96CCE1-8712-4002-80A7-F619605FA46E';
declare @userId1 nvarchar(128) = '66D29A44-9A68-4507-93F7-B9ED6C999E77';
declare @userId2 nvarchar(128) = '1BCF19A5-0916-4D27-BFA0-617F4058F63C';
declare @userId3 nvarchar(128) = 'CEA2E5F9-337C-4862-B1C4-00D064B392AA';

insert into Users(Id, Username, Discriminator) values
	(@userId0, 'TestSlpRepoUser0', 'ApplicationUser'),
	(@userId1, 'TestSlpRepoUser1', 'ApplicationUser'),
	(@userId2, 'TestSlpRepoUser2', 'ApplicationUser'),
	(@userId3, 'TestSlpRepoUser3', 'ApplicationUser');

begin
	insert into ShoppingLists(Title, CreatorId, CreatedDate) values
		('SlpRepo - Test list', @userId0, SYSDATETIME());

	declare @shoppingListGetId bigint = SCOPE_IDENTITY(); 

	insert into ShoppingListPermissions(PermissionTypeId, UserId, ShoppingListId, CreatorId, CreatedDate) values
		({{Permissions.View}}, @userId0, @shoppingListGetId, @userId0, SYSDATETIME()),
		({{Permissions.EditListItems}}, @userId0, @shoppingListGetId, @userId0, SYSDATETIME()),
		({{Permissions.View}}, @userId1, @shoppingListGetId, @userId0, SYSDATETIME()),
		({{Permissions.EditListItems}}, @userId1, @shoppingListGetId, @userId0, SYSDATETIME()),
		({{Permissions.View}}, @userId2, @shoppingListGetId, @userId0, SYSDATETIME());

	insert into ShoppingListPermissions(PermissionTypeId, UserId, ShoppingListId, CreatorId, CreatedDate) values
		({{Permissions.Share}}, @userId0, @shoppingListGetId, @userId0, SYSDATETIME());

	declare @slpShareId bigint = SCOPE_IDENTITY();  
end

select
	@userId0 userId0,
	@userId1 userId1,
	@userId2 userId2,
	@userId3 userId3,
	@shoppingListGetId shoppingListGetId,
	@slpShareId slpShareId
