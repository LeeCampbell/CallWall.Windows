using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace CallWall.Windows.Shell.Controls
{
    public class ItemsStack : ItemsControl
    {
        static ItemsStack()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemsStack), new FrameworkPropertyMetadata(typeof(ItemsStack)));
        }


        #region MaxVisible DependencyProperty

        public int MaxVisible
        {
            get { return (int)GetValue(MaxVisibleProperty); }
            set { SetValue(MaxVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxVisibleProperty =
            DependencyProperty.Register("MaxVisible", typeof(int), typeof(ItemsStack), new PropertyMetadata(3));

        #endregion

        #region HorizontalItemOffset DependencyProperty

        public double HorizontalItemOffset
        {
            get { return (double)GetValue(HorizontalItemOffsetProperty); }
            set { SetValue(HorizontalItemOffsetProperty, value); }
        }

        public static readonly DependencyProperty HorizontalItemOffsetProperty =
            DependencyProperty.Register("HorizontalItemOffset", typeof(double), typeof(ItemsStack),
                                        new PropertyMetadata(10.0));
        #endregion

        #region VerticalItemOffset DependencyProperty

        public double VerticalItemOffset
        {
            get { return (double)GetValue(VerticalItemOffsetProperty); }
            set { SetValue(VerticalItemOffsetProperty, value); }
        }

        public static readonly DependencyProperty VerticalItemOffsetProperty =
            DependencyProperty.Register("VerticalItemOffset", typeof(double), typeof(ItemsStack),
                                        new PropertyMetadata(10.0));
        #endregion

        #region ItemDisplayDuration DependencyProperty

        public TimeSpan ItemDisplayDuration
        {
            get { return (TimeSpan)GetValue(ItemDisplayDurationProperty); }
            set { SetValue(ItemDisplayDurationProperty, value); }
        }

        public static readonly DependencyProperty ItemDisplayDurationProperty =
            DependencyProperty.Register("ItemDisplayDuration", typeof(TimeSpan), typeof(ItemsStack),
                                        new PropertyMetadata(TimeSpan.FromSeconds(3)));

        #endregion

        #region ItemTransitionDuration DependencyProperty

        public TimeSpan ItemTransitionDuration
        {
            get { return (TimeSpan)GetValue(ItemTransitionDurationProperty); }
            set { SetValue(ItemTransitionDurationProperty, value); }
        }

        public static readonly DependencyProperty ItemTransitionDurationProperty =
            DependencyProperty.Register("ItemTransitionDuration", typeof(TimeSpan), typeof(ItemsStack),
                                        new PropertyMetadata(TimeSpan.FromSeconds(1)));

        private DispatcherTimer _animationTimer;
        private readonly List<UIElement> _animatableItem = new List<UIElement>();

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_animationTimer != null)
            {
                _animationTimer.Stop();
            }
            _animationTimer = new DispatcherTimer(ItemDisplayDuration, DispatcherPriority.Background, Animate, this.Dispatcher);
            _animationTimer.Start();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var stdContainer = base.GetContainerForItemOverride();
            //Should I tack on the Canvas Setting here? Maybe apply the animation?
            return stdContainer;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var uiElement = element as UIElement;
            if (uiElement != null)
            {
                var idx = Items.IndexOf(item);
                Canvas.SetLeft(uiElement, HorizontalItemOffset * idx);
                Canvas.SetTop(uiElement, VerticalItemOffset * idx);
                Canvas.SetZIndex(uiElement, Items.Count - idx);

                _animatableItem.Add(uiElement);
            }
        }

        private Storyboard CreateAnimationStoryboard(UIElement uiElement)
        {
            Storyboard sb;
            var zIdx = Canvas.GetZIndex(uiElement);
            if (zIdx == Items.Count)
            {
                sb = CreateRemoveStoryboard(uiElement);
            }
            else
            {
                sb = CreatePromoteStoryboard(uiElement);
            }
            return sb;
        }

        private void Animate(object sender, EventArgs e)
        {
            Console.WriteLine("Animating");
            foreach (var item in _animatableItem)
            {
                var sb = CreateAnimationStoryboard(item);
                BeginStoryboard(sb);
            }
        }

        private Storyboard CreateRemoveStoryboard(UIElement uiElement)
        {
            var sb = new Storyboard();

            //If idx==0 animate down and out then zindex to 0or1
            //else move up by VertOffSet and left by HorOffset, then ZIndex++;
            var vertAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(vertAnimation, uiElement);
            Storyboard.SetTargetProperty(vertAnimation, new PropertyPath(Canvas.TopProperty));
            var newY = Items.Count * VerticalItemOffset;
            vertAnimation.KeyFrames.Add(new SplineDoubleKeyFrame(newY, ItemTransitionDuration));

            var horizAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(horizAnimation, uiElement);
            Storyboard.SetTargetProperty(horizAnimation, new PropertyPath(Canvas.LeftProperty));
            var newX = Items.Count * HorizontalItemOffset;
            horizAnimation.KeyFrames.Add(new SplineDoubleKeyFrame(newX, ItemTransitionDuration));

            var zindexAnimation = new Int32AnimationUsingKeyFrames();
            zindexAnimation.BeginTime = ItemTransitionDuration;
            Storyboard.SetTarget(zindexAnimation, uiElement);
            Storyboard.SetTargetProperty(zindexAnimation, new PropertyPath(Canvas.ZIndexProperty));
            zindexAnimation.KeyFrames.Add(new SplineInt32KeyFrame(0, TimeSpan.Zero));

            sb.Children.Add(vertAnimation);
            sb.Children.Add(horizAnimation);
            sb.Children.Add(zindexAnimation);
            return sb;
        }

        private Storyboard CreatePromoteStoryboard(UIElement uiElement)
        {
            var sb = new Storyboard();

            var vertAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(vertAnimation, uiElement);
            Storyboard.SetTargetProperty(vertAnimation, new PropertyPath(Canvas.TopProperty));
            var newY = Canvas.GetTop(uiElement) - VerticalItemOffset;
            vertAnimation.KeyFrames.Add(new SplineDoubleKeyFrame(newY, ItemTransitionDuration));

            var horizAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(horizAnimation, uiElement);
            Storyboard.SetTargetProperty(horizAnimation, new PropertyPath(Canvas.LeftProperty));
            var newX = Canvas.GetLeft(uiElement) - HorizontalItemOffset;
            horizAnimation.KeyFrames.Add(new SplineDoubleKeyFrame(newX, ItemTransitionDuration));

            var zindexAnimation = new Int32AnimationUsingKeyFrames();
            zindexAnimation.BeginTime = ItemTransitionDuration + TimeSpan.FromMilliseconds(1);
            Storyboard.SetTarget(zindexAnimation, uiElement);
            Storyboard.SetTargetProperty(zindexAnimation, new PropertyPath(Canvas.ZIndexProperty));
            var newZidx = Canvas.GetZIndex(uiElement) + 1;
            zindexAnimation.KeyFrames.Add(new SplineInt32KeyFrame(newZidx, TimeSpan.Zero));

            sb.Children.Add(vertAnimation);
            sb.Children.Add(horizAnimation);
            sb.Children.Add(zindexAnimation);
            return sb;
        }

        //Plan is to initially show a stack of pictures as we do currently.
        //They should be stacked with the top Item having a Top/Left Margin of 0. Right margin is MaxVisible * HorizontalItemOffset. Bottom Margin is MaxVisible * VerticalItemOffset.

    }
}
