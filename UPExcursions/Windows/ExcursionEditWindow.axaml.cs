using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPExcursions.Data;

namespace UPExcursions.Views;

public partial class ExcursionEditWindow : Window
{
    private readonly Excursion _excursion;
    public List<Category> Categories { get; private set; } = new();

    public ExcursionEditWindow()
    {
        InitializeComponent();
        _excursion = new Excursion();
        DataContext = _excursion;
        _ = LoadAndInitAsync();
    }

    public ExcursionEditWindow(Excursion excursion) : this()
    {
        _excursion = excursion;
        DataContext = _excursion;
    }

    private async Task LoadAndInitAsync()
    {
        var db = App.dbContext;
        Categories = await db.Categories.ToListAsync();

        CategoryComboBox.ItemsSource = Categories;

        if (_excursion.ExcursionId != 0)
        {
            if (_excursion.Category == null && _excursion.CategoryId != 0)
            {
                _excursion.Category = Categories.FirstOrDefault(c => c.CategoryId == _excursion.CategoryId);
            }

            DurationBox.Text = _excursion.DurationHours == 0 ? "" : _excursion.DurationHours.ToString();
            PriceBox.Text = _excursion.BasePrice == 0 ? "" : _excursion.BasePrice.ToString();
        }
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_excursion.Title) ||
            CategoryComboBox.SelectedItem is not Category selectedCategory)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(DurationBox.Text))
        {
            if (!decimal.TryParse(DurationBox.Text, out var duration) || duration <= 0)
            {
                return;
            }
            _excursion.DurationHours = duration;
        }
        else
        {
            _excursion.DurationHours = 0;
        }

        if (!string.IsNullOrWhiteSpace(PriceBox.Text))
        {
            if (!decimal.TryParse(PriceBox.Text, out var price) || price < 0)
            {
                return;
            }
            _excursion.BasePrice = price;
        }
        else
        {
            _excursion.BasePrice = 0;
        }

        _excursion.Category = selectedCategory;
        _excursion.CategoryId = selectedCategory.CategoryId;

        var db = App.dbContext;
        if (_excursion.ExcursionId == 0)
        {
            _excursion.CreatedAt = DateTime.Now;
            db.Excursions.Add(_excursion);
        }
        else
        {
            db.Excursions.Update(_excursion);
        }

        db.SaveChanges();
        Close(true);
    }

    private void CancelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(false);
    }
}