using System.Collections.ObjectModel;

namespace TOTools.Seeding;

/// <summary>
/// A page that allows the user to select a list of competitors to be used in bracket generation.
/// </summary>
public partial class SelectCompetitorsPage : ContentPage
{

	private SeedingBusinessLogic? _seedingBusinessLogic;
	
	public SelectCompetitorsPage()
	{
		InitializeComponent();
		HandlerChanged += OnHandlerChanged;
	}
	
	private async void OnHandlerChanged(object? sender, EventArgs e)
	{
		_seedingBusinessLogic ??= Handler?.MauiContext?.Services
			.GetService<SeedingBusinessLogic>();
		if (_seedingBusinessLogic == null)
		{
			return;
		}
		await _seedingBusinessLogic.LoadTask;
		BindingContext = _seedingBusinessLogic;
	}

	private void OnSubmitClicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new BracketsPage());
	}
}
