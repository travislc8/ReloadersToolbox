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

    private Models.Powder? SelectedPowder;
    private Models.Bullet? SelectedBullet;
    private Models.Firearm? SelectedFirearm;
    List<Models.Powder> AllPowders = new List<Models.Powder>();
    List<Models.Bullet> AllBullets = new List<Models.Bullet>();
    List<Models.Firearm> AllFirearms = new List<Models.Firearm>();
    private bool QueueCheckBox = true;
    private bool BulletByCaliber = false;
    private int RoundId = 0;
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
    ObservableCollection<Models.Powder> refinedPowders;
    [ObservableProperty]
    ObservableCollection<Models.Bullet> refinedBullets;
    [ObservableProperty]
    ObservableCollection<Models.Firearm> refinedFirearms;

    private void UpdatePowders()
    {
        AllPowders = App.RoundRepo.GetPowders();
        FilterPowders();
    }
    private void UpdateBullets()
    {
        AllBullets = App.RoundRepo.GetBullets();
        FilterBullets();
    }
    private void UpdateFirearms()
    {
        AllFirearms = App.FirearmRepo.GetAllFirearms();
        FilterFirearms();
    }
    private void FilterBullets()
    {
        RefinedBullets.Clear();
        if (BulletByCaliber)
        {
            foreach (var bullet in AllBullets)
            {
                if (bullet.Diameter == CaliberEntry  && bullet.Name != null && bullet.Name.Contains((string)this.BulletEntry ))
                    RefinedBullets.Add(bullet);
            }
        }
        else
        {
            foreach (var bullet in AllBullets)
            {
                if (bullet.Name != null && bullet.Name.Contains((string)this.BulletEntry ))
                    RefinedBullets.Add(bullet);
            }
        }
    }
    private void FilterFirearms()
    {
        RefinedFirearms.Clear();
        foreach (var firearm in AllFirearms)
        {
            if (firearm.Name != null && firearm.Name.Contains((string)this.FirearmEntry))
                RefinedFirearms.Add(firearm);
        }
    }
    private void FilterPowders()
    {
        RefinedPowders.Clear();
        foreach (var unit in AllPowders)
        {
            if (unit.Name != null && unit.Name.Contains(PowderEntry ))
            {
                RefinedPowders.Add(unit);
            }
        }
    }
    public bool PowderNameTextChanged()
    {
        FilterPowders();
        return true;
    }
    public bool BulletNameTextChanged()
    {
        FilterBullets();
        return true;
    }
    public bool FirearmNameTextChanged()
    {
        FilterFirearms();
        return true;
    }
    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null)
            return;
        if (attributes.ContainsKey("AddedPowder"))
        {
            if (attributes["AddedPowder"].ToString() == "1")
            {
                UpdatePowders();
            }
        }
        if (attributes.ContainsKey("AddedBullet"))
        {
            if (attributes["AddedBullet"].ToString() == "1")
            {
                UpdateBullets();
            }
        }
        if (attributes.ContainsKey("firearm"))
        {
            var temp = attributes["firearm"] as Models.Firearm;

            if (temp != null)
            {
                UpdateFirearms();
            }
        }
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
                if (data.TotalLength != null)
                    LengthEntry = data.TotalLength.ToString();
                if (data.Primer != null)
                    PrimerEntry = data.Primer;
                if (data.PowderWeight != null)
                    PowderWeightEntry = data.PowderWeight.ToString();

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
    public void QueueCheckBoxChecked(bool check) {
        QueueCheckBox = check;  
    }
    public void BulletByCaliberCheckBoxChanged(bool check) {
        BulletByCaliber = check;
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
    public void PowderSelected(int index)
    {
        SelectedPowder = RefinedPowders[index];
        if (SelectedPowder != null && SelectedPowder.Name != null)
            PowderEntry  = SelectedPowder.Name;
        RefinedPowders.Clear();
        foreach (var unit in AllPowders)
        {
            if (unit.Name != null && unit.Name == SelectedPowder.Name)
            {
                RefinedPowders.Add(unit);
            }
        }
    }
    public void BulletSelected(int index)
    {
        SelectedBullet = RefinedBullets[index];
        if (SelectedBullet != null && SelectedBullet.Name != null)
            BulletEntry  = SelectedBullet.Name;
        RefinedBullets.Clear();
        foreach (var unit in AllBullets)
        {
            if (unit.Name != null && unit.Name == SelectedBullet.Name)
            {
                RefinedBullets.Add(unit);
            }
        }
    }
    public void FirearmSelected(int index)
    {
        SelectedFirearm = RefinedFirearms[index];
        if (SelectedFirearm != null && SelectedFirearm.Name != null)
            FirearmEntry = SelectedFirearm.Name;
        RefinedFirearms.Clear();
        foreach (var unit in AllFirearms)
        {
            if (unit.Name != null && unit.Name == SelectedFirearm.Name)
            {
                RefinedFirearms.Add(unit);
            }
        }
    }
    [RelayCommand]
    public void Save()
    {
        var check = int.TryParse(PowderWeightEntry, out int weight);
        if (!check)
        {
            weight = 0;
        }
        check = decimal.TryParse(LengthEntry, out decimal length);
        if (!check)
        {
            length = 0;
        }

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
        
        Shell.Current.GoToAsync("..",NavigationParemeter);
    }
    [RelayCommand]
    public void Cancel()
    {
        Shell.Current.GoToAsync("..");
    }
}
