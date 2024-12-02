using System.Windows;
using L4.Services;

namespace Shop.WPF
{
    internal class WpfMessageDialogService : IMessageDialogService
    {
        public void ShowMessage(string message)
        {
            // Użycie MessageBox do wyświetlenia komunikatu
            MessageBox.Show(message, "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
