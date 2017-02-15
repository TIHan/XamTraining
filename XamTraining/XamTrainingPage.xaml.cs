using System;
using Xamarin.Forms;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using System.Collections.ObjectModel;

namespace XamTraining
{
    public partial class XamTrainingPage : ContentPage
    {
        public XamTrainingPage()
        {
            BindingContext = this;

            InitializeComponent();

            this.TestCrash.Clicked += (sender, e) =>
            {
                throw new Exception("Mobile Center Crash Example");
            };

            this.TestEvent.Clicked += (sender, e) =>
            {
                Analytics.TrackEvent("Mobile Center Event Example");
            };

            AddTodoItem.Clicked += async (sender, e) =>
            {
                try
                {
                    AddTodoItem.IsEnabled = false;

                    if (string.IsNullOrEmpty(TodoEntry.Text))
                    {
                        await DisplayAlert("Error", "Text cannot be empty", "OK");
                        return;
                    }

                    var todoItem = new TodoItem
                    {
                        Title = TodoEntry.Text,
                    };

                    await TodoItemManager.Instance.AddTodoItemAsync(todoItem);
                    await DisplayAlert("Success", "Todo Item Added to Local Sync Table", "OK");
                }
                finally
                {
                    TodoEntry.Text = null;
                    AddTodoItem.IsEnabled = true;
                }
            };

            GetTodosButton.Clicked += async (sender, e) =>
            {
                LoadingIndicator.IsRunning = true;
                GetTodosButton.IsEnabled = false;

                TodoItems = await TodoItemManager.Instance.GetTodoItemsAsync(true);

                LoadingIndicator.IsRunning = false;
                GetTodosButton.IsEnabled = true;
            };

            LoginButton.Clicked += async (sender, e) =>
            {
                var user = await App.Authenticator.Authenticate();
                if (user != null)
                {
                    IsAuthenticated = true;
                    TodoItemManager.Instance.User = user;
                }
            };
        }

        private ObservableCollection<TodoItem> todoItems = new ObservableCollection<TodoItem>();
        public ObservableCollection<TodoItem> TodoItems
        {
            get
            {
                return todoItems;
            }
            set
            {
                todoItems = value;
                OnPropertyChanged(nameof(TodoItems));
            }
        }


        private InvertableBool isAuthenticated = false;
        public InvertableBool IsAuthenticated
        {
            get
            {
                return isAuthenticated;
            }
            set
            {
                isAuthenticated = value;
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }
    }
}
