using System.Printing;
using System.Windows;
using System.Windows.Input;

namespace MAU_Csharp_lab6;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ViewModel viewModel = new ViewModel();
    }


    // Menu - New
    private void ExecutedNewCommand(object sender, ExecutedRoutedEventArgs e)
    {
        MessageBox.Show("new");
        // New code
    }

    private void CanExecuteNewCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }


    // Menu - Open
    private void ExecutedOpenCommand(object sender, ExecutedRoutedEventArgs e)
    {
        MessageBox.Show("open");
        // Open code
    }

    private void CanExecuteOpenCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }



    // Menu - Exit
    private void ExecutedCloseCommand(object sender, ExecutedRoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void CanExecuteCloseCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }


    // Menu - Save
    private void ExecutedSaveCommand(object sender, ExecutedRoutedEventArgs e)
    {
        MessageBox.Show("save");
        // Save code
    }

    private void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
}
