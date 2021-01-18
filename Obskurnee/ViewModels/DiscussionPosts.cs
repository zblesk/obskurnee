using Obskurnee.Models;
using System.Collections.Generic;

namespace Obskurnee.ViewModels
{
    public record DiscussionPosts (Discussion Discussion, List<Post> posts);
}
