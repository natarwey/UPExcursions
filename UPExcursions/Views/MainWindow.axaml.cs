using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using UPExcursions.Data;
using UPExcursions.Views;

namespace UPExcursions.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (_, _) => UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            ExcursionsButton.IsVisible = CurrentUser.CanViewExcursions;

            FavoritesButton.IsVisible = CurrentUser.CanManageFavorites;
            MyOrdersButton.IsVisible = CurrentUser.CanViewOwnBookings;

            OrderReportButton.IsVisible = CurrentUser.CanViewOrderReport;
            PopularityReportButton.IsVisible = CurrentUser.CanViewPopularityReport;
        }

        private async void ExcursionsButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainContent.Content = new ExcursionsView();
        }

        public void ShowReviews(int excursionId)
        {
            MainContent.Content = new ReviewsView(excursionId);
        }

        public void GoBack()
        {
            MainContent.Content = new ExcursionsView();
        }

        private void FavoritesButton_Click(object? sender, RoutedEventArgs e)
        {

            MainContent.Content = new FavoritesView();
        }

        private void OrdersButton_Click(object? sender, RoutedEventArgs e)
        {

            MainContent.Content = new OrdersHistoryView();
        }

        public void ShowBookingView(Excursion excursion)
        {
            MainContent.Content = new BookingView(excursion);
        }

        public void OrderReportButton_Click(object? sender, RoutedEventArgs e)
        {
            MainContent.Content = new OrderReportView();
        }

        public void ExcursionReportButton_Click(object? sender, RoutedEventArgs e)
        {
            MainContent.Content = new ExcursionReportView();
        }

        private void LogoutButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CurrentUser.currentUser = null;

            var authWindow = new AuthWindow();
            authWindow.Show();

            this.Close();
        }
    }
}