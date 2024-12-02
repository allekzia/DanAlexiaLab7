namespace DanAlexiaLab7;
using DanAlexiaLab7.Models;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}

    async void OnSaveButtonClicked(object sender, EventArgs e) 
	{ 
		var slist = (ShopList)BindingContext; 
		slist.Date = DateTime.UtcNow; 
		await App.Database.SaveShopListAsync(slist); 
		await Navigation.PopAsync(); 
	}

    async void OnDeleteButtonClicked(object sender, EventArgs e) 
	{ 
		var slist = (ShopList)BindingContext; 
		await App.Database.DeleteShopListAsync(slist); 
		await Navigation.PopAsync(); 
	}

    async void OnChooseButtonClicked(object sender, EventArgs e) 
	{ 
		await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext) 
		{ 
			BindingContext = new Product() 
		}); 
	}

    protected override async void OnAppearing() 
	{ 
		base.OnAppearing(); 
		
		var shopl = (ShopList)BindingContext; 
		
		listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID); 
	}

    async void OnDeleteItemClicked(object sender, EventArgs e)
    {
        var selectedItem = listView.SelectedItem as Product; 
        if (selectedItem != null)
        {
            bool confirmDelete = await DisplayAlert(
                "Confirm Delete",
                $"Are you sure you want to delete '{selectedItem.Description}'?",
                "Yes", "No");

            if (confirmDelete)
            {
                await App.Database.DeleteProductAsync(selectedItem);
                listView.ItemsSource = await App.Database.GetProductsAsync();
            }
        }
        else
        {
            await DisplayAlert("Error", "No item selected for deletion.", "OK");
        }
    }

}