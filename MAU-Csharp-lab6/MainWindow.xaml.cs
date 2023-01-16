using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Controls;
using System.Linq;
using MAU_Charp_lab6;

namespace MAU_Csharp_lab6;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    TaskManager taskManager;
    Task currentTask;
    public MainWindow()
    {
        InitializeComponent();
        ResetGUI();
        ViewModel viewModel = new ViewModel();
        taskManager = new TaskManager();
        currentTask = new Task();
    }


    /// <summary>
    /// Reset all fields and start over
    /// </summary>
    public void ResetGUI()
    {
        btn_change.IsEnabled = false;
        btn_delete.IsEnabled = false;       
        dp_date.SelectedDate = null;
        cbx_time.SelectedIndex = 8;
        cbx_priority.SelectedIndex = 2;
        tbx_toDo.Text = null;
        lbx_toDoList.ItemsSource = null;
        lbl_info.Content = "Please enter all requied info";
        
    }


    /// <summary>
    /// 
    /// </summary>
    private void btn_Add_Click(object sender, RoutedEventArgs e)
    {        
        // The UI button toggle method prevents the values from beeing null
        // Pass in -1 to indicate that we are adding a task (not changing)
        taskManager.AddOrChangeTask((DateTime)dp_date.SelectedDate, cbx_time.SelectedValue.ToString(), cbx_priority.SelectedIndex, tbx_toDo.Text, -1);

        // Output the object to the listbox
        UpdateListBox();        
    }



    private void btn_Change_Click(object sender, RoutedEventArgs e)
    {
        int index = lbx_toDoList.SelectedIndex;
        taskManager.AddOrChangeTask((DateTime)dp_date.SelectedDate, cbx_time.SelectedItem.ToString(), cbx_priority.SelectedIndex, tbx_toDo.Text, index);
        UpdateListBox();
    }


    private void btn_Delete_Click(object sender, RoutedEventArgs e)
    {
        taskManager.DeleteTask(lbx_toDoList.SelectedIndex);
        UpdateListBox();
    }


    private void cbx_PriorityChanged(object sender, RoutedEventArgs e)
    {
        // Call the toggle method and pass in the boolean result of CheckForRequired-method
        ToggleAddButtonAndWarningText(CheckForRequired());
    }


    private void tbx_TodoChanged(object sender, RoutedEventArgs e)
    {
        ToggleAddButtonAndWarningText(CheckForRequired());
    }


    private void dp_DateChanged(object sender, RoutedEventArgs e)
    {
        ToggleAddButtonAndWarningText(CheckForRequired());
    }


    /// <summary>
    /// Listens for selection change in the list box. If a task is selected, the buttons
    /// change, and delete will be enabled. If nothing is selected, the buttons will be disabled.
    /// When something is selected, the data from that task is shown in the datepicker, comboboxes
    /// and the textbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void lbx_Selection_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (lbx_toDoList.SelectedIndex < 0 || lbx_toDoList.SelectedIndex == null)
        {
            btn_change.IsEnabled = false;
            btn_delete.IsEnabled = false;
        }
        else
        {
            btn_change.IsEnabled = true;    
            btn_delete.IsEnabled = true;

            Task t = taskManager.GetOneTask(lbx_toDoList.SelectedIndex);

            // Set the datepicker to the selected task's date
            dp_date.SelectedDate = t.TaskDateAndTime.Date;

            // Set the time combobox to the selected task's time
            cbx_time.SelectedValue = t.Time;

            // Set the priority combobox to the selected Task's priority
            cbx_priority.SelectedIndex = (int)t.Pt;

            // Set the to do-textbox to the selected Task's to-do text.
            tbx_toDo.Text = t.ToDoText;
        }
    }


    // Menu - New
    private void ExecutedNewCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (taskManager != null)
        {
            taskManager.ClearTasks();
        }
        ResetGUI();
        // New code
    }


    private void CanExecuteNewCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }


    // Menu - Open
    private void ExecutedOpenCommand(object sender, ExecutedRoutedEventArgs e)
    {        
        FileManager fileManager = new FileManager(taskManager);
        if (fileManager.Read())
            ResetGUI();
        UpdateListBox();
    }


    private void CanExecuteOpenCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }



    /// <summary>
    /// Actions to take when the user clicks Exit in the menu or presses
    /// Ctrl+Q. A confirmation dialog is shown. If the user clicks OK, the application
    /// terminates. If the user clicks cancel, the application is not closed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExecutedCloseCommand(object sender, ExecutedRoutedEventArgs e)
    {
        MessageBoxResult mbr = MessageBox.Show("Are you sure?", "Exit program", MessageBoxButton.OKCancel);
        if (mbr == MessageBoxResult.OK)
        {
            Application.Current.Shutdown();
        }       
    }


    private void CanExecuteCloseCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }


    /// <summary>
    /// Method is called when the save-option in the menu is clicked, or when the associated 
    /// shortcut is pressed. Opens the save file dialog.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExecutedSaveCommand(object sender, ExecutedRoutedEventArgs e)
    {
        FileManager fileManager = new FileManager(taskManager);
        fileManager.Save();        
    }


    private void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }


    /// <summary>
    /// Check all required fields if the user has typed in and selected something
    /// </summary>
    /// <returns>True if all required fields are entered, otherwise returns false.</returns>
    private bool CheckForRequired()
    {       
        if (dp_date.SelectedDate == null) return false;
        if (cbx_time.SelectedIndex == null || cbx_time.SelectedIndex == -1) return false;
        if (cbx_priority.SelectedIndex == null || cbx_priority.SelectedIndex == -1) return false;
        if (tbx_toDo.Text == null || tbx_toDo.Text == "") return false;
        return true;
    }


    /// <summary>
    /// Enable or disable add-button and warning text based on the bool that is passed in
    /// </summary>
    /// <param name="isValidInput">Boolean that determines if all valid input is entered or not.</param>
    private void ToggleAddButtonAndWarningText(bool isValidInput)
    {
        lbl_info.Foreground = Brushes.Red;
        lbl_info.Content = "Please enter all required info";
        btn_add.IsEnabled = isValidInput;
        if (isValidInput)
        {
            lbl_info.Content = "All info entered";
            lbl_info.Foreground = Brushes.Green;
        }
    }


    

    private void UpdateListBox()
    {
        lbx_toDoList.ItemsSource = null;
        lbx_toDoList.ItemsSource = taskManager.GetAllTasksAsList();
    }
}
