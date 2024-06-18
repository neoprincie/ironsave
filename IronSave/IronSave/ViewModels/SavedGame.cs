using System;

namespace IronSave.ViewModels;

public class SavedGame
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Modified { get; set; }
    public string Path { get; set; }
}