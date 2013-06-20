using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SpinningImage {
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate {
        UIWindow window;
        UIViewController viewController;
        UIButton switchButton;
        UIImageView animationImagesImageView;
        UIImageView basicAnimationImageView;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
            window = new UIWindow(UIScreen.MainScreen.Bounds);
            viewController = new UIViewController();
            viewController.View.Frame = new RectangleF(PointF.Empty, viewController.View.Bounds.Size);
            viewController.View.BackgroundColor = UIColor.White;

            PointF viewCenter = new PointF(viewController.View.Bounds.Width / 2f, viewController.View.Bounds.Height / 2f);
            animationImagesImageView = new UIImageView() {
                Frame = new RectangleF(PointF.Empty, new SizeF(40f, 40f)),
                Center = viewCenter,
                AnimationImages = new UIImage[] {
                    UIImage.FromBundle("loading_1"),
                    UIImage.FromBundle("loading_2"),
                    UIImage.FromBundle("loading_3"),
                    UIImage.FromBundle("loading_4"),
                },
            };
            viewController.View.Add(animationImagesImageView);
            animationImagesImageView.AnimationRepeatCount = 0;
            animationImagesImageView.AnimationDuration = 0.5;
            animationImagesImageView.StartAnimating();

            basicAnimationImageView = new UIImageView(UIImage.FromBundle("loading_1")) {
                Frame = new RectangleF(PointF.Empty, new SizeF(40f, 40f)),
                Center = viewCenter,
                Hidden = true,
            };
            CABasicAnimation rotationAnimation = CABasicAnimation.FromKeyPath("transform.rotation");
            rotationAnimation.To = NSNumber.FromDouble(Math.PI * 2); // full rotation (in radians)
            rotationAnimation.RepeatCount = int.MaxValue; // repeat forever
            rotationAnimation.Duration = 0.5;
            // Give the added animation a key for referencing it later (to remove, in this case).
            basicAnimationImageView.Layer.AddAnimation(rotationAnimation, "rotationAnimation");
            viewController.View.Add(basicAnimationImageView);

            // Button overlay to switch between the two versions.
            switchButton = new UIButton(new RectangleF(PointF.Empty, viewController.View.Bounds.Size));
            switchButton.TouchUpInside += (sender, e) => {
                animationImagesImageView.Hidden = !animationImagesImageView.Hidden;
                basicAnimationImageView.Hidden = !basicAnimationImageView.Hidden;
            };
            viewController.View.Add(switchButton);

            window.RootViewController = viewController;
            window.MakeKeyAndVisible();
            
            return true;
        }
    }
}