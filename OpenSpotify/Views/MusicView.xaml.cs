using System.Windows;
using System.Windows.Input;

namespace OpenSpotify.Views
{
    /// <summary>
    /// Interaktionslogik für MusicView.xaml
    /// </summary>
    public partial class MusicView {
        public MusicView()
        {
            InitializeComponent();
        }

        private void MusicPlayerView_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
