namespace Uni_NET_Pz3.Models;

public class Content
{
    public string Path { get; set; } = "";
    public ContentType ContentType { get; set; } = ContentType.Image;
    public string MimeType { get; set; } = "image/jpeg";
}