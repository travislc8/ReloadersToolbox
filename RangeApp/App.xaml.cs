namespace RangeApp;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

public partial class App : Application
{
    public static Models.FirearmRepository FirearmRepo { get; private set; }
    public static Models.LocationRepository LocationRepo { get; private set; }
    public static Models.SessionRepository SessionRepo { get; private set; }
    public static Models.RoundRepository RoundRepo { get; private set; }

    const int WindowWidth = 500;
    const int WindowHeight = 700;
    public App(Models.FirearmRepository firearmRepoIn, Models.LocationRepository locationRepoIn, Models.SessionRepository sessionRepoIn, Models.RoundRepository roundRepoIn)
    {
        InitializeComponent();
        FirearmRepo = firearmRepoIn;
        LocationRepo = locationRepoIn;
        SessionRepo = sessionRepoIn;
        RoundRepo = roundRepoIn;

#if WINDOWS
        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
{

                var mauiWindow = handler.VirtualView;
                var nativeWindow = handler.PlatformView;
                nativeWindow.Activate();
                IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                appWindow.Resize(new Windows.Graphics.SizeInt32(WindowWidth, WindowHeight));
});
#endif
        MainPage = new AppShell();


    }
}
