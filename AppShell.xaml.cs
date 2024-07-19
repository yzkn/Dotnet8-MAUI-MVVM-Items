namespace Items;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Views.ItemPage), typeof(Views.ItemPage));
    }
}