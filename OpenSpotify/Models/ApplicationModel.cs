using System.Collections.ObjectModel;

namespace OpenSpotify.Models {

    public class ApplicationModel : BaseModel {

        public ApplicationModel() {

            if (SongCollection == null) {
                SongCollection = new ObservableCollection<SongModel>();
            }
        }

        public ObservableCollection<SongModel> SongCollection { get; set; }
    }
}
