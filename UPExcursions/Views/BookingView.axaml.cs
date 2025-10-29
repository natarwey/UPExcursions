using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPExcursions.Data;

namespace UPExcursions.Views;

public partial class BookingView : UserControl
{
    private readonly Excursion _excursion;
    private List<ExcursionSession> _sessions = new();

    public BookingView(Excursion excursion)
    {
        InitializeComponent();
        _excursion = excursion;
        TitleText.Text = excursion.Title;
        this.Loaded += async (_, _) => await LoadSessionsAsync();
    }

    private async Task LoadSessionsAsync()
    {
        var db = App.dbContext;
        _sessions = await db.ExcursionSessions
            .Where(s => s.ExcursionId == _excursion.ExcursionId && s.AvailableSpots > 0)
            .OrderBy(s => s.SessionDate)
            .ThenBy(s => s.StartTime)
            .ToListAsync();

        var sessionItems = _sessions.Select(s => new
        {
            Session = s,
            SessionDisplay = $"{s.SessionDate:dd.MM.yyyy} at {s.StartTime:HH:mm} ({s.AvailableSpots} spots left)"
        }).ToList();

        SessionComboBox.ItemsSource = sessionItems;
        if (sessionItems.Any())
            SessionComboBox.SelectedIndex = 0;

        UpdateTotalPrice();
    }

    private void UpdateTotalPrice()
    {
        if (int.TryParse(ParticipantsBox.Text, out int count) &&
            SessionComboBox.SelectedItem is object item &&
            item.GetType().GetProperty("Session")?.GetValue(item) is ExcursionSession session)
        {
            var total = _excursion.BasePrice * count;
            TotalPriceText.Text = $"Total: {total:F2} ₽";
        }
    }

    private void ParticipantsBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        UpdateTotalPrice();
    }

    private async void ConfirmButton_Click(object? sender, RoutedEventArgs e)
    {
        if (CurrentUser.currentUser == null) return;

        if (!int.TryParse(ParticipantsBox.Text, out int participants) || participants <= 0)
            return;

        if (SessionComboBox.SelectedItem is not object item ||
            item.GetType().GetProperty("Session")?.GetValue(item) is not ExcursionSession selectedSession)
            return;

        if (participants > selectedSession.AvailableSpots)
        {
            // Можно показать MessageBox
            return;
        }

        if (PaymentMethodBox.SelectedItem is not ComboBoxItem paymentItem)
            return;

        var paymentMethod = PaymentMethodBox.SelectedItem.ToString() ?? "cash";

        if (participants > selectedSession.AvailableSpots)
        {
            return;
        }

        var db = App.dbContext;

        var order = new Order
        {
            UserId = CurrentUser.currentUser.UserId,
            SessionId = selectedSession.SessionId,
            ParticipantsCount = participants,
            TotalPrice = _excursion.BasePrice * participants,
            Status = "confirmed",
            CreatedAt = DateTime.Now
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        var payment = new Payment
        {
            OrderId = order.OrderId,
            Amount = order.TotalPrice,
            PaymentMethod = paymentMethod,
            PaymentStatus = "paid",
            PaidAt = DateTime.Now
        };

        order.Payment = payment;

        selectedSession.AvailableSpots -= participants;
        //db.ExcursionSessions.Update(selectedSession);

        db.Orders.Add(order);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex.InnerException?.Message ?? ex.Message}");
            return;
        }

        var confirmWindow = new ConfirmationWindow("Your booking has been confirmed!");
        await confirmWindow.ShowDialog(this.VisualRoot as Window);

        var mainWindow = this.VisualRoot as MainWindow;
        mainWindow?.GoBack();
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        var mainWindow = this.VisualRoot as MainWindow;
        mainWindow?.GoBack();
    }
}