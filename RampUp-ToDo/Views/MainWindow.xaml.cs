using RampUp_ToDo.Data;
using RampUp_ToDo.ViewModels;
using System.Windows;

namespace RampUp_ToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly TaskViewModel _taskViewModel;
        public IEnumerable<StorageViewModel> StoringTypes =
        [
            new StorageViewModel("JSON", new DataContextFile()), new StorageViewModel("Database", new DataContextDB())
        ];

        public MainWindow()
        {
            InitializeComponent();
            _taskViewModel = new TaskViewModel(StoringTypes);

            DataContext = _taskViewModel;
        }
    }
}