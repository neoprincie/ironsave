using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ReactiveUI;

namespace IronSave.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Campaign _selectedCampaign;
    private ObservableCollection<SavedGame> _savedGames;
    public string ParadoxSaveFolder => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Paradox Interactive/Europa Universalis IV/save games";
    public string IronSaveFolder => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/IronSave";

    public Campaign SelectedCampaign
    {
        get => _selectedCampaign;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedCampaign, value);
            LoadSaves();
        }
    }

    public ObservableCollection<Campaign> Campaigns { get; set; }
    
    public SavedGame SelectedSavedGame { get; set; }

    public ObservableCollection<SavedGame> SavedGames
    {
        get => _savedGames;
        set => this.RaiseAndSetIfChanged(ref _savedGames, value);
    }
    
    public string Description { get; set; }

    public MainViewModel()
    {
        LoadCampaigns();
    }

    public void Save()
    {
        var saveName = DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + "_" + Description;
        
        //if(!Directory.Exists(IronSaveFolder + "/" + SelectedCampaign.Name + "/" + saveName))
        Directory.CreateDirectory(IronSaveFolder + "/" + SelectedCampaign.Name + "/" + saveName);
        
        File.Copy(ParadoxSaveFolder + "/" + SelectedCampaign.Name + ".eu4",
            IronSaveFolder + "/" + SelectedCampaign.Name + "/" + saveName + "/" + SelectedCampaign.Name + ".eu4");
        File.Copy(ParadoxSaveFolder + "/" + SelectedCampaign.Name + "_backup.eu4",
            IronSaveFolder + "/" + SelectedCampaign.Name + "/" + saveName + "/" + SelectedCampaign.Name + "_backup.eu4");
        
        LoadSaves();
    }

    public void QuickSave()
    {
        var saveName = DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + "_" + "Quick save";
        
        //if(!Directory.Exists(IronSaveFolder + "/" + SelectedCampaign.Name + "/" + saveName))
        Directory.CreateDirectory(IronSaveFolder + "/" + SelectedCampaign.Name + "/" + saveName);
        
        File.Copy(ParadoxSaveFolder + "/" + SelectedCampaign.Name + ".eu4",
            IronSaveFolder + "/" + SelectedCampaign.Name + "/" + saveName + "/" + SelectedCampaign.Name + ".eu4");
        File.Copy(ParadoxSaveFolder + "/" + SelectedCampaign.Name + "_backup.eu4",
            IronSaveFolder + "/" + SelectedCampaign.Name + "/" + saveName + "/" + SelectedCampaign.Name + "_backup.eu4");
        
        LoadSaves();
    }

    public void Load()
    {
        File.Copy(SelectedSavedGame.Path + "/" + SelectedCampaign.Name + ".eu4",
            ParadoxSaveFolder + "/" + SelectedCampaign.Name + ".eu4", true);
        File.Copy(SelectedSavedGame.Path + "/" + SelectedCampaign.Name + "_backup.eu4",
            ParadoxSaveFolder + "/" + SelectedCampaign.Name + "_backup.eu4", true);
        // File.Copy(ParadoxSaveFolder + "/" + SelectedCampaign.Name + ".eu4",
        //     IronSaveFolder + "/" + SelectedCampaign.Name + "/" + saveName + "/" + SelectedCampaign.Name + ".eu4");
    }

    public void OnCampaignChanged()
    {
        
    }
    
    private void LoadCampaigns()
    {
        Campaigns = new ObservableCollection<Campaign>();

        var files = Directory.GetFiles(ParadoxSaveFolder)
            .Where(f => f.EndsWith(".eu4") && !f.EndsWith("_Backup.eu4"))
            .OrderByDescending(f => File.GetLastWriteTime(f));

        foreach (var f in files)
        {
            Campaigns.Add(new()
            {
                Name = Path.GetFileName(f).Split(".")[0],
                Modified = File.GetLastWriteTime(f)
            });
        }

        SelectedCampaign = Campaigns.FirstOrDefault();
    }

    private void LoadSaves()
    {
        SavedGames = new ObservableCollection<SavedGame>();

        if (!Directory.Exists(IronSaveFolder + "/" + SelectedCampaign.Name))
            return;

        var saveFolders = Directory.GetDirectories(IronSaveFolder + "/" + SelectedCampaign.Name).OrderByDescending(s => s);

        foreach (var folder in saveFolders)
        {
            var folderParts = Path.GetFileName(folder).Split("_");
            
            SavedGames.Add(new()
            {
                Name = folderParts[0],
                Description = folderParts[1],
                Modified = File.GetLastWriteTime(folder + "/" + SelectedCampaign.Name + ".eu4"),
                Path = folder
            });
        }
    }
}