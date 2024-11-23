using L4.Services;

namespace P12MAUI.Client.MessageBox
{
    internal class MauiMessageDialogService : IMessageDialogService
    {
        public void ShowMessage(string message)
        {
            Shell.Current.DisplayAlert("Message", message, "OK");
        }
    }
}
