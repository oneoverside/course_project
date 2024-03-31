using Microsoft.AspNetCore.Mvc;

namespace Uni_NET_Pz3.Controllers;

public class HomeController : Controller
{
    private readonly MediaGiver _mediaGiver;

    public HomeController(MediaGiver mediaGiver)
    {
        this._mediaGiver = mediaGiver;
    }

    public async Task<IActionResult> Index()
    {
        return this.View();
    }
    
    public IActionResult Gallery()
    {
        return this.View("Gallery", model: this._mediaGiver.NextImage());
    }
    
    public async Task<IActionResult> NextImage()
    {
        var x = await this._mediaGiver.NextImage();
        return this.Ok(x);
    }
    
    public async Task<IActionResult> DelImage(string image)
    {
        return this.Ok(await this._mediaGiver.DelImage(image));
    }
    
    public IActionResult PreviousImage()
    {
        return this.Ok(this._mediaGiver.PreviousImage());
    }
    
    public async Task<IActionResult> GetAll()
    {
        return this.Ok(await this._mediaGiver.GetAll());
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        await this._mediaGiver.AddImage(file);
        return this.Ok();
    }
}