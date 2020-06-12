﻿using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GibDD.Pages
{
    public class DeleteAppeal : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public ListView _appealList;

        public DeleteAppeal(ListView appealList)
        {
            _appealList = appealList;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var appeal = (Appeal)parameter;
            _appealList.ItemsSource = null;
            App.Database.DeleteItem(appeal.Id, "Appeals");
            _appealList.ItemsSource = App.Database.GetItems("Appeals");
        }
    }
    public class SelectAppeal : ICommand
    {
        public event EventHandler CanExecuteChanged;
        Editor _AppealEditor;
        MainModel _MainModel;

        public SelectAppeal(Editor editor, MainModel mainModel) {
            _AppealEditor = editor;
            _MainModel = mainModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var appeal = (Appeal)parameter;
            _AppealEditor.Text = appeal.Text;
            _MainModel.selectedAppeal = appeal;
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppealPage : ContentPage
    {
        public DeleteAppeal DeleteAppeal { get; private set; }
        public SelectAppeal SelectAppeal { get; private set; }
        public MainModel _MainModel { get; set; }

        public AppealPage(MainModel mainModel)
        {
            _MainModel = mainModel;
            BindingContext = this;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            appealsList.ItemsSource = App.Database.GetItems("Appeals");
            DeleteAppeal = new DeleteAppeal(appealsList);
            SelectAppeal = new SelectAppeal(AppealEditor, _MainModel);
            if (_MainModel.selectedAppeal != null)
                AppealEditor.Text = _MainModel.selectedAppeal.Text;
            base.OnAppearing();
        }

        private async void AddPhotos(object sender, EventArgs e)
        {
            Appeal appeal = new Appeal();
            appeal.Text = AppealEditor.Text;
            if (App.Database.GetItemsByQuery(String.Format("SELECT * FROM Appeals WHERE Text = \"{0}\"", appeal.Text)).Count() == 0)
            {
                App.Database.SaveItem("Appeals", appeal);
                appealsList.ItemsSource = App.Database.GetItems("Appeals");
            }

            _MainModel.selectedAppeal = appeal;
            await Navigation.PushAsync(new Pages.PhotosPage(_MainModel));
        }

        private void ChangeAppealsText(object sender, TextChangedEventArgs e)
        {
            Editor editor = (Editor)sender;
            editor.BackgroundColor = Color.FromRgba(255, 255, 255, 1.0);
            if (String.IsNullOrEmpty(editor.Text))
            {
                editor.BackgroundColor = Color.FromRgba(255, 0, 0, 0.2);
                editor.Placeholder = "Необходимо заполнить поле";
                ContinueButton.IsEnabled = false;
            } else {
                ContinueButton.IsEnabled = true;
            }
        }

        private void appealsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void ClearEditor(object sender, EventArgs e)
        {
            AppealEditor.Text = "";
        }
    }
}
