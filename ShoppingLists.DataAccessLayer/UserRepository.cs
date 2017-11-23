using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;
using System.Data.Entity;

namespace ShoppingLists.DataAccessLayer
{
    public class UserRepository
    {
        private ShoppingListsDbContext dbContext;

        public UserRepository(ShoppingListsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public User Get(string id, bool includePermissions = false, long? shoppingListId = null)
        {
            User user = dbContext.Users.Find(id);
            if (includePermissions)
            {
                if (shoppingListId == null)
                {
                    throw new ArgumentNullException("shoppingListId cannot be null when includePermissions is set to true.");
                }
                dbContext.Entry(user).Collection(u => u.ShoppingListPermissions).Query().Where(p => p.ShoppingListId == shoppingListId).Load();
            }
            return user;
        }

        public void Create(User entity)
        {
            dbContext.Users.Add(entity);
        }

        public void Update(User entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(string id)
        {
            var entity = dbContext.Users.Find(id);
            dbContext.Users.Remove(entity);
        }

        // Should compare Username column with case sensitivity. Set collation to Latin1_General_CS_AS on the column in the database.
        public User FindByName(string username)
        {
            return dbContext.Users.FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<User> FindAllForShoppingList(long shoppingListId)
        {
            return dbContext.Users.Where(u =>
                u.ShoppingListPermissions.Any(p =>
                    p.PermissionTypeId == Permissions.View
                    && p.ShoppingListId == shoppingListId
                )
            ).ToList();
        }
    }
}
