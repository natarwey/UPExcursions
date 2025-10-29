using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPExcursions.Data;

namespace UPExcursions.Views;

public partial class OrdersHistoryView : UserControl
{
    public OrdersHistoryView()
    {
        InitializeComponent();
        this.Loaded += async (_, _) => await LoadOrdersAsync();
    }

    private async Task LoadOrdersAsync()
    {
        if (CurrentUser.currentUser == null)
        {
            OrdersList.ItemsSource = new List<Order>();
            return;
        }

        var db = App.dbContext;
        var orders = await db.Orders
            .Include(o => o.Session)
                .ThenInclude(s => s.Excursion)
            .Where(o => o.UserId == CurrentUser.currentUser.UserId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        OrdersList.ItemsSource = orders;
    }
}