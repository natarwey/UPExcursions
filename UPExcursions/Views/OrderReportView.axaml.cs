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

public partial class OrderReportView : UserControl
{
    private List<Order> _allOrders = new();

    public OrderReportView()
    {
        InitializeComponent();
        StatusFilter.SelectedIndex = 0;
        this.Loaded += async (_, _) => await LoadOrdersAsync();
    }

    private async Task LoadOrdersAsync(string? statusFilter = null)
    {
        var db = App.dbContext;

        IQueryable<Order> query = db.Orders
            .Include(o => o.Payment);

        if (statusFilter == "pending" || statusFilter == "confirmed")
        {
            query = query.Where(o => o.Status == statusFilter);
        }
        else
        {
            query = query.Where(o => o.Status == "confirmed");
        }

        _allOrders = await query
            .OrderBy(o => o.OrderId)
            .ToListAsync();

        OrdersDataGrid.ItemsSource = _allOrders;
    }

    private async void FilterButton_Click(object? sender, RoutedEventArgs e)
    {
        var selectedStatus = (StatusFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
        await LoadOrdersAsync(selectedStatus);
    }

    private async void ClearFilterButton_Click(object? sender, RoutedEventArgs e)
    {
        StatusFilter.SelectedIndex = 0;
        await LoadOrdersAsync("confirmed");
    }
}