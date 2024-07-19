using CommunityToolkit.Mvvm.Input;
using Items.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Items.ViewModels;

internal class ItemsViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.ItemViewModel> AllItems { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectItemCommand { get; }

    public ItemsViewModel()
    {
        AllItems = new ObservableCollection<ViewModels.ItemViewModel>(Models.Item.LoadAll().Select(n => new ItemViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewItemAsync);
        SelectItemCommand = new AsyncRelayCommand<ViewModels.ItemViewModel>(SelectItemAsync);
    }

    private async Task NewItemAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.ItemPage));
    }

    private async Task SelectItemAsync(ViewModels.ItemViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.ItemPage)}?load={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            ItemViewModel matchedItem = AllItems.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedItem != null)
                AllItems.Remove(matchedItem);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            ItemViewModel matchedItem = AllItems.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedItem != null)
            {
                matchedItem.Reload();
                AllItems.Move(AllItems.IndexOf(matchedItem), 0);
            }
            // If note isn't found, it's new; add it.
            else
                AllItems.Insert(0, new ItemViewModel(Models.Item.Load(noteId)));
        }
    }
}
