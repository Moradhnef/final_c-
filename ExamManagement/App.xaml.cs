using EasyTestMaker.Services;
using EasyTestMaker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace EasyTestMaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    // Define the application class
    public partial class App : Application
    {
        // Declare a static IServiceProvider to be used across the application
        private static IServiceProvider provider;

        // App constructor
        public App()
        {
            // Create a new service collection
            ServiceCollection collection = new ServiceCollection();

            // Configure services for the application
            ConfigureServices(collection);

            // Build the service provider from the service collection
            provider = collection.BuildServiceProvider();
        }

        // Method to configure services for dependency injection
        private void ConfigureServices(ServiceCollection service)
        {
            // Register services with their respective interfaces
            service.AddTransient<IAuthService, AuthService>();
            service.AddSingleton<IEventAggregator, EventAggregator>();
            service.AddTransient<ITestService, TestService>();
            service.AddTransient<IImageService, ImageService>();

            // Register ViewModels
            service.AddTransient<SplashScreenViewModel>();
            service.AddTransient<HomeViewModel>();
            service.AddTransient<MenuViewModel>();
            service.AddTransient<MainViewModel>();
            service.AddTransient<LoginViewModel>();
            service.AddTransient<QuestionViewModel>();
            service.AddTransient<CreateTestViewModel>();
            service.AddTransient<CreateStudentViewModel>();
            service.AddTransient<ResultsViewModel>();
            service.AddTransient<StudentMenuViewModel>();
            service.AddTransient<TestViewModel>();
            service.AddTransient<ResultsDetailsViewModel>();

        }

        // Method to get the registered service of the specified type
        public static T GetService<T>() where T : class
        {
            return (T)provider.GetService<T>();
        }

        // Method to handle the application startup event
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Create a new MainWindow instance
            var window = new MainWindow();

            // Set the DataContext of the window to the MainViewModel
            window.DataContext = provider.GetService<MainViewModel>();

            // Show the window
            window.Show();
        }
    }

}
