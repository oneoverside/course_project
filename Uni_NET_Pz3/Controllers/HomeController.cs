using Microsoft.AspNetCore.Mvc;

namespace Uni_NET_Pz3.Controllers;

public class HomeController : Controller
{
    private readonly MediaGiver _mediaGiver;

    public HomeController(MediaGiver mediaGiver)
    {
        this._mediaGiver = mediaGiver;
    }

    public IActionResult Index()
    {
        return this.View();
    }
    
    public IActionResult Gallery()
    {
        return this.View("Gallery", model: this._mediaGiver.NextImage());
    }
    
    public IActionResult NextImage()
    {
        return this.Ok(this._mediaGiver.NextImage());
    }
    
    public IActionResult DelImage(string image)
    {
        return this.Ok(this._mediaGiver.DelImage(image));
    }
    
    public IActionResult PreviousImage()
    {
        return this.Ok(this._mediaGiver.PreviousImage());
    }
    
    public IActionResult GetAll()
    {
        return this.Ok(this._mediaGiver.GetAll());
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        await this._mediaGiver.UploadImage(file);
        return this.Ok();
    }
}

public class MediaGiver
{
    private readonly List<string> _images;
    private readonly List<string> _videos;
    
    private readonly Random _rnd;
    private readonly List<Model> _usedFiles = new List<Model>();
    private int _usedIndex = 0;

    public MediaGiver()
    {
        const string imagesPath = @"wwwroot\Home\images";
        this._images = Directory.GetFiles(imagesPath, "*.*").ToList();
        
        const string videosPath = @"wwwroot\Home\videos";
        this._videos = Directory.GetFiles(videosPath, "*.*").ToList();

        this._rnd = new Random();
        
        // var filePath = this._images[this._rnd.Next(0, this._images.Count - 1)][8..];
        // this._usedFiles.Add(filePath);
    }
    
    public Model NextImage()
    {
        if (this._usedIndex >= this._usedFiles.Count)
        {
            var type = this._rnd.Next(0, 1);
            if (type == 0) 
            {
                this._usedIndex++;
                var image = this._images[this._rnd.Next(0, this._images.Count - 1)][13..];
                var model = new Model { Path = image, Type = "image" };
                this._usedFiles.Add(model);
                return model;
            }
            else
            {
                this._usedIndex++;
                var image = this._videos[this._rnd.Next(0, this._videos.Count - 1)][13..];
                var model = new Model { Path = image, Type = "video" };
                this._usedFiles.Add(model);
                return model;
            }
        }
        else
        {
            var image = this._usedFiles[this._usedIndex];
            this._usedIndex++;
            return image;
        }
    }
    
    public Model DelImage(string image)
    {
        var newFilePath = $@"wwwroot\Home\images\deleted\{image[13..]}";
        var oldPath = $@"wwwroot\Home\{image}";
        Directory.Move(oldPath, newFilePath);
        this._usedIndex--;
        var file = this._usedFiles.First(x => x.Path == image);
        this._usedFiles.Remove(file);
        this._images.Remove($@"wwwroot\Home\{image}");
        return this.NextImage();
    }
    
    public async Task UploadImage(IFormFile file)
    {
        var directoryPath = Path.Combine("wwwroot", "Home", "images");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Construct the new file path
        var newFilePath = Path.Combine(directoryPath, file.FileName);

        // Ensure the file name is unique to avoid overwriting existing files
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);
        var counter = 1;
        while (File.Exists(newFilePath))
        {
            var newFileName = $"{fileNameWithoutExtension}({counter}){extension}";
            newFilePath = Path.Combine(directoryPath, newFileName);
            counter++;
        }

        // Save the file
        this._images.Insert(0, newFilePath);
        await using var stream = new FileStream(newFilePath, FileMode.Create);
        await file.CopyToAsync(stream);
    }
    
    public Model PreviousImage()
    {
        if (this._usedIndex > 1)    
        {
            this._usedIndex--;
        }
        return this._usedFiles[this._usedIndex-1];
    }

    public HashSet<string> GetAll()
    {
        var response = new List<string>();
        response.AddRange(this._videos);
        response.AddRange(this._images);
        return response.Select(x => $@"..\Home\{x[13..]}").ToHashSet();
    }
}

public class Model
{
    public string Path { get; set; } = null!;
    public string Type { get; set; } = null!;
}