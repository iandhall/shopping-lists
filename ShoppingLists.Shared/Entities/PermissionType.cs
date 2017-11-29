namespace ShoppingLists.Shared.Entities
{
    public class PermissionType
    {
        public Permissions Id { get; set; }
        public string Description { get; set; }
        public bool SelectedDefault { get; set; }
    }
}
