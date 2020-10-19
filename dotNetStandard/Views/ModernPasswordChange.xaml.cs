using Atomus.Page.PasswordChange.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atomus.Page.PasswordChange
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModernPasswordChange : CarouselPage, ICore
    {
        #region INIT
        public ModernPasswordChange()
        {
            this.BindingContext = new ModernPasswordChangeViewModel(this);

            InitializeComponent();

            //this.BackgroundColor = ((string)Config.Client.GetAttribute("BackgroundColor")).ToColor();
        }
        #endregion

        #region EVENT
        protected override void OnAppearing() { }
        #endregion

        #region ETC
        #endregion
    }
}