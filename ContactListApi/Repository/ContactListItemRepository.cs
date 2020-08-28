using ContactListApi.Models;

namespace ContactListApi.Repository
{
    public class ContactListItemRepository : Repository<ContactListItem>, IContactListItemRepository
    {
        public ContactListItemRepository(ContactListContext context) : base(context)
        {
        }
    }
}
