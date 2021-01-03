using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public record Discussion(int Id, string Title, string Description, bool isArchived = false);

    public record Post (int Id, int DiscussionId, string BookName, string Text, int? PageCount = null, string Url = null);
}
