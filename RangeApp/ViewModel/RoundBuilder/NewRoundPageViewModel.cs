using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class NewRoundPageViewModel : ObservableObject, IQueryAttributable
{
    public NewRoundPageViewModel()
    {
        RefinedPowders = new ObservableCollection<Models.Powder>();
        RefinedBullets = new ObservableCollection<Models.Bullet>();
        RefinedFirearms = new ObservableCollection<Models.Firearm>();

        UpdatePowders();
        UpdateBullets();
        UpdateFirearms();
    }

    private int RoundId = 0;
    List<Models.Powder> AllPowders = new List<Models.Powder>();
    List<Models.Bullet> AllBullets = new List<Models.Bullet>();
    List<Models.Firearm> AllFirearms = new List<Models.Firearm>();


    [ObservableProperty]
    Models.Powder? selectedPowder;
    [ObservableProperty]
    Models.Bullet? selectedBullet;
    [ObservableProperty]
    Models.Firearm? selectedFirearm;
    [ObservableProperty]
    bool bulletByCaliber = false;
    [ObservableProperty]
    bool queueCheckBox = true;
    [ObservableProperty]
    string nameEntry = string.Empty;
    [ObservableProperty]
    string caliberEntry = string.Empty;
    [ObservableProperty]
    string caseNameEntry = string.Empty;
    [ObservableProperty]
    string lengthEntry = string.Empty;
    [ObservableProperty]
    string primerEntry = string.Empty;
    [ObservableProperty]
    string powderEntry = string.Empty;
    [ObservableProperty]
    string powderWeightEntry = string.Empty;
    [ObservableProperty]
    string bulletEntry = string.Empty;
    [ObservableProperty]
    string firearmEntry = string.Empty;
    [ObservableProperty]
    string statusMessage = string.Empty;

    [ObservableProperty]
    ObservableCollection<Models.Powder> refinedPowders;
    [ObservableProperty]
    ObservableCollection<Models.Bullet> refinedBullets;
    [ObservableProperty]
    ObservableCollection<Models.Firearm> refinedFirearms;

    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null)
            return;
        // for catching return from NewPowderPage
        if (attributes.ContainsKey("AddedPowder"))
        {
            if (attributes["AddedPowder"].ToString() == "1")
            {
                UpdatePowders();
            }
        }
        // for catching return from NewBulletPage
        if (attributes.ContainsKey("AddedBullet"))
        {
            if (attributes["AddedBullet"].ToString() == "1")
            {
                UpdateBullets();
            }
        }
        // for catching return from NewFirearmPage
        if (attributes.ContainsKey("Firearm"))
        {
            var temp = attributes["Firearm"] as ViewModel.FirearmData;

            if (temp != null)
            {
                UpdateFirearms();
            }
        }
        // For catching that the page is updating a RoundData instance 
        if (attributes.ContainsKey("RoundData"))
        {
            var data = attributes["RoundData"] as ViewModel.RoundData;
            if (data != null)
            {
                RoundId = data.RoundId;
                if (data.Name != null)
                    NameEntry = data.Name;
                if (data.Caliber != null)
                    CaliberEntry = data.Caliber;
                if (data.CaseName != null)
                    CaseNameEntry = data.CaseName;
                var length_temp = data.TotalLength.ToString();
                if (length_temp != null)
                    LengthEntry = length_temp;
                if (data.Primer != null)
                    PrimerEntry = data.Primer;
                string? temp = data.PowderWeight.ToString();
                if (temp != null)
                    PowderWeightEntry = temp;

                if (data.PowderId != null)
                {
                    SelectedPowder = App.RoundRepo.GetPowder(data.PowderId);
                    if (SelectedPowder != null && SelectedPowder.Name != null)
                        PowderEntry = SelectedPowder.Name;
                }
                if (data.BulletId != null)
                {
                    SelectedBullet = App.RoundRepo.GetBullet(data.BulletId);
                    if (SelectedBullet != null && SelectedBullet.Name != null)
                        BulletEntry = SelectedBullet.Name;
                }
                SelectedFirearm = App.RoundRepo.GetFirearmForRound(data.RoundId);
                if (SelectedFirearm != null && SelectedFirearm.Name != null)
                    FirearmEntry = SelectedFirearm.Name;

            }
        }
    }

    ///<summary>
    /// Updates the powders in the displayed list
    ///</summary>
    private void UpdatePowders()
    {
        AllPowders = App.RoundRepo.GetPowders();
        FilterPowders();
    }

    ///<summary>
    /// Updates the bullets in the displayed list
    ///</summary>
    private void UpdateBullets()
    {
        AllBullets = App.RoundRepo.GetBullets();
        FilterBullets();
    }

    ///<summary>
    /// Updates the Firearms in the displayed list
    ///</summary>
    private void UpdateFirearms()
    {
        AllFirearms = App.FirearmRepo.GetAllFirearms();
        FilterFirearms();
    }

    ///<summary>
    /// Filters the AllBullets objects based the filters
    ///</summary>
    private void FilterBullets()
    {
        RefinedBullets.Clear();
        if (BulletByCaliber)
        {
            foreach (var bullet in AllBullets)
            {
                if (bullet.Caliber == CaliberEntry)
                    RefinedBullets.Add(bullet);
            }
        }
        else
        {
            foreach (var bullet in AllBullets)
            {
                if (bullet.Name != null && bullet.Name.Contains((string)this.BulletEntry))
                    RefinedBullets.Add(bullet);
            }
        }
    }

    ///<summary>
    /// Filters the AllFirearms objects based the filters
    ///</summary>
    private void FilterFirearms()
    {
        RefinedFirearms.Clear();
        foreach (var firearm in AllFirearms)
        {
            if (firearm.Name != null && firearm.Name.Contains(FirearmEntry))
                RefinedFirearms.Add(firearm);
        }
    }

    ///<summary>
    /// Filters the AllPowders objects based the filters
    ///</summary>
    private void FilterPowders()
    {
        RefinedPowders.Clear();
        foreach (var unit in AllPowders)
        {
            if (unit.Name != null && unit.Name.Contains(PowderEntry))
            {
                RefinedPowders.Add(unit);
            }
        }
    }

    [RelayCommand]
    public void BulletNameTextChanged()
    {
        FilterBullets();
    }

    [RelayCommand]
    public void FirearmNameTextChanged()
    {
        FilterFirearms();
    }
    [RelayCommand]
    public void PowderNameTextChanged()
    {
        FilterPowders();
    }


    [RelayCommand]
    public void BulletByCaliberCheckBox()
    {
        FilterBullets();
    }
    [RelayCommand]
    public async Task NewPowder()
    {
        await Shell.Current.GoToAsync("NewPowderPage");
    }
    [RelayCommand]
    async public Task NewBullet()
    {
        await Shell.Current.GoToAsync("NewBulletPage");
    }
    [RelayCommand]
    async public Task NewFirearm()
    {
        await Shell.Current.GoToAsync("NewFirearmPage");
    }

    [RelayCommand]
    public void PowderSelected()
    {
        if (SelectedPowder != null && SelectedPowder.Name != null)
            PowderEntry = SelectedPowder.Name;
    }
    [RelayCommand]
    public void BulletSelected()
    {
        if (SelectedBullet != null && SelectedBullet.Name != null)
            BulletEntry = SelectedBullet.Name;
    }
    [RelayCommand]
    public void FirearmSelected()
    {
        if (SelectedFirearm != null && SelectedFirearm.Name != null)
            FirearmEntry = SelectedFirearm.Name;

    }
    [RelayCommand]
    public void Save()
    {
        // Checks for bad data
        StatusMessage = string.Empty;
        if (NameEntry == string.Empty)
            StatusMessage += "Round must have a name\n";
        var check = decimal.TryParse(PowderWeightEntry, out decimal weight);
        if (!check)
        {
            if (PowderWeightEntry == string.Empty)
                weight = 0;
            else
                StatusMessage += "Invalid Powder Weight\n";
        }
        check = decimal.TryParse(LengthEntry, out decimal length);
        if (!check)
        {
            if (LengthEntry == string.Empty)
                length = 0;
            else
                StatusMessage += "Invalid Length Entry\n";
        }

        // returns if the data is invalid
        if (StatusMessage != string.Empty)
            return;

        // Saves the bullet to the database
        int bullet_id = 0;
        if (SelectedBullet != null)
            bullet_id = SelectedBullet.Id;

        int powder_id = 0;
        if (SelectedPowder != null)
            powder_id = SelectedPowder.Id;
        var round = new Models.Round
        {
            Id = RoundId,
            Name = NameEntry,
            BulletId = bullet_id,
            Caliber = CaliberEntry,
            PowderGrains = weight,
            PowderId = powder_id,
            CaseName = CaseNameEntry,
            Primer = PrimerEntry,
            TotalLength = length,
            InQueue = QueueCheckBox
        };
        App.RoundRepo.AddNewRound(round);

        int round_id = App.RoundRepo.GetRoundId(round);

        if (SelectedFirearm != null)
        {
            App.RoundRepo.AddRoundToFirearm(SelectedFirearm.Id, round_id);
        }

        var NavigationParemeter = new Dictionary<string, object>
        {
            {"AddedRound", round_id }
        };
        Shell.Current.GoToAsync("..", NavigationParemeter);
        //TODO cannot go back to page if there isn't one
        //await Shell.Current.Navigation.PopToRootAsync();
    }
    [RelayCommand]
    public void Cancel()
    {
        Shell.Current.GoToAsync("..");
    }
}
