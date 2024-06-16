using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace IronSave.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to IronSave!";
    public string ParadoxSaveFolder => "/Users/username/Documents/Paradox Interactive/Europa Universalis IV/save games";
    public string IronSaveFolder => "/Users/username/Documents/IronSave";
    public string CampaignName => "Chill";
    public ObservableCollection<Campaign> Campaigns { get; set; }

    public MainViewModel()
    {
        Campaigns = new ObservableCollection<Campaign>()
        {
            new() { Name ="Chill" },
            new() { Name ="England" }
        };
    }

    public void Save()
    {
        // File.Copy(ParadoxSaveFolder + "/" + CampaignName + ".eu4",
        //     IronSaveFolder + "/" + CampaignName + ".eu4");
    }
}