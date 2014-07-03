/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

merge dbo.PermissionTypes as target using (
	select 1, 'View', 0
	union
	select 2, 'Edit change shopping list title', 0
	union
	select 3, 'Share shopping list with other users', 0
	union
	select 4, 'Delete the shopping list', 0
	union
	select 5, 'Add list items', 1
	union
	select 6, 'Pick or unpick list items', 1
	union
	select 7, 'Remove list items', 1
	union
	select 8, 'Edit list items change description and quantity', 1
) as source (Id, [Description], SelectedDefault)
	on target.Id = source.Id
when matched then
	update set target.[Description] = source.[Description], target.[SelectedDefault] = source.[SelectedDefault]
when not matched then
	insert (Id, [Description], SelectedDefault) values (source.Id, source.[Description], source.[SelectedDefault])
;

merge dbo.ListItemStatuses as target using (
	select 1, 'Not picked'
	union
	select 2, 'Picked'
) as source (Id, [Description])
	on target.Id = source.Id
when matched then
	update set target.[Description] = source.[Description]
when not matched then
	insert (Id, [Description]) values (source.Id, source.[Description])
;
