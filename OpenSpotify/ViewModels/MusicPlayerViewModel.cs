using System;
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

        public MusicPlayerViewModel(ApplicationModel applicationModel, SongModel currentSong)
        {
            ApplicationModel = applicationModel;
            CurrentSong = currentSong;
            Initialize();
        }
        
        public MusicPlayerViewModel()
        {
         
        }

        #region Fields

        private ApplicationModel _applicationModel;
        private ImageSource _soundImage;
        private MediaElement _soundPlayerElement;
        private double _sliderTrackValue;
        private TimeSpan _totalTrackTime;
        private double _sliderTrackMaximum;
        private Visibility _soundSliderVisibility;
        private double _soundSliderValue;
        private SongModel _currentSong;
        private double _largeChange;
        private bool _isDragging;
        private double _smallChange;

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

        public MediaElement SoundPlayerElement {
            get { return _soundPlayerElement; }
            set {
                _soundPlayerElement = value;
                OnPropertyChanged(nameof(SoundPlayerElement));
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

        #endregion

        #region Commands


        public CommandHandler<bool> PlayPauseCommand {
            get {
                return new CommandHandler<bool>(state =>
                {
                    if (state)
                    {
                        SoundPlayerElement.Pause();
                    }
                    else
                    {
                        SoundPlayerElement.Play();
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

                    SoundSliderVisibility = state ? Visibility.Visible : Visibility.Collapsed;
                });
            }
        }

        public CommandHandler<object> ValueChangedCommand {
            get {
                return new CommandHandler<object>(state =>
                {

                });
            }
        }

        public CommandHandler<object> DragCompletedCommand { 
    
            get {
                return new CommandHandler<object>(state => {
                    IsDragging = false;
                    SoundPlayerElement.Position = TimeSpan.FromSeconds(SliderTrackValue);
                });
            }
        }

        public CommandHandler<object> SoundValueChangedCommand {
            get {
                return new CommandHandler<object>(state => {

                    SoundPlayerElement.Volume = SoundSliderValue;

                    if (SoundSliderValue == 0) {
                        SoundPlayerElement.Volume = 0;
                        SoundImage = SoundImageOff;
                        return;
                    }

                    if (SoundSliderValue <= 0.1) {
                        SoundPlayerElement.Volume = 0.1;
                        SoundImage = SoundImage10;
                        return;
                    }

                    if (SoundSliderValue <= 0.5) {
                        SoundPlayerElement.Volume = 0.5;
                        SoundImage = SoundImage50;
                        return;
                    }

                    if (SoundSliderValue > 0.5) {
                        SoundPlayerElement.Volume = 0.8;
                        SoundImage = SoundImage100;
                    }
                });
            }
        }

        #endregion

        #region Functions 

        private void Initialize() {
            SoundSliderVisibility = Visibility.Collapsed;
            SoundImage = SoundImage100;
            SoundSliderValue = 0.2;
            SoundPlayerElement = new MediaElement {
                LoadedBehavior = MediaState.Manual,
                Source = new Uri(CurrentSong.FullPath, UriKind.RelativeOrAbsolute)
            };
            SoundPlayerElement.Play();
            SoundPlayerElement.MediaOpened += SoundPlayerElementOnMediaOpened;
        }

        private void SoundPlayerElementOnMediaOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            TotalTrackTime = SoundPlayerElement.NaturalDuration.TimeSpan;
            SliderTrackMaximum = TotalTrackTime.Seconds;
            SmallChange = 1;
            LargeChange = Math.Min(10, TotalTrackTime.Seconds / 10);

            SoundElementTimer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            SoundElementTimer.Tick += SoundElementTimerTick;
            SoundElementTimer.Start();

            SoundPlayerElement.Play();
        }
        private void SoundElementTimerTick(object sender, EventArgs e) {

            if (!(SoundPlayerElement.NaturalDuration.TimeSpan.TotalSeconds > 0)) {
                return;
            }

            if (TotalTrackTime.TotalSeconds > 0) {
                SliderTrackValue = SoundPlayerElement.Position.TotalSeconds;
            }
        }
        #endregion 
    }
}
