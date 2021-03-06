﻿using ShoppingLists.Shared;

namespace ShoppingLists.IntegrationTests
{
    public class MockUserContext : IUserContext
    {
        private string _userId;

        public MockUserContext(string userId)
        {
            _userId = userId;
        }

        public string UserId
        {
            get
            {
                return _userId;
            }
        }
    }
}
