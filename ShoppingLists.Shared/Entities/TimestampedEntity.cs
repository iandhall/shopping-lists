using System;

namespace ShoppingLists.Shared.Entities
{
    public abstract class TimestampedEntity : Entity
    {
        public string CreatorId { get; set; }
        public string AmenderId { get; set; }

        private DateTime createdDate;
        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        private DateTime? amendedDate;
        public DateTime? AmendedDate
        {
            get { return amendedDate; }
            set
            {
                if (value == null)
                {
                    amendedDate = null;
                }
                else
                {
                    amendedDate = DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
                }
            }
        }

        public User Creator { get; set; } // The User that created this Entity.
        public User Amender { get; set; } // The User that last modified this Entity.
    }
}
