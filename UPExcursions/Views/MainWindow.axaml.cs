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
    }
}