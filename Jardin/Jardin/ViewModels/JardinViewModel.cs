using Jardin;
using JardinApp.Models;
using JardinApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace JardinApp.ViewModels
{
    public class JardinViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Planta> Jardin { get; set; } = new ObservableCollection<Planta>();
        
        public Planta Planta { get; set; }
        public string Error { get; set; }

        public ICommand VistaCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand VerDetallesCommand { get; set; }
        public ICommand GuardarCommand { get; set; }

        public JardinViewModel()
        {
            Deserializar();
            VistaCommand = new Command<string>(Vista);
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command<Planta>(Editar);
            EliminarCommand = new Command<Planta>(Eliminar);
            VerDetallesCommand = new Command<Planta>(VerDetalles);
            GuardarCommand = new Command(Guardar);
        }

        AgregarPlanta verPlanta;
        EditarPlantaView verEditar;
        DetallesPlanta verDetalles;

        private void Guardar(object obj)
        {
            if (Planta != null)
            {
                Error = "";

                if (string.IsNullOrWhiteSpace(Planta.Nombre))
                {
                    Error = "Escriba el nombre de la planta";
                }
                if (string.IsNullOrWhiteSpace(Planta.Riego))
                {
                    Error = "Escriba el modo de riego de la planta";
                }
                if (string.IsNullOrWhiteSpace(Planta.Luz))
                {
                    Error = "Escriba cuanta luz necesita la planta";
                }
                if (string.IsNullOrWhiteSpace(Planta.Ubicacion))
                {
                    Error = "Escriba la ubicación de la planta";
                }
                if (string.IsNullOrWhiteSpace(Planta.Imagen))
                {
                    Error = "Escriba la URL de la imagen";
                }
                if (!Uri.TryCreate(Planta.Imagen, UriKind.Absolute, out var uri))
                {
                    Error = "Escriba una URL valida para la imagen";
                }
                if (string.IsNullOrWhiteSpace(Error))
                {
                    Jardin[indice] = Planta; //remplaza por el clon
                    Serializar();
                    App.Current.MainPage.Navigation.PopToRootAsync();
                }
                PropertyChange();
            }   
        }

        private void VerDetalles(Planta p)
        {
            Planta = new Planta
            {

                Nombre = p.Nombre,
                Riego = p.Riego,
                Ubicacion = p.Ubicacion,
                Luz = p.Luz,
                Comentario = p.Comentario,
                Imagen = p.Imagen
            };
            verDetalles = new DetallesPlanta()
            {
                BindingContext = this
            };
            this.Planta = p;
            App.Current.MainPage.Navigation.PushAsync(verDetalles);
        }

        private void Eliminar(Planta p)
        {
            if(p!=null)
            {
                Jardin.Remove(p);
                Serializar();
            }
        }

        int indice;
        private void Editar(Planta p)
        {
            Error="";
            indice = Jardin.IndexOf(p);
            Planta = new Planta
            {
                
                Nombre = p.Nombre,
                Riego = p.Riego,
                Ubicacion = p.Ubicacion,
                Luz = p.Luz,
                Comentario = p.Comentario,
                Imagen = p.Imagen
            };

            verEditar = new EditarPlantaView()
            {
                BindingContext = this
            };

            App.Current.MainPage.Navigation.PushAsync(verEditar);
        }

        private void Agregar()
        {
            if(Planta != null)
            {
                Error = "";

                if(string.IsNullOrWhiteSpace(Planta.Nombre))
                {
                    Error = "Escriba el nombre de la planta";
                }
                if(string.IsNullOrWhiteSpace(Planta.Riego))
                {
                    Error = "Escriba el modo de riego de la planta";
                }
                if(string.IsNullOrWhiteSpace(Planta.Luz))
                {
                    Error = "Escriba cuanta luz necesita la planta";
                }
                if(string.IsNullOrWhiteSpace(Planta.Ubicacion))
                {
                    Error = "Escriba la ubicación de la planta";
                }
                if (string.IsNullOrWhiteSpace(Planta.Imagen))
                {
                    Error = "Escriba la URL de la imagen";
                }
                if(!Uri.TryCreate(Planta.Imagen, UriKind.Absolute, out var uri))
                {
                    Error = "Escriba una URL valida para la imagen";
                }
                if (string.IsNullOrWhiteSpace(Error))
                {
                    Jardin.Add(Planta);
                    Vista("Ver");
                    Serializar();
                }
                PropertyChange();
            }
        }

        private void Vista(string vista)
        {
            if(vista =="Agregar")
            {
                Error = "";
                Planta = new Planta();
                verPlanta = new AgregarPlanta()
                {
                    BindingContext = this
                };
                App.Current.MainPage.Navigation.PushAsync(verPlanta);
            }
            else if(vista =="Ver")
            {
                App.Current.MainPage.Navigation.PopToRootAsync();
            }
        }

        public void Serializar()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "jardin.json";
            File.WriteAllText(file, JsonConvert.SerializeObject(Jardin));
        }

        public void Deserializar()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "jardin.json";
            if(File.Exists(file))
            {
                Jardin = JsonConvert.DeserializeObject<ObservableCollection<Planta>>(File.ReadAllText(file));
            }
        }

        private void PropertyChange()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
