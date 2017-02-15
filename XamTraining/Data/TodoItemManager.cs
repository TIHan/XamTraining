using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace XamTraining
{
    public class TodoItemManager
    {
        private static TodoItemManager instance;
        public static TodoItemManager Instance => instance ?? (instance = new TodoItemManager());

        public MobileServiceClient Client { get; private set; }
        public MobileServiceUser User { get; set; }

        IMobileServiceSyncTable<TodoItem> todoTable;
        MobileServiceSQLiteStore store;

        static bool isInitialized = false;

        private TodoItemManager()
        {
            var path = "syncstore.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);
            store = new MobileServiceSQLiteStore(path);
            Client = new MobileServiceClient("https://mobile-e1e3e522-d2b7-44a1-8d61-7657a5ac1001.azurewebsites.net/");
        }


        static SemaphoreSlim sempahoreSlim = new SemaphoreSlim(1);
        private async Task InitializeAsync()
        {
            await sempahoreSlim.WaitAsync();
            try
            {
                if (isInitialized)
                    return;

                store.DefineTable<TodoItem>();
                await Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler()).ConfigureAwait(false);
                todoTable = Client.GetSyncTable<TodoItem>();
                isInitialized = true;
            }
            finally
            {
                sempahoreSlim.Release();
            }
        }

        public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsync(bool syncItems = false)
        {
            await InitializeAsync().ConfigureAwait(false);

            if (syncItems)
                await SyncAsync().ConfigureAwait(false);

            IEnumerable<TodoItem> items = await todoTable.ToEnumerableAsync();

            return new ObservableCollection<TodoItem>(items);
        }

        public async Task AddTodoItemAsync(TodoItem item)
        {
            await InitializeAsync().ConfigureAwait(false);
            await todoTable.InsertAsync(item);
        }

        public async Task UpdateTodoItemAsync(TodoItem item)
        {
            await InitializeAsync().ConfigureAwait(false);
            await todoTable.UpdateAsync(item);
        }

        public async Task DeleteTodoItemAsync(TodoItem item)
        {
            await InitializeAsync().ConfigureAwait(false);
            await todoTable.DeleteAsync(item);
        }

        public async Task SyncAsync()
        {
            await InitializeAsync().ConfigureAwait(false);

            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.Client.SyncContext.PushAsync().ConfigureAwait(false);
                await this.todoTable.PullAsync("allTodoItems", this.todoTable.CreateQuery()).ConfigureAwait(false);
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        // Update failed, revert to server's copy
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
    }
}