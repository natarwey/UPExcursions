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

public partial class FavoritesView : UserControl
{
    public FavoritesView()
    {
        InitializeComponent();
        this.Loaded += async (_, _) =>
        {
            await LoadUserProfileAsync();
            await LoadFavoritesAsync();
        };
    }

    private async Task LoadUserProfileAsync()
    {
        if (CurrentUser.currentUser == null)
            return;

        var user = CurrentUser.currentUser;

        EmailBox.Text = user.Email;
        FirstNameBox.Text = user.FirstName;
        LastNameBox.Text = user.LastName;
        PhoneBox.Text = user.Phone ?? "—";
        RoleBox.Text = user.Role?.RoleName ?? "—";
    }

    private async Task LoadFavoritesAsync()
    {
        if (CurrentUser.IsAdmin)
        {
            FavoritesListBox.ItemsSource = new List<Excursion>();
            return;
        }

        if (CurrentUser.currentUser == null)
        {
            FavoritesListBox.ItemsSource = new List<Excursion>();
            return;
        }

        var userId = CurrentUser.currentUser.UserId;
        var db = App.dbContext;

        var favoriteExcursions = await db.Excursions
            .Include(e => e.Category)
            .Include(e => e.ExcursionSessions)
            .Where(e => e.Favorites.Any(f => f.UserId == userId))
            .ToListAsync();

        FavoritesListBox.ItemsSource = favoriteExcursions;
    }

    private void ShowReviewsButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Excursion excursion)
        {
            var mainWindow = this.VisualRoot as MainWindow;
            mainWindow?.ShowReviews(excursion.ExcursionId);
        }
    }

    private async void AddToFavoritesButton_Click(object? sender, RoutedEventArgs e)
    {
        if (CurrentUser.currentUser == null)
            return;

        if (sender is Button button && button.Tag is Excursion excursion)
        {
            var db = App.dbContext;

            var existing = await db.Favorites
                .FirstOrDefaultAsync(f => f.UserId == CurrentUser.currentUser.UserId && f.ExcursionId == excursion.ExcursionId);

            if (existing != null)
            {
                db.Favorites.Remove(existing);
            }
            else
            {
                db.Favorites.Add(new Favorite
                {
                    UserId = CurrentUser.currentUser.UserId,
                    ExcursionId = excursion.ExcursionId
                });
            }
            await db.SaveChangesAsync();
            await LoadFavoritesAsync();
        }
    }
}