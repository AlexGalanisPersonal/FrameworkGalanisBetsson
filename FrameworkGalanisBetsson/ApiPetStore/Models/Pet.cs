namespace SauceDemoUiBetsson.ApiPetStore.Models;

public class Pet
{
    public long Id { get; set; }
    public Category? Category { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> PhotoUrls { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    public string Status { get; set; } = string.Empty;
}

public class Category
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class Tag
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}