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

public partial class ExcursionsView : UserControl
{
    private List<Excursion> _allExcursions = new();
    private List<Category> _categories = new();

    public ExcursionsView()
    {
        InitializeComponent();

        this.Loaded += async (_, _) =>
        {
            await LoadReferenceDataAsync();
            await LoadExcursionsAsync();
        };
    }

    private async Task LoadReferenceDataAsync()
    {
        // Загружаем категории из БД (или откуда у вас данные)
        var db = App.dbContext;
        _categories = await db.Categories.ToListAsync();
        CategoryFilter.ItemsSource = _categories;
        CategoryFilter.DisplayMemberBinding = new Avalonia.Data.Binding("CategoryName");
    }

    private async Task LoadExcursionsAsync()
    {
        var db = App.dbContext;
        IQueryable<Excursion> query = db.Excursions
            .Include(e => e.Category)
            .Include(e => e.ExcursionSessions);

        if (CategoryFilter.SelectedItem is Category selectedCategory)
        {
            query = query.Where(e => e.CategoryId == selectedCategory.CategoryId);
        }

        _allExcursions = await query.ToListAsync();
        ExcursionsListBox.ItemsSource = _allExcursions;
    }

    private void FilterButton_Click(object? sender, RoutedEventArgs e)
    {
        _ = LoadExcursionsAsync();
    }

    private void ClearFiltersButton_Click(object? sender, RoutedEventArgs e)
    {
        CategoryFilter.SelectedIndex = -1;
        _ = LoadExcursionsAsync();
    }

    private void ShowReviewsButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Excursion excursion)
        {
            var mainWindow = this.VisualRoot as MainWindow;
            mainWindow?.ShowReviews(excursion.Reviews.ToList());
        }
    }
}