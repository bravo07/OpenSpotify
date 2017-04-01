using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Services.Util;
using static OpenSpotify.Services.Util.Utils;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
// ReSharper disable ExplicitCallerInfoArgument

namespace OpenSpotify.ViewModels {

    public class SettingsViewModel : BaseViewModel {

        public SettingsViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            Initialize();
        }

        #region Fields

        private ApplicationModel _applicationModel;
        private BitrateModel _selectedBitrate;
        private bool _startWithWindows;

        #endregion

        #region Properties

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value;
                OnPropertyChanged(nameof(ApplicationModel));
                Validate();
            }
        }

        public bool StartWithWindows {
            get { return _startWithWindows; }
            set {
                _startWithWindows = value;

                ApplicationModel.Settings.StartWithWindows = _startWithWindows;
                if (_startWithWindows) {
                    StartUpManager.AddApplicationToCurrentUserStartup();
                }
                else {
                    StartUpManager.RemoveApplicationFromCurrentUserStartup();
                }

                OnPropertyChanged(nameof(StartWithWindows));
            }
        }



        public ObservableCollection<BitrateModel> BitrateCollection { get; set; }

        public ObservableCollection<FormatModel> FormatCollection { get; set; }

        #endregion

        #region Commands

        public CommandHandler<object> FFmpegPathCommand {
            get {
                return new CommandHandler<object>(o => {
                    var openFileDialog = new OpenFileDialog {
                        Filter = "Executable Files |*.exe;",
                        InitialDirectory =
                            Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                    };

                    if (openFileDialog.ShowDialog() == true) {
                        ApplicationModel.Settings.FFmpegPath = openFileDialog.FileName;
                    }
                });
            }
        }

        public CommandHandler<object> MusicPathCommand {
            get {
                return new CommandHandler<object>(o => {
                    using (var folderBrowserDialog = new FolderBrowserDialog()) {
                        folderBrowserDialog.SelectedPath = MusicPath;
                        folderBrowserDialog.ShowNewFolderButton = true;

                        var result = folderBrowserDialog.ShowDialog();
                        if (result == DialogResult.OK) {
                            ApplicationModel.Settings.MusicPath = folderBrowserDialog.SelectedPath;
                        }
                    }
                });
            }
        }

        public CommandHandler<object> DeleteSettingsCommand {
            get {
                return new CommandHandler<object>(o => {
                    if (File.Exists(ApplicationDataPath)) {
                        File.Delete(ApplicationDataPath);
                        MessageBox.Show(@"Settings Deleted!", @"Information", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else {
                        MessageBox.Show(@"No Settings available!", @"Information", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                });
            }
        }

        #endregion

        #region Functions
        
        private void Initialize()
        {
            BitrateCollection = new ObservableCollection<BitrateModel> {
                new BitrateModel {
                    BitrateName = "320 kBit/s", Bitrate = "320K"
                },
                new BitrateModel {
                    BitrateName = "256 kBit/s", Bitrate = "256K"
                },
                new BitrateModel {
                    BitrateName = "192 kBit/s", Bitrate = "192K"
                },
                new BitrateModel {
                    BitrateName = "128 kBit/s", Bitrate = "128K"
                },
            };

            FormatCollection = new ObservableCollection<FormatModel> {
                new FormatModel {
                    FormatName = "Mp3",
                    Format = ".mp3"
                },
                new FormatModel {
                    FormatName = "Wav",
                    Format = ".wav"
                },
            };

            ApplicationModel.Settings.SelectedBitrate = BitrateCollection[0];
            ApplicationModel.Settings.SelectedFormat = FormatCollection[0];
        }

        private void Validate() {
            if (string.IsNullOrEmpty(ApplicationModel.Settings.YoutubeApiKey) ||
                string.IsNullOrEmpty(ApplicationModel.Settings.FFmpegPath)) {
                return;
            }

            ApplicationModel.StatusText = "Ready.";
            ApplicationService.SaveApplicationModel(ApplicationModel);
        }

        #endregion
    }
}