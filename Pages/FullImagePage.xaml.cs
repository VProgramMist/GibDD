using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GibDD.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullImagePage : ContentPage
    {
        public FullImagePage(Photo image)
        {
            BindingContext = image.Image;
            InitializeComponent();
        }
    }
}