using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace UPExcursions.Views;

public partial class ConfirmationWindow : Window
{
    public ConfirmationWindow(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
    }

    private void OkButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}