using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Drawing.CenterViewWPF;
    public class IconButton : Button
    {
        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton),
                new FrameworkPropertyMetadata(typeof(IconButton)));
        }
        
        public static readonly DependencyProperty PathDataProperty = 
            DependencyProperty.Register(nameof(PathData), typeof(Geometry),
                typeof(IconButton), new PropertyMetadata(Geometry.Empty));

        public Geometry PathData
        {
            get => (Geometry)GetValue(PathDataProperty);
            set => SetValue(PathDataProperty, value);
        }
        
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
            typeof(string),
            typeof(IconButton),
            new PropertyMetadata(string.Empty));
        
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation),
            typeof(Orientation),
            typeof(IconButton),
            new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        
        public static readonly DependencyProperty TextVisibilityProperty = DependencyProperty.Register(nameof(TextVisibility),
            typeof(Visibility),
            typeof(IconButton),
            new PropertyMetadata(default(Visibility)));

        public Visibility TextVisibility
        {
            get => (Visibility)GetValue(TextVisibilityProperty);
            set => SetValue(TextVisibilityProperty, value);
        }

        public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register(
            nameof(IconVisibility), typeof(Visibility), typeof(IconButton), new PropertyMetadata(default(Visibility)));

        public Visibility IconVisibility
        {
            get => (Visibility)GetValue(IconVisibilityProperty);
            set => SetValue(IconVisibilityProperty, value);
        }
        
    }