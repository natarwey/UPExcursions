using Avalonia.Controls;
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

        public void ShowReviews(List<Review> reviews)
        {
            MainContent.Content = new ReviewsView(reviews);
        }
    }
}