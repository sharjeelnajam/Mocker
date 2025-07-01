using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using MockerProject.Services;
using MockerProject.Views;
using ReactiveUI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Color = Avalonia.Media.Color;
using Image = Avalonia.Controls.Image;
using Path = System.IO.Path;
using DynamicData.Kernel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FontFamily = Avalonia.Media.FontFamily;
using Size = System.Drawing.Size;
using MockerProject.Models;


namespace MockerProject.ViewModels
{
    public  class HeaderViewModel : ViewModelBase
    {

        public HeaderViewModel()
        {

            onMenuOpen = ReactiveCommand.Create(() =>
            {
                var window = new RunWindow();

                window.Show();
                // Code here will be executed when the button is clicked.
                IsMenuOpened = true;
            });
        }
        public ICommand onMenuOpen { get; }

     

        public bool w_IsMenuOpenState = false;

        public bool IsMenuOpened
        {
            get => w_IsMenuOpenState;
            set => this.RaiseAndSetIfChanged(ref w_IsMenuOpenState, value);
        }
       

        
    }
}
