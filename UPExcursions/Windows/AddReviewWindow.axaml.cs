using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;
using UPExcursions.Data;

namespace UPExcursions.Views;

public partial class AddReviewWindow : Window
{
    private readonly Review _review;
    private readonly int _excursionId;

    public AddReviewWindow(int excursionId)
    {
        InitializeComponent();
        _excursionId = excursionId;

        _review = new Review
        {
            UserId = CurrentUser.currentUser!.UserId,
            ExcursionId = _excursionId,
            CreatedAt = DateTime.Now
        };

        DataContext = _review;
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        if (RatingBox.SelectedItem is ComboBoxItem item && int.TryParse(item.Content.ToString(), out int rating))
        {
            _review.Rating = rating;
        }
        else
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_review.Comment))
        {
            return;
        }

        var db = App.dbContext;

        var existing = db.Reviews
            .FirstOrDefault(r => r.UserId == _review.UserId && r.ExcursionId == _excursionId);

        if (existing != null)
        {
            existing.Rating = _review.Rating;
            existing.Comment = _review.Comment;
            existing.CreatedAt = DateTime.Now;
            db.Reviews.Update(existing);
        }
        else
        {
            db.Reviews.Add(_review);
        }

        db.SaveChanges();
        Close(true);
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}