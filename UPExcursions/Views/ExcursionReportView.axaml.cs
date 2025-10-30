using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace UPExcursions.Views;

public partial class ExcursionReportView : UserControl
{
    public ExcursionReportView()
    {
        InitializeComponent();
        this.Loaded += async (_, _) => await LoadPopularityReportAsync();
    }

    private async Task LoadPopularityReportAsync()
    {
        var db = App.dbContext;

        var reportData = await db.Excursions
            .Select(e => new
            {
                Title = e.Title,
                CategoryName = e.Category.CategoryName,
                TotalParticipants = e.ExcursionSessions
                    .SelectMany(es => es.Orders)
                    .Sum(o => (int?)o.ParticipantsCount) ?? 0
            })
            .Where(x => x.TotalParticipants > 0)
            .OrderByDescending(x => x.TotalParticipants)
            .ToListAsync();

        PopularityDataGrid.ItemsSource = reportData;
    }
}