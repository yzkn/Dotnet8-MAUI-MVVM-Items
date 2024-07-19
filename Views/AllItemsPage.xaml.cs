namespace Items.Views;

public partial class AllItemsPage : ContentPage
{
    public AllItemsPage()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        itemsCollection.SelectedItem = null;
    }
}