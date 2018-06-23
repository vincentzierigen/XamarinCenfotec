using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Tarea2.Views;

namespace Tarea2
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
    }

    async void Handle_Clicked(object sender, System.EventArgs e)
    {
      await Navigation.PushAsync(new Register());
    }
  }
}
