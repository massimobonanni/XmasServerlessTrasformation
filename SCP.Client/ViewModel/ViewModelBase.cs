
namespace SCP.Client.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ViewModelBase()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private bool _IsBusy;

        public bool IsBusy
        {
            get => this._IsBusy;
            set => Set(ref _IsBusy, value);
        }

        private string _ErrorMessage;

        public string ErrorMessage
        {
            get => this._ErrorMessage;
            set
            {
                Set(ref _ErrorMessage, value);
                RaisePropertyChanged(nameof(HasError));
            }
        }

        public bool HasError { get => !string.IsNullOrWhiteSpace(ErrorMessage); }
    }
}