using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace TWKPrompter.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand SlowerCommand { get; private set; }
        public RelayCommand FasterCommand { get; private set; }
        public RelayCommand SmallerCommand { get; private set; }
        public RelayCommand LargerCommand { get; private set; }
        public RelayCommand MirrorCommand { get; private set; }
        public RelayCommand PlayPauseCommand { get; private set; }

        private double _scrollspeed = 20;
        public double ScrollSpeed
        {
            get { return _scrollspeed; }
            set
            {
                _scrollspeed = value;

                RaisePropertyChanged();
            }
        }

        private double _scale = 2;
        public double Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;

                RaisePropertyChanged();
                RaisePropertyChanged(() => RenderOffsetScale);
            }
        }

        private int _mirror = -1;
        public int Mirror
        {
            get { return _mirror; }
            set
            {
                _mirror = value;

                RaisePropertyChanged();
            }
        }

        private bool _playing = false;
        public bool Playing
        {
            get { return _playing; }
            set
            {
                _playing = value;

                RaisePropertyChanged();
            }
        }

        public System.Windows.Point RenderOffsetScale
        {
            get { return new System.Windows.Point(_scale / 2, _scale / 2); }

        }

        public MainViewModel()
        {
            SlowerCommand = new RelayCommand(Slower);
            FasterCommand = new RelayCommand(Faster);
            SmallerCommand = new RelayCommand(Smaller);
            LargerCommand = new RelayCommand(Larger);
            MirrorCommand = new RelayCommand(MirrorFlip);
            PlayPauseCommand = new RelayCommand(PlayPause);

        }

        public void MirrorFlip()
        {
            var x = (Mirror == -1) ? Mirror = 1 : Mirror = -1;
        }
        public void Smaller() { Scale -= 0.5; }
        public void Larger() { Scale += 0.5; }
        public void Slower() { ScrollSpeed -= 10; }
        public void Faster() { ScrollSpeed += 10; }

        public void PlayPause()
        {
            Playing = !Playing;

            MessengerInstance.Send(new PlayPauseMessage(Playing));
        }
    }
}