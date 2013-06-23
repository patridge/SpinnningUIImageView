using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SpinningImage {
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate {
        UIWindow window;
        UINavigationController navigationController;
        UIViewController menuViewController;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
            window = new UIWindow(UIScreen.MainScreen.Bounds);
            menuViewController = new MenuViewController();

            navigationController = new UINavigationController(menuViewController);
            window.RootViewController = navigationController;
            window.MakeKeyAndVisible();
            return true;
        }
    }
    public class Application {
        static void Main(string[] args) {
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}