using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPExcursions.Data;

namespace UPExcursions.Views;

public partial class ReviewsView : UserControl
{
    private readonly int _excursionId;

    public ReviewsView(int excursionId)
    {
        InitializeComponent();
        _excursionId = excursionId;

        this.Loaded += async (_, _) => await LoadReviewsAsync();
    }

    private async Task LoadReviewsAsync()
    {
        var db = App.dbContext;
        var reviews = await db.Reviews
            .Include(r => r.User)
            .Where(r => r.ExcursionId == _excursionId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        ReviewsList.ItemsSource = reviews;
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        var mainWindow = this.VisualRoot as MainWindow;
        mainWindow?.GoBack();
    }

    private async void AddReviewButton_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddReviewWindow(_excursionId);
        var owner = this.VisualRoot as Window;
        var result = await window.ShowDialog<bool>(owner);

        if (result == true)
        {
            await LoadReviewsAsync();
        }
    }

}