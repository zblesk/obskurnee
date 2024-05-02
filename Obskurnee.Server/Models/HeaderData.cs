
using Obskurnee.Services;

namespace Obskurnee.Models;

public class HeaderData(string ownerId)
{
    public string OwnerId { get; set; } = ownerId;
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string OwnerName { get => UserServiceBase.GetUserName(OwnerId); }
}
