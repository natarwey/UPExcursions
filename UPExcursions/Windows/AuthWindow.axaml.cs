using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using UPExcursions.Data;
using UPExcursions.Views;

namespace UPExcursions.Views;

public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object? sender, RoutedEventArgs e)
    {
        var email = EmailBox.Text?.Trim();
        var password = PasswordBox.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return;
        }

        var user = App.dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

        if (user != null)
        {
            CurrentUser.currentUser = user;
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        else
        {

        }
    }

    private void RegisterButton_Click(object? sender, RoutedEventArgs e)
    {
        var email = EmailBox.Text?.Trim();
        var password = PasswordBox.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return;
        }

        if (App.dbContext.Users.Any(u => u.Email == email))
        {
            return;
        }

        var clientRole = App.dbContext.Roles.FirstOrDefault(r => r.RoleName == "client");
        if (clientRole == null)
        {
            return;
        }

        var newUser = new User
        {
            Email = email,
            PasswordHash = password,
            FirstName = "New",
            LastName = "User",
            RoleId = clientRole.RoleId,
            CreatedAt = DateTime.Now
        };

        App.dbContext.Users.Add(newUser);
        App.dbContext.SaveChanges();

        CurrentUser.currentUser = newUser;
        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }
}