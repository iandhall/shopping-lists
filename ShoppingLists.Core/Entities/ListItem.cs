namespace ShoppingLists.Core.Entities
{
    public class ListItem : TimestampedEntity
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public Statuses StatusId { get; set; }
        public long ShoppingListId { get; set; }

        public ShoppingList ShoppingList { get; set; }
    }
}
