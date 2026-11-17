using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace PreciousPractice;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
.ConfigureFonts(fonts =>
{
    fonts.AddFont(
        "MartelSans-ExtraLight.ttf",
        "MartelSansExtraLight");

    fonts.AddFont(
        "MartelSans-Light.ttf",
        "MartelSansLight");

    fonts.AddFont(
        "MartelSans-Regular.ttf",
        "MartelSansRegular");

    fonts.AddFont(
        "MartelSans-SemiBold.ttf",
        "MartelSansSemiBold");

    fonts.AddFont(
        "MartelSans-Bold.ttf",
        "MartelSansBold");

    fonts.AddFont(
        "MartelSans-ExtraBold.ttf",
        "MartelSansExtraBold");

    fonts.AddFont(
        "MartelSans-Black.ttf",
        "MartelSansBlack");
});
#if DEBUG
        builder.Logging.AddDebug(); 
#endif

#if ANDROID
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
            "NoUnderline",
            (handler, view) =>
            {
                handler.PlatformView.Background = null;
            });

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
            "DisableAutofill",
            (handler, view) =>
            {
                handler.PlatformView.ImportantForAutofill =
                    global::Android.Views.ImportantForAutofill.NoExcludeDescendants;
            });
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
            "CenterEntryText",
            (handler, view) =>
            {
                handler.PlatformView.SetPadding(0, 0, 0, 0);
                handler.PlatformView.Gravity = global::Android.Views.GravityFlags.CenterVertical;
            });

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
            "InstantPasswordMask",
           (handler, view) =>
            {
               if (view.IsPassword)
               {
                    handler.PlatformView.TransformationMethod =
                       new PreciousPractice.Platforms.Android.InstantPasswordTransformationMethod();
                }
           });
#endif

        return builder.Build();
    }
}