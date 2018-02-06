using System;
using System.Linq;
using System.Data.Common;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;
using System.Collections.Generic;

namespace ShoppingLists.IntegrationTests
{
    public class TestUtils
    {
        public static ShoppingListsDbContext CreateDbContext(DbConnection connection, IUserContext userContext = null)
        {
            if (userContext == null)
            {
                userContext = new MockUserContext(NewId());
            }
            
            var dbContext = new ShoppingListsDbContext(connection, userContext);

            if (dbContext.Users.Find(userContext.UserId) == null)
            {
                dbContext.Users.Add(new User { Id = userContext.UserId });
                dbContext.SaveChanges();
            }
            
            if (!dbContext.PermissionTypes.Any())
            {
                dbContext.PermissionTypes.AddRange(new List<PermissionType>()
                {
                    new PermissionType() { Id = Permissions.View, Description = "View", SelectedDefault = false },
                    new PermissionType() { Id = Permissions.Edit, Description = "Edit change shopping list title", SelectedDefault = false },
                    new PermissionType() { Id = Permissions.Share, Description = "Share shopping list with other users", SelectedDefault = false },
                    new PermissionType() { Id = Permissions.Delete, Description = "Delete the shopping list", SelectedDefault = false },
                    new PermissionType() { Id = Permissions.AddListItems, Description = "Add list items", SelectedDefault = true },
                    new PermissionType() { Id = Permissions.PickOrUnpickListItems, Description = "Pick or unpick list items", SelectedDefault = true },
                    new PermissionType() { Id = Permissions.RemoveListItems, Description = "Remove list items", SelectedDefault = true },
                    new PermissionType() { Id = Permissions.EditListItems, Description = "Edit list items change description and quantity", SelectedDefault = true }
                });
                dbContext.SaveChanges();
            }

            return dbContext;
        }
        
        public static string NewId()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetTestShoppingListTitle()
        {
            return "Test shopping list: " + TestUtils.NewId();
        }
    }
}
