using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;
using static OpenSpotify.Services.Util.Utils;
// ReSharper disable ExplicitCallerInfoArgument

namespace OpenSpotify.ViewModels
{

    public class MusicPlayerViewModel : BaseViewModel
    {

        public MusicPlayerViewModel(ApplicationModel applicationModel, SongModel currentSong) {
            ApplicationModel = applicationModel;
            CurrentSong = currentSong;
            Initialize();
        }

        public MusicPlayerViewModel() {
        }

        #region Fields

        private ApplicationModel _applicationModel;
        private ImageSource _soundImage;
        private MediaElement _mediaElementPlayer;
        private double _sliderTrackValue;
        private TimeSpan _totalTrackTime;
        private double _sliderTrackMaximum;
        private Visibility _soundSliderVisibility;
        private double _soundSliderValue;
        private SongModel _currentSong;
        private double _largeChange;
        private bool _isDragging;
        private double _smallChange;
        private string _currentSongName;
        private string _currentSongTime;

        #endregion

        #region Properties

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value;
                OnPropertyChanged(nameof(ApplicationModel));
            }
        }

        public SongModel CurrentSong {
            get { return _currentSong; }
            set {
                _currentSong = value;
                OnPropertyChanged(nameof(CurrentSong));
            }
        }

        public ImageSource SoundImage {
            get { return _soundImage; }
            set {
                _soundImage = value;
                OnPropertyChanged(nameof(SoundImage));
            }
        }

        public MediaElement MediaElementPlayer {
            get { return _mediaElementPlayer; }
            set {
                _mediaElementPlayer = value;
                OnPropertyChanged(nameof(MediaElementPlayer));
            }
        }

        public double SliderTrackValue {
            get { return _sliderTrackValue; }
            set {
                _sliderTrackValue = value;
                OnPropertyChanged(nameof(SliderTrackValue));
            }
        }

        public double SliderTrackMaximum {
            get { return _sliderTrackMaximum; }
            set {
                _sliderTrackMaximum = value;
                OnPropertyChanged(nameof(SliderTrackMaximum));
            }
        }

        public Visibility SoundSliderVisibility {
            get { return _soundSliderVisibility; }
            set {
                _soundSliderVisibility = value;
                OnPropertyChanged(nameof(SoundSliderVisibility));
            }
        }

        public double SoundSliderValue {
            get { return _soundSliderValue; }
            set {
                _soundSliderValue = value;
                OnPropertyChanged(nameof(SoundSliderValue));
            }
        }

        public TimeSpan TotalTrackTime {
            get { return _totalTrackTime; }
            set {
                _totalTrackTime = value;
                OnPropertyChanged(nameof(TotalTrackTime));
            }
        }

        public DispatcherTimer SoundElementTimer { get; set; }

        public double SmallChange {
            get { return _smallChange; }
            set {
                _smallChange = value;
                OnPropertyChanged(nameof(SmallChange));
            }
        }

        public double LargeChange {
            get { return _largeChange; }
            set {
                _largeChange = value;
                OnPropertyChanged(nameof(LargeChange));
            }
        }

        public bool IsDragging {
            get { return _isDragging; }
            set {
                _isDragging = value;
                OnPropertyChanged(nameof(IsDragging));
            }
        }

        public int IndexOfLastSong { get; set; }

        public string CurrentSongName {
            get { return _currentSongName; }
            set {
                _currentSongName = value;
                OnPropertyChanged(nameof(CurrentSongName));
            }
        }

        public string CurrentSongTime {
            get { return _currentSongTime; }
            set {
                _currentSongTime = value;
                OnPropertyChanged(nameof(CurrentSongTime));
            }
        }

        #endregion

        #region Commands

        public CommandHandler<bool> PlayPauseCommand {
            get {
                return new CommandHandler<bool>(state => {
                    if (state) {
                        MediaElementPlayer.Pause();
                    }
                    else {
                        MediaElementPlayer.Play();
                    }
                });
            }
        }

        public CommandHandler<bool> SoundCommand {
            get {
                return new CommandHandler<bool>(state => {

                    if (!state && SoundSliderValue == 0) {
                        SoundImage = SoundImageOff;
                    }
                    else {
                        SoundImage = SoundImage100;
                    }

                    if (!state && SoundSliderVisibility == Visibility.Collapsed) {
                        SoundSliderVisibility = Visibility.Visible;
                    }
                    else {
                        SoundSliderVisibility = state ? Visibility.Visible : Visibility.Collapsed;
                    }
                });
            }
        }

        public CommandHandler<object> ValueChangedCommand {
            get {
                return
                    new CommandHandler<object>(
                        state => { MediaElementPlayer.Position = TimeSpan.FromSeconds(SliderTrackValue); });
            }
        }

        public CommandHandler<object> GotFocusCommand {
            get {
                return new CommandHandler<object>(state => {
                    SoundSliderVisibility = Visibility.Collapsed;
                });
            }
        }

        public CommandHandler<object> DragCompletedCommand {
            get {
                return new CommandHandler<object>(state => {
                    IsDragging = false;
                    MediaElementPlayer.Position = TimeSpan.FromSeconds(SliderTrackValue);
                });
            }
        }

        public CommandHandler<object> SoundValueChangedCommand {
            get {
                return new CommandHandler<object>(state => {
                    MediaElementPlayer.Volume = SoundSliderValue;

                    if (SoundSliderValue == 0) {
                        MediaElementPlayer.Volume = 0;
                        SoundImage = SoundImageOff;
                        return;
                    }

                    if (SoundSliderValue <= 0.1) {
                        MediaElementPlayer.Volume = 0.1;
                        SoundImage = SoundImage10;
                        return;
                    }

                    if (SoundSliderValue <= 0.5) {
                        MediaElementPlayer.Volume = 0.5;
                        SoundImage = SoundImage50;
                        return;
                    }

                    if (SoundSliderValue > 0.5) {
                        MediaElementPlayer.Volume = 0.8;
                        SoundImage = SoundImage100;
                    }
                });
            }
        }

        public CommandHandler<object> PlayerBackCommand {
            get {
                return new CommandHandler<object>(state => {
                    Reset();
                    LoadLastSong();
                });
            }
        }

        public CommandHandler<object> PlayerNextCommand {
            get {
                return new CommandHandler<object>(state => {
                    Reset();
                    LoadNextSong();
                });
            }
        }

        public CommandHandler<UserControl> ClosePlayerCommand {
            get {
                return new CommandHandler<UserControl>(userControl => {
                    var parentWin = Window.GetWindow(userControl);
                    MediaElementPlayer.Stop();
                    parentWin?.Hide();
                });
            }
        }

        public CommandHandler<object> SoundSliderLostFocusCommand {
            get {
                return new CommandHandler<object>(o => {
                    SoundSliderVisibility = Visibility.Collapsed;
                });
            }
        }

        public CommandHandler<object> SoundSliderMouseLeaveCommand {
            get {
                return new CommandHandler<object>(o => {
                    SoundSliderVisibility = Visibility.Collapsed;
                });
            }
        }
        #endregion

        #region Functions 

        private void Initialize() {
            SoundSliderVisibility = Visibility.Collapsed;
            SoundImage = SoundImage100;
            SoundSliderValue = 0.2;
            InitializeMediaElement(CurrentSong);
        }

        private void InitializeMediaElement(SongModel song) {
            if (ApplicationModel.SongCollection.Count > 0) {
                IndexOfLastSong = ApplicationModel.SongCollection.IndexOf(song);
            }

            if (!File.Exists(song.FullPath)) {
                return;
            }

            MediaElementPlayer = new MediaElement {
                LoadedBehavior = MediaState.Manual,
                Source = new Uri(song.FullPath, UriKind.RelativeOrAbsolute)
            };

            SoundElementTimer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(200)
            };

            GetCurrentSongName(song);
            MediaElementPlayer.Play();
            MediaElementPlayer.MediaOpened += SoundPlayerElementOnMediaOpened;
            MediaElementPlayer.MediaEnded += SoundPlayerElementMediaEnded;

            SoundElementTimer.Tick += SoundElementTimerTick;
            SoundElementTimer.Start();
        }

        private void SoundPlayerElementMediaEnded(object sender, RoutedEventArgs e) {
            if (ApplicationModel.Settings.AutoPlay) {
                LoadNextSong();
            }
        }

        private void SoundPlayerElementOnMediaOpened(object sender, RoutedEventArgs routedEventArgs) {
            if (MediaElementPlayer.NaturalDuration.HasTimeSpan) {
                var naturalDurationTimeSpan = MediaElementPlayer.NaturalDuration.TimeSpan;
                SliderTrackMaximum = naturalDurationTimeSpan.TotalSeconds;
                SmallChange = 1;
                LargeChange = Math.Min(10, naturalDurationTimeSpan.Seconds / 10);
            }
            SoundElementTimer.Start();
            MediaElementPlayer.Play();
        }

        private void SoundElementTimerTick(object sender, EventArgs e) {
            SliderTrackValue = MediaElementPlayer.Position.TotalSeconds;
            if (MediaElementPlayer.NaturalDuration.HasTimeSpan) {
                CurrentSongTime = $@"{TimeSpan.FromMinutes(MediaElementPlayer.Position.Minutes):mm}:{TimeSpan.FromSeconds(
                                  MediaElementPlayer.Position.Seconds):ss} / {MediaElementPlayer.NaturalDuration.TimeSpan:mm\:ss}";
            }
        }

        private void Reset() {
            MediaElementPlayer.Stop();

            MediaElementPlayer.Close();
            SliderTrackValue = 0;
        }

        private void LoadNextSong() {
            if (ApplicationModel.SongCollection.Count == 0) {
                return;
            }

            if (IndexOfLastSong + 1 >= ApplicationModel.SongCollection.Count) {
                IndexOfLastSong = 0;
            }

            var nextSong = ApplicationModel.SongCollection.Count <= 1 ?
                ApplicationModel.SongCollection[IndexOfLastSong] :
                ApplicationModel.SongCollection[IndexOfLastSong + 1];

            if (nextSong != null) {
                InitializeMediaElement(nextSong);
            }
        }

        private void LoadLastSong() {
            if (ApplicationModel.SongCollection.Count < 1) {
                return;
            }

            if (IndexOfLastSong > ApplicationModel.SongCollection.Count) {
                IndexOfLastSong = 0;
            }

            if (IndexOfLastSong == 0) {
                IndexOfLastSong = 1;
            }

            var nextSong = ApplicationModel.SongCollection[IndexOfLastSong - 1];
            if (nextSong != null) {
                if (!File.Exists(nextSong.FullPath)) {
                    var song = ApplicationModel.SongCollection[IndexOfLastSong + 1];
                    InitializeMediaElement(song);
                    return;
                }
                InitializeMediaElement(nextSong);
            }
        }

        private void GetCurrentSongName(SongModel currentSong) {
            CurrentSongName = $"{currentSong.ArtistName} / {currentSong.SongName}";
        }

        #endregion
    }
}