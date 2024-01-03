
using Obskurnee.Services;

namespace Obskurnee.Models;

public class HeaderData
{
    public string OwnerId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string OwnerName { get => UserServiceBase.GetUserName(OwnerId); }

    public HeaderData(string ownerId)
    {
        OwnerId = ownerId;
    }
}
