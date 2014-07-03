declare @userId0 nvarchar(128) = 'EBF8AF9E-7D08-4667-ACEC-B4A7DE182DAC';

insert into Users(Id, Username, Discriminator) values
	(@userId0, 'TestLiRepoUser0', 'ApplicationUser');

begin
	insert into ShoppingLists(Title, CreatorId, CreatedDate) values
		('LiRepo - Test list to get', @userId0, SYSDATETIME());

	declare @shoppingListGetId bigint = SCOPE_IDENTITY(); 

	insert into ListItems([Description], ShoppingListId, StatusId, CreatorId, CreatedDate, Quantity) values
		('LiRepo - Test ListItem to get', @shoppingListGetId, 0, @userId0, SYSDATETIME(), 1);

	declare @listItemGetId bigint = SCOPE_IDENTITY(); 
	
	insert into ListItems([Description], ShoppingListId, StatusId, CreatorId, CreatedDate, Quantity) values
		('LiRepo - Test ListItem to update', @shoppingListGetId, 0, @userId0, SYSDATETIME(), 1);

	declare @listItemUpdateId bigint = SCOPE_IDENTITY();
	
	insert into ListItems([Description], ShoppingListId, StatusId, CreatorId, CreatedDate, Quantity) values
		('LiRepo - Test ListItem to update', @shoppingListGetId, 0, @userId0, SYSDATETIME(), 1);

	declare @listItemDeleteId bigint = SCOPE_IDENTITY();
		
	insert into ListItems([Description], ShoppingListId, StatusId, CreatorId, CreatedDate, Quantity) values
		('LiRepo - To be matched by description.', @shoppingListGetId, 0, @userId0, SYSDATETIME(), 1);

	declare @listItemGetByDescId bigint = SCOPE_IDENTITY();
end

begin
	insert into ShoppingLists(Title, CreatorId, CreatedDate) values
		('LiRepo - Test list 2', @userId0, SYSDATETIME());

	declare @shoppingListUnpickAllId bigint = SCOPE_IDENTITY(); 

	insert into ListItems([Description], ShoppingListId, StatusId, CreatorId, CreatedDate, Quantity) values
		('LiRepo - Test ListItem to unpick1', @shoppingListUnpickAllId, 0, @userId0, SYSDATETIME(), 2),
		('LiRepo - Test ListItem to unpick2', @shoppingListUnpickAllId, 0, @userId0, SYSDATETIME(), 2),
		('LiRepo - Test ListItem to unpick3', @shoppingListUnpickAllId, 0, @userId0, SYSDATETIME(), 2),
		('LiRepo - Test ListItem to unpick4', @shoppingListUnpickAllId, 0, @userId0, SYSDATETIME(), 2),
		('LiRepo - Test ListItem to unpick5', @shoppingListUnpickAllId, 0, @userId0, SYSDATETIME(), 2);
end

select
	@userId0 userId0,
	@shoppingListGetId shoppingListGetId,
	@shoppingListUnpickAllId shoppingListUnpickAllId,
	@listItemGetId listItemGetId,
	@listItemGetByDescId listItemGetByDescId,
	@listItemUpdateId listItemUpdateId,
	@listItemDeleteId listItemDeleteId;
