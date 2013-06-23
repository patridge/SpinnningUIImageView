using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace SpinningImage.Comparison {
    public static class AnimationHelpers {
        /// <summary>
        /// Builds up a basic rotation spin animation for use on various UI objects (e.g., loading spinner).
        /// </summary>
        /// <returns>
        /// A spin animation.
        /// </returns>
        /// <param name='rotationDegrees'>
        /// Amount of rotation to perform for each animation cycle. Defaults to 360 degrees.
        /// </param>
        /// <param name='repeatCount'>
        /// Number of types to repeat animation cycle. Defaults to infinite (int.MaxValue).
        /// </param>
        /// <param name='duration'>
        /// Duration of animation cycle in seconds. Defaults to 1s.
        /// </param>
        public static CABasicAnimation GetSpinAnimation(int rotationDegrees = 360, int repeatCount = int.MaxValue, double duration = 1) {
            CABasicAnimation rotationAnimation = CABasicAnimation.FromKeyPath("transform.rotation");
            rotationAnimation.To = NSNumber.FromDouble(rotationDegrees.FromDegreesToRadians());
            rotationAnimation.RepeatCount = repeatCount;
            rotationAnimation.Duration = duration;
            return rotationAnimation;
        }
        public static double FromDegreesToRadians(this int degrees) {
            return Math.PI * degrees / 180;
        }
    }

    public class SampleRowModel {
        public string ImageUrl;
        public string Description;
    }
    public class SampleTableCell : UITableViewCell {
        const string RotationAnimationKey = "RotationAnimation";
        public SampleTableCell(NSString key) : base(UITableViewCellStyle.Default, key) { }

        public static Task<UIImage> GetImageAsync(string url) {
            return Task.Factory.StartNew(() => {
                UIImage image;
                try {
                    using (WebClient webClient = new WebClient()) {
                        byte[] imageData = webClient.DownloadData(url);
                        image = UIImage.LoadFromData(NSData.FromArray(imageData));
                    }
                } catch {
                    // Swallow any exceptions and sending a null UIImage back.
                    image = null;
                }
                return image;
            });
        }
        public void UpdateCell(SampleRowModel model) {
            int tagForRequest = Tag;
            ImageView.Image = UIImage.FromBundle("loading_1");
            ImageView.Layer.AddAnimation(AnimationHelpers.GetSpinAnimation(), RotationAnimationKey);
            TextLabel.Text = model.Description;
            GetImageAsync(model.ImageUrl).ContinueWith(task => {
                InvokeOnMainThread(() => {
                    // Scrolling up and down will spin up multiple requests for the same recycled cell object.
                    // Make sure we only load the one appropriate for the currently displayed data.
                    // NOTE: This would be a good place to implement a limited-concurrency request queue.
                    // NOTE: This would be a good place to implement request cancellation, too.
                    int currentTag = Tag;
                    if (currentTag == tagForRequest) {
                        // Remove the spinning animation (hilarious if you don't).
                        ImageView.Layer.RemoveAnimation(RotationAnimationKey);
                        UIImage result = task.Result;
                        if (result != null) {
                            // Only showing an image if one came back successfully.
                            ImageView.Image = result;
                        }
                    }
                });
            });
        }
    }
    public class SampleListTableViewSource : UITableViewSource {
        IList<SampleRowModel> rows;
        static NSString cellKey = new NSString("SampleItem");
        public SampleListTableViewSource(IList<SampleRowModel> samples) {
            rows = samples;
        }
        public override int RowsInSection(UITableView tableview, int section) {
            return rows.Count;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
            SampleRowModel model = rows[indexPath.Row];
            SampleTableCell cell = (SampleTableCell)tableView.DequeueReusableCell(cellKey);
            if (cell == null) {
                cell = new SampleTableCell(cellKey);
            }
            cell.Tag = indexPath.Row;
            cell.UpdateCell(model);
            return cell;
        }
    }

    public class TableSampleTableViewController : UITableViewController {
        public TableSampleTableViewController() {
            Title = "Table Sample";
        }
        static List<string> ImageUrls = new List<string>() {
            "http://lorempixel.com/40/40/",
            "http://fillmurray.com/40/40",
            "http://baconmockup.com/40/40",
            "http://placekitten.com/40/40",
            "http://lorempixum.com/40/40/",
            "http://placecage.com/40/40",
            "http://placezombies.com/40x40",
            "http://flickholdr.com/40/40/",
            "http://dummyimage.com/40x40/000/f00",
            "http://placedog.com/40/40",
            "http://nicenicejpg.com/40/40",
            "http://placesheen.com/40/40",
            "http://placehold.it/40x40.jpg&text=sample",
            "http://placebear.com/40/40",
        };
        public override void ViewDidLoad() {
            base.ViewDidLoad();

            List<SampleRowModel> data = Enumerable.Range(0, 100).Select(i => {
                return new SampleRowModel() {
                    ImageUrl = ImageUrls[i % ImageUrls.Count],
                    Description = "Random Image " + i.ToString(),
                };
            }).ToList();
            TableView.Source = new SampleListTableViewSource(data);
            TableView.ReloadData();
        }
        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);

            NavigationController.SetNavigationBarHidden(hidden: false, animated: true);
        }
    }
}

