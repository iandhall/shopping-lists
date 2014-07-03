using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingLists.Web.Hubs
{
    public static class ConnectionHolder
    {
        private static object locker = new object();
        private static Dictionary<string, HubConnection> connections = new Dictionary<string, HubConnection>();

        // Adds a new HubConnection if one doesn't already exist, otherwise updates the Group property of the existing HubConnection.
        public static void AddOrUpdate(string connectionId, string group, string userId, string username)
        {
            lock (locker)
            {
                HubConnection connection;
                if (connections.TryGetValue(connectionId, out connection))
                {
                    connection.Group = group;
                }
                else
                {
                    connections.Add(connectionId, new HubConnection() { Id = connectionId, Group = group, UserId = userId, Username = username });
                }
            }
        }

        public static HubConnection Get(string connectionId)
        {
            lock (locker)
            {
                HubConnection connection;
                if (connections.TryGetValue(connectionId, out connection))
                {
                    return connection;
                }
                return null;
            }
        }

        public static IEnumerable<string> GetAllUsernamesInGroup(string group)
        {
            lock (locker)
            {
                return connections.Where(kvp => kvp.Value.Group == group).Select(kvp => kvp.Value.Username).ToList();
            }
        }

        public static void Remove(string connectionId)
        {
            lock (locker)
            {
                HubConnection connection;
                if (connections.TryGetValue(connectionId, out connection))
                {
                    connections.Remove(connectionId);
                }
            }
        }
    }
}