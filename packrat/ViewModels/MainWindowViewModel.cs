using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
namespace packrat.ViewModels;

using packrat.Models;
public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase current_page;
    [ObservableProperty] public string email;
    [ObservableProperty] public string password;
    public ApiClient client=new();

    [RelayCommand] 
    async void Loginn()
    {
        if (await client.login(Email, Password))
        {
            Current_page = new HomeViewModel();
        }
    }
}
