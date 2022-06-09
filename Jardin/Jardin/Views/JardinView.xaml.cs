using JardinApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JardinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JardinView : ContentPage
    {
        public JardinView()
        {
            InitializeComponent();
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPlanta());
        }

        private async void SwipeItem_Clicked(object sender, EventArgs e)
        {
            var swipe = (SwipeItem)sender;
            if (await DisplayAlert("Confirme", $"¿Desea eliminar {((Planta)swipe.CommandParameter).Nombre}?","Sí","No")== true)
                {
                   pvm.EliminarCommand.Execute(swipe.CommandParameter);
                }

        }
    }
}