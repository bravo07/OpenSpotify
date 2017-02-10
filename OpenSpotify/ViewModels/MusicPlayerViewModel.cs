using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;

// ReSharper disable ExplicitCallerInfoArgument

namespace OpenSpotify.ViewModels
{
    public class MusicPlayerViewModel : BaseViewModel {

        public MusicPlayerViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            Initialize();
        }

        #region Fields

        private ApplicationModel _applicationModel;
        private ImageSource _soundImage;
        private MediaElement _soundPlayerElement;
        private double _sliderTrackValue;
        private TimeSpan _totalTrackTime;
        private double _sliderTrackMaximum;

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
                return new CommandHandler<bool>(state => {
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

                });
            }
        }

        public CommandHandler<object> ValueChangedCommand {
            get {
                return new CommandHandler<object>(state => {

                });
            }
        }

        #endregion

        #region Functions 

        private void Initialize() {
            SoundPlayerElement = new MediaElement();
            SoundPlayerElement.MediaOpened += SoundPlayerElementOnMediaOpened;
        }

        private void SoundPlayerElementOnMediaOpened(object sender, RoutedEventArgs routedEventArgs) {
            TotalTrackTime = SoundPlayerElement.NaturalDuration.TimeSpan;

            SoundElementTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            SoundElementTimer.Tick += SoundElementTimerTick;
            SoundElementTimer.Start();
        }

        private void SoundElementTimerTick(object sender, EventArgs e) {

            if (!(SoundPlayerElement.NaturalDuration.TimeSpan.TotalSeconds > 0)) {
                return;
            }

            if (TotalTrackTime.TotalSeconds > 0) {
                SliderTrackValue = SoundPlayerElement.Position.TotalSeconds / TotalTrackTime.TotalSeconds;
            }
        }

        #endregion 
    }
}
