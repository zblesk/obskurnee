using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obskurnee.Services;

namespace Obskurnee.Controllers;

[Route("images")]
[AllowAnonymous]
public class ImageController : Controller
{
    private readonly ApplicationDbContext _db;

    public ImageController(
        ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [Route("{imageName}")]
    [ResponseCache(Duration = 31536000, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Get(string imageName)
    {
        var image = await _db.Images.AsNoTracking().FirstOrDefaultAsync(i => i.FileName == imageName);
        if (image != null)
        {
            return File(image.FileContents, ExtensionToMime(image.Extension));
        }
        return await Task.FromResult((IActionResult)NotFound());
    }
}
