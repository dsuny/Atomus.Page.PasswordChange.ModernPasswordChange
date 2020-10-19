using Atomus.Page.PasswordChange.Controllers;
using Atomus.Security;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Atomus.Page.PasswordChange.ViewModel
{
    public class ModernPasswordChangeViewModel : MVVM.ViewModel
    {
        #region Declare
        private ICore core;

        private string email;
        private string oldAccessNumber;
        private string accessNumber;
        private string reAccessNumber;
        private bool activityIndicator;
        private bool isEnabledControl;
        #endregion

        #region Property
        public ICore Core
        {
            get
            {
                return this.core;
            }
            set
            {
                this.core = value;
            }
        }
        public string AppName
        {
            get
            {
                return Config.Client.GetAttribute("App.Name").ToString();
            }
            set
            {
                NotifyPropertyChanged();
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string OldAccessNumber
        {
            get
            {
                return this.oldAccessNumber;
            }
            set
            {
                if (this.oldAccessNumber != value)
                {
                    this.oldAccessNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string AccessNumber
        {
            get
            {
                return this.accessNumber;
            }
            set
            {
                if (this.accessNumber != value)
                {
                    this.accessNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string ReAccessNumber
        {
            get
            {
                return this.reAccessNumber;
            }
            set
            {
                if (this.reAccessNumber != value)
                {
                    this.reAccessNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool ActivityIndicator
        {
            get
            {
                return this.activityIndicator;
            }
            set
            {
                if (this.activityIndicator != value)
                {
                    this.activityIndicator = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool IsEnabledControl
        {
            get
            {
                return this.isEnabledControl;
            }
            set
            {
                if (this.isEnabledControl != value)
                {
                    this.isEnabledControl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand PasswordChangeCommand { get; set; }
        public ICommand BackCommand { get; set; }
        #endregion

        #region INIT
        public ModernPasswordChangeViewModel() { }
        public ModernPasswordChangeViewModel(ICore core) : this()
        {
            this.Core = core;

            this.email = "";
            this.oldAccessNumber = "";
            this.accessNumber = "";
            this.reAccessNumber = "";

            this.activityIndicator = false;
            this.isEnabledControl = true;

            this.PasswordChangeCommand = new Command(async () => await this.PasswordChangeProcess()
                                            , () => { return !this.ActivityIndicator; });

            this.BackCommand = new Command(async () => await this.BackProcess()
                                            , () => { return !this.ActivityIndicator; });
        }
        #endregion

        #region IO
        private async Task PasswordChangeProcess()
        {
            Service.IResponse result;
            ISecureHashAlgorithm secureHashAlgorithm;

            try
            {
                this.IsEnabledControl = false;
                this.ActivityIndicator = true;
                (this.PasswordChangeCommand as Command).ChangeCanExecute();

                if (this.Email.IsNullOrEmpty())
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "이메일을 입력해 주시기 바랍니다.", "OK");
                    return;
                }

                if (this.oldAccessNumber.IsNullOrEmpty())
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "기존 비밀번호를 입력해 주시기 바랍니다.", "OK");
                    return;
                }

                if (this.AccessNumber.IsNullOrEmpty())
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "비밀번호를 입력해 주시기 바랍니다.", "OK");
                    return;
                }

                if (this.AccessNumber != this.ReAccessNumber)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "비밀번호가 일치하지 않습니다.", "OK");
                    return;
                }

                secureHashAlgorithm = (ISecureHashAlgorithm)this.core.CreateInstance("SecureHashAlgorithm");

                result = await this.core.SavePasswordChangeAsync(this.Email, secureHashAlgorithm.ComputeHashToBase64String(this.oldAccessNumber), secureHashAlgorithm.ComputeHashToBase64String(this.ReAccessNumber));

                if (result.Status == Service.Status.OK)
                {
                    await Application.Current.MainPage.DisplayAlert("Information", "비밀번호 변경이 완료 되었습니다.", "OK");
                    await this.BackProcess();
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Warning", ex.Message, "OK");
            }
            finally
            {
                this.ActivityIndicator = false;
                this.IsEnabledControl = true;
                (this.PasswordChangeCommand as Command).ChangeCanExecute();
            }
        }

        internal async Task BackProcess()
        {
            this.IsEnabledControl = false;
            this.ActivityIndicator = true;
            (this.BackCommand as Command).ChangeCanExecute();

            await ((NavigationPage)Application.Current.MainPage).PopAsync();

            this.ActivityIndicator = false;
            this.IsEnabledControl = true;
            (this.BackCommand as Command).ChangeCanExecute();
        }
        #endregion

        #region ETC
        #endregion
    }
}