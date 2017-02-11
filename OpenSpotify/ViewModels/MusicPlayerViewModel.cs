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

        public MusicPlayerViewModel(ApplicationModel applicationModel)
        {
            ApplicationModel = applicationModel;
            Initialize();
        }


        public MusicPlayerViewModel()
        {
            Initialize();
        }

        #region Fields

        private ApplicationModel _applicationModel;
        private ImageSource _soundImage;
        private MediaElement _soundPlayerElement;
        private double _sliderTrackValue;
        private TimeSpan _totalTrackTime;
        private double _sliderTrackMaximum;
        private Visibility _soundSliderVisibility;
        private bool _soundSliderValue;

        #endregion

        #region Properties

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value;
                OnPropertyChanged(nameof(ApplicationModel));
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

        public TimeSpan TotalTrackTime {
            get { return _totalTrackTime; }
            set {
                _totalTrackTime = value;
                OnPropertyChanged(nameof(TotalTrackTime));
            }
        }

        public DispatcherTimer SoundElementTimer { get; set; }
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
                    SoundSliderVisibility = state ? Visibility.Visible : Visibility.Collapsed;
                    SoundImage = SliderTrackValue > 50 ? SoundImage100 : SoundImage50;
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

        public bool SoundSliderValue {
            get { return _soundSliderValue; }
            set {
                _soundSliderValue = value;
                OnPropertyChanged(nameof(SoundSliderValue));
            }
        }

        #endregion

        #region Functions 

        private void Initialize()
        {

            SoundSliderVisibility = Visibility.Collapsed;
            SoundImage = SoundImage100;
            SoundPlayerElement = new MediaElement();
            SoundPlayerElement.MediaOpened += SoundPlayerElementOnMediaOpened;
            SoundPlayerElement.LoadedBehavior = MediaState.Manual;
        }

        private void SoundPlayerElementOnMediaOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            TotalTrackTime = SoundPlayerElement.NaturalDuration.TimeSpan;

            SoundElementTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            SoundElementTimer.Tick += SoundElementTimerTick;
            SoundElementTimer.Start();
        }

        private void SoundElementTimerTick(object sender, EventArgs e)
        {

            if (!(SoundPlayerElement.NaturalDuration.TimeSpan.TotalSeconds > 0))
            {
                return;
            }

            if (TotalTrackTime.TotalSeconds > 0)
            {
                SliderTrackValue = SoundPlayerElement.Position.TotalSeconds / TotalTrackTime.TotalSeconds;
            }
        }

        #endregion 
    }
}
