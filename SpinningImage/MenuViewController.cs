using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SpinningImage.Comparison;

namespace SpinningImage {
    public partial class MenuViewController : UIViewController {
        UIButton comparisonButton;
        UIButton tableSampleButton;

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            
            View.Frame = new RectangleF(PointF.Empty, View.Bounds.Size);
            View.BackgroundColor = UIColor.White;

            View.BackgroundColor = UIColor.White;
            comparisonButton = UIButton.FromType(UIButtonType.RoundedRect);
            comparisonButton.SetTitle("Compare Animations", UIControlState.Normal);
            comparisonButton.SizeToFit();
            comparisonButton.Center = new PointF(View.Center.X, comparisonButton.Center.Y + 6f);
            View.Add(comparisonButton);

            tableSampleButton = UIButton.FromType(UIButtonType.RoundedRect);
            tableSampleButton.SetTitle("Table Sample", UIControlState.Normal);
            tableSampleButton.SizeToFit();
            tableSampleButton.Center = new PointF(View.Center.X, comparisonButton.Frame.Bottom + tableSampleButton.Center.Y + 6f);
            View.Add(tableSampleButton);

            comparisonButton.TouchUpInside += (sender, e) => {
                NavigationController.PushViewController(new ComparisonViewController(), animated: true);
            };

            tableSampleButton.TouchUpInside += (sender, e) => {
                NavigationController.PushViewController(new TableSampleTableViewController(), animated: true);
            };
        }
        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);

            NavigationController.SetNavigationBarHidden(hidden: true, animated: false);
        }
    }
}

