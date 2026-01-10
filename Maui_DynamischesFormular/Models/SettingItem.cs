using CommunityToolkit.Mvvm.ComponentModel;

public partial class SettingItem : ObservableObject
{
    public string Name { get; set; }
    public SettingType Type { get; set; }

    [ObservableProperty]
    private string stringValue;

    [ObservableProperty]
    private bool boolValue;

    [ObservableProperty]
    private DateTime dateValue = DateTime.Today;
}

public enum SettingType
{
    Text,
    Boolean,
    Date
}
