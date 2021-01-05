using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.ViewModels
{
    public record DiscussionPosts (Discussion Discussion, List<Post> posts);
}
