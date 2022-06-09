using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JardinApp.Views;

namespace Jardin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new JardinView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
