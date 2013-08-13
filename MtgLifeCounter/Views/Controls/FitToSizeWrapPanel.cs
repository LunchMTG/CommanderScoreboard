using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.UI.Xaml.Media;

namespace MtgLifeCounter.Views.Controls
{
    class FitToSizeWrapPanel : Panel
    {
        private int _lastChildrenCount = 0;

        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows", typeof(int), typeof(FitToSizeWrapPanel), new PropertyMetadata(1));
        public int Rows
        {
            get { return (int)this.GetValue(RowsProperty); }
            set { this.SetValue(RowsProperty, value); }
        }


        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(int), typeof(FitToSizeWrapPanel), new PropertyMetadata(1));
        public int Columns
        {
            get { return (int)this.GetValue(ColumnsProperty); }
            set { this.SetValue(ColumnsProperty, value); }
        }


        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(FitToSizeWrapPanel), new PropertyMetadata(Orientation.Vertical));

        public Orientation Orientation
        {
            get { return (Orientation)this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // We want it all!
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_lastChildrenCount != Children.Count)
                UpdateRowsAndColumns();

            int r = 0, c = 0, rows = Rows, cols = Columns;
            Size childSize = ComputeSize(rows, cols);
            double sideMargins = (finalSize.Width - (childSize.Width * cols) ) / 2.0;
            double topMargins = (finalSize.Height - (childSize.Height * rows)) / 2.0;

            foreach (UIElement child in Children.Where(a=>IsVisible(a)))
            {
                Point pos = new Point(c * childSize.Width + sideMargins, r * childSize.Height + topMargins);
                child.Arrange(new Rect(pos, childSize));
                if (Orientation == Orientation.Horizontal)
                {
                    if (c < cols-1)
                    {
                        c++;
                    }
                    else
                    {
                        r++;
                        c = 0;
                    }
                }
                else
                {
                    if (r < rows-1)
                        r++;
                    else
                    {
                        c++; r = 0;
                    }
                }
            }

            return base.ArrangeOverride(finalSize);
        }

        private void UpdateRowsAndColumns()
        {
            int rows = 1, cols = 1, x = 0;

            // TODO refactor this so we dont repeat code
            if (Orientation == Orientation.Horizontal)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    if (!IsVisible(Children[i]))
                        continue;

                    if (x < cols)
                        x++;
                    else
                    {
                        double rSize = ComputeArea(rows + 1, cols);
                        double cSize = ComputeArea(rows, cols + 1);
                        double rSize2 = cols <= rows ? 0.0 : ComputeArea(rows + 1, cols - 1);

                        if (cSize >= rSize && cSize >= rSize2)
                        {
                            cols++;
                            x++;
                        }
                        else if (rSize2 >= rSize)
                        {
                            //cols--;
                            rows++;
                            x = 0;
                        }
                        else
                        {
                            rows++;
                            x = 0;
                        }
                    }
                }

                while (rows * cols < Children.Count)
                    cols++;
            }
            else
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    if (!IsVisible(Children[i]))
                        continue;

                    if (x < rows)
                        x++;
                    else
                    {
                        double rSize = ComputeArea(rows + 1, cols );
                        double cSize = ComputeArea(rows, cols + 1);
                        double cSize2 = rows <= cols ? 0.0 : ComputeArea(rows - 1, cols + 1);

                        if (rSize >= cSize && rSize >= cSize2)
                        {
                            rows++;
                            x++;
                        }
                        else if (cSize2 >= cSize)
                        {
                            //rows--;
                            cols++;
                            x = 0;
                        }
                        else 
                        {
                            cols++;
                            x = 0;
                        }
                    }
                }

                while (rows * cols < Children.Count)
                    rows++;
            }

            Rows = rows;
            Columns = cols;
            _lastChildrenCount = Children.Count;
        }

        private double ComputeArea(int rows, int cols)
        {
            Size size = ComputeSize(rows, cols);
            return size.Width * size.Height;
        }

        private Size ComputeSize(int rows, int cols)
        {
            Size desiredSize = DesiredSize;
            double width = desiredSize.Width / (double)cols;
            double height = desiredSize.Height / (double)rows;
            Size size = new Size(width, height);
            return CalculateDesiredSize(size);
        }

        private Size CalculateDesiredSize(Size size)
        {
            Size desiredSize = new Size();
            foreach (var child in Children)
            {
                child.Measure(size);
                if (child.DesiredSize.Width > desiredSize.Width)
                    desiredSize.Width = child.DesiredSize.Width;
                if (child.DesiredSize.Height > desiredSize.Height)
                    desiredSize.Height = child.DesiredSize.Height;
            }

            return desiredSize;
        }

        private bool IsVisible(UIElement uiElement)
        {
            if (uiElement is ContentPresenter)
            {
                int childCount = VisualTreeHelper.GetChildrenCount(uiElement);
                if (childCount > 0)
                {
                    uiElement = VisualTreeHelper.GetChild(uiElement, 0) as UIElement;
                }
            }

            return uiElement.Visibility == Windows.UI.Xaml.Visibility.Visible;
        }
    }
}
