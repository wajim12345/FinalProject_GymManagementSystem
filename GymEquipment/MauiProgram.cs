using Microsoft.AspNetCore.Components.WebView.Maui;
using GymEquipment.Backend;
using GymEquipment.Backend.Entitites;
using GymEquipment.Data;

namespace GymEquipment;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});
		builder.Services.AddTransient<EquipmentManager>();
		builder.Services.AddTransient<Equipment>();
		builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

		return builder.Build();
	}
}
