using RampUp_ToDo.ViewModels;
using System.Windows;

namespace RampUp_ToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskViewModel taskViewModel;
        public MainWindow()
        {
            InitializeComponent();
            taskViewModel = new TaskViewModel();
            DataContext = taskViewModel;
        }
        private void ListBoxItem_OnSelected(object sender, RoutedEventArgs e)
        {
            var storageTypes = taskViewModel.StoringTypes;
            var item = storageTypes[cbStorage.SelectedIndex];
            if (item != null)
            {
                taskViewModel.SelectedStorageType = item;
                taskViewModel.AllTodos = taskViewModel.FillData(item);
                DataContext = taskViewModel;
            }
        }
    }
}