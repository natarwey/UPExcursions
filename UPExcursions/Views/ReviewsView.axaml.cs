using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using UPExcursions.Data;

namespace UPExcursions.Views;

public partial class ReviewsView : UserControl
{
    public List<Review> Reviews { get; set; } = new();
    public ReviewsView()
    {
        InitializeComponent();
    }

    public ReviewsView(List<Review> reviews) : this()
    {
        Reviews = reviews;
        //RreviewList.ItemsSource = Review;
    }

}