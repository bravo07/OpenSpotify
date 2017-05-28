using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OpenSpotify.Views.Custom.Controls
{
    /// <summary>
    /// Interaction logic for CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl, INotifyPropertyChanged
    {
        public CustomButton() {
            InitializeComponent();
            DataContext = this;
        }

        public string Text {
            get {
                return (string)GetValue(TextProperty);
            }
            set {
                SetValue(TextProperty, value);
            }
        }

        public int TextSize {
            get {
                return (int)GetValue(FontSizeProperty);
            }
            set {
                SetValue(FontSizeProperty, value);
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CustomButton));
        public static readonly DependencyProperty TextSizeProperty = DependencyProperty.Register("TextSize", typeof(int), typeof(CustomButton));
        private Color _borderColor;

        public Color BorderColor {
            get { return _borderColor; }
            set {
                _borderColor = value;
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e) {
            BorderColor = (Color)ColorConverter.ConvertFromString("#FF4C4C4C");

        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e) {
            BorderColor = (Color)ColorConverter.ConvertFromString("#FF191919");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
