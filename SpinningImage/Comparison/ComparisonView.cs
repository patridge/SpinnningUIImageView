using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;

namespace SpinningImage.Comparison {
    public class ComparisonView : UIView {
        UIButton switchButton;
        UIImageView animationImagesImageView;
        UIImageView basicAnimationImageView;
        CABasicAnimation rotationAnimation;

        public ComparisonView(RectangleF frame) : base(frame) {
            BackgroundColor = UIColor.White;

            animationImagesImageView = new UIImageView() {
                Frame = new RectangleF(PointF.Empty, new SizeF(40f, 40f)),
                AutoresizingMask = UIViewAutoresizing.FlexibleMargins,
                AnimationImages = new UIImage[] {
                    UIImage.FromBundle("loading_1"),
                    UIImage.FromBundle("loading_2"),
                    UIImage.FromBundle("loading_3"),
                    UIImage.FromBundle("loading_4"),
                },
            };
            animationImagesImageView.AnimationRepeatCount = 0;
            animationImagesImageView.AnimationDuration = 0.5;
            animationImagesImageView.StartAnimating();
            Add(animationImagesImageView);

            basicAnimationImageView = new UIImageView(UIImage.FromBundle("loading_1")) {
                Frame = new RectangleF(PointF.Empty, new SizeF(40f, 40f)),
                AutoresizingMask = UIViewAutoresizing.FlexibleMargins,
                Hidden = true,
            };
            rotationAnimation = CABasicAnimation.FromKeyPath("transform.rotation");
            rotationAnimation.To = NSNumber.FromDouble(Math.PI * 2); // full rotation (in radians)
            rotationAnimation.RepeatCount = int.MaxValue; // repeat forever
            rotationAnimation.Duration = 0.5;
            // Give the added animation a key for referencing it later (e.g., to remove it when it is replaced).
            basicAnimationImageView.Layer.AddAnimation(rotationAnimation, "rotationAnimation");
            Add(basicAnimationImageView);

            // Button overlay to switch between the two versions.
            switchButton = new UIButton();
            switchButton.TouchUpInside += (sender, e) => {
                animationImagesImageView.Hidden = !animationImagesImageView.Hidden;
                basicAnimationImageView.Hidden = !basicAnimationImageView.Hidden;
            };
            Add(switchButton);
        }
        public override void LayoutSubviews() {
            base.LayoutSubviews();

            PointF viewCenter = new PointF(Bounds.Width / 2f, Bounds.Height / 2f);
            animationImagesImageView.Center = viewCenter;
            basicAnimationImageView.Center = viewCenter;
            switchButton.Frame = new RectangleF(PointF.Empty, Bounds.Size);
        }
    }    
}
