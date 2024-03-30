using Microsoft.EntityFrameworkCore;
using Uni_NET_Pz3.DB;

namespace Uni_NET_Pz3.Controllers;

public class MediaGiver
{
    private readonly SqlLiteDbContext _dbContext;
    private readonly List<Model> _usedFiles = [];
    private int _usedIndex;

    public MediaGiver(SqlLiteDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    public async Task<Model> NextImage()
    {
        if (this._usedIndex >= this._usedFiles.Count)
        {
            this._usedIndex++;
            // TODO: get from DB
            var image = await this._dbContext.Contents.OrderBy(r => Guid.NewGuid()).FirstAsync();
            var model = new Model { Path = image.Path, Type = "image" };
            this._usedFiles.Add(model);
            return model;
        }
        else
        {
            var image = this._usedFiles[this._usedIndex];
            this._usedIndex++;
            return image;
        }
    }
    
    public async Task<Model> DelImage(string image)
    {
        var newFilePath = $@"wwwroot\Home\images\deleted\{image[13..]}";
        var oldPath = $@"wwwroot\Home\{image}";
        
        Directory.Move(oldPath, newFilePath);
        this._usedIndex--;
        
        var file = this._usedFiles.First(x => x.Path == image);
        this._usedFiles.Remove(file);
        
        var fileInDb = await this._dbContext.Contents.FirstAsync(x => x.Path == $@"wwwroot\Home\{image}");
        this._dbContext.Contents.Remove(fileInDb);
        
        return await this.NextImage();
    }
    
    public async Task AddImage(IFormFile file)
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

        this._dbContext.Contents.Add(new Content
        {
            Path = newFilePath,
            Name = file.FileName
        });

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

    public async Task<HashSet<string>> GetAll()
    {
        var images = await this._dbContext.Contents.ToListAsync();
        return images.Select(x => $@"..\Home\{x.Path[13..]}").ToHashSet();
    }
}

public class Model
{
    public string Path { get; set; }
    public string Type { get; set; }
}