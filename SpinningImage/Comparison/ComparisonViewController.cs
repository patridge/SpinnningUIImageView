using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;

namespace SpinningImage.Comparison {
    public class ComparisonViewController : UIViewController {
        ComparisonView comparisonView;

        public ComparisonViewController() {
            Title = "Click to Toggle";
            comparisonView = new ComparisonView(View.Bounds);
            View = comparisonView;
        }
        public void Update(RectangleF bounds) {
            comparisonView.Frame = bounds;
        }
        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);

            NavigationController.SetNavigationBarHidden(hidden: false, animated: true);
        }
    }
}

