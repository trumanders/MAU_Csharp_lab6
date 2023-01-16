using MAU_Charp_lab6;
using System.Windows.Threading;

namespace MAU_Csharp_lab6;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const string WINDOW_TITLE = "Anders's To-Do Manager";
    private TaskManager taskManager;
    private FileManager fileManager;
    private bool isSaved;

    public MainWindow()
    {
        InitializeComponent();        
        ViewModel viewModel = new ViewModel();
        taskManager = new TaskManager();
        fileManager = new FileManager(taskManager);
        ResetGUI();

        // Live time
        DispatcherTimer LiveTime = new DispatcherTimer();
        LiveTime.Interval = TimeSpan.FromSeconds(1);
        LiveTime.Tick += timer_Tick;
        LiveTime.Start();
    }


    private void timer_Tick(object sender, EventArgs e)
    {
        lbl_liveTime.Content = DateTime.Now.ToString("HH:mm:ss");
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


    /// <summary>
    /// Performs actions when the change-button is clicked. Grabs the index
    /// of the selected item in the list box and calls the AddOrChange method. Updates the UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btn_Change_Click(object sender, RoutedEventArgs e)
    {
        int index = lbx_toDoList.SelectedIndex;
        taskManager.AddOrChangeTask((DateTime)dp_date.SelectedDate, cbx_time.SelectedItem.ToString(), cbx_priority.SelectedIndex, tbx_toDo.Text, index);
        UpdateListBox();
    }


    /// <summary>
    /// Deletes the selected task in the list box and updates the UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btn_Delete_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult mbr = MessageBox.Show("Are you sure?", "Delete task", MessageBoxButton.OKCancel);
        if (mbr == MessageBoxResult.OK)
        {
            taskManager.DeleteTask(lbx_toDoList.SelectedIndex);
            ResetGUI();
            UpdateListBox();
        }        
    }


    /// <summary>
    /// Performs actions when the priority combobox is changed and enables the buttons
    /// accordingly. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbx_PriorityChanged(object sender, RoutedEventArgs e)
    {
        // Call the toggle method and pass in the boolean result of CheckForRequired-method
        ToggleAddButtonAndWarningText(CheckForRequired());
    }


    /// <summary>
    /// Performs actions when the todo-textbox is changed, and enables/disables the
    /// buttons accordingly.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tbx_TodoChanged(object sender, RoutedEventArgs e)
    {
        ToggleAddButtonAndWarningText(CheckForRequired());
    }


    /// <summary>
    /// Performs actions when the datepicker is changed, and enables/disables the
    /// buttons accordingly.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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


    /// <summary>
    /// Performs actions when New is selected in the menu. Clears all tasks and
    /// resets the UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExecutedNewCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (taskManager != null)
        {
            taskManager.ClearTasks();
        }
        isSaved = false;
        ResetGUI();

        // Remove filename from window title
        this.Title = WINDOW_TITLE;
    }


    /// <summary>
    /// Enables the new command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CanExecuteNewCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }


    /// <summary>
    /// Performs actions when Open is selected in the menu. Calls the Read method in the
    /// task manager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExecutedOpenCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (fileManager.Read())
        {
            ResetGUI();         /* Clean the field after open file */
            UpdateListBox();    /* Update the tasks list box */   
            isSaved = true;     /* Save As... will not be called upon save */

            SetWindowTitleToFileName();
        }
    }


    /// <summary>
    /// Enables the open command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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


    /// <summary>
    /// Enables the Close command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        // If the user has not saved at least once, the save as method is called
        if (isSaved)
        {
            fileManager.Save();
            SetWindowTitleToFileName();
        }
        else
        {
            ExecutedSaveAsCommand(sender, e);
        }
    }


    /// <summary>
    /// Enables the Save command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }


    /// <summary>
    /// Performs actions when Save AS... is selected in the menu. Calls the SaveAs-mehtod in the
    /// task manager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExecutedSaveAsCommand(object sender, ExecutedRoutedEventArgs e)
    {        
        fileManager.SaveAs();
        isSaved = true;
        SetWindowTitleToFileName();
    }


    /// <summary>
    /// Enables the Save as command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CanExecuteSaveAsCommand(object sender, CanExecuteRoutedEventArgs e)
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
    

    /// <summary>
    /// Sets the item source of the list box to show all in memory tasks.
    /// </summary>
    private void UpdateListBox()
    {
        lbx_toDoList.ItemsSource = null;
        lbx_toDoList.ItemsSource = taskManager.GetAllTasksAsList();
    }


    private void SetWindowTitleToFileName()
    {
        this.Title = System.IO.Path.GetFileName(fileManager.FileName) + " - " + WINDOW_TITLE;
    }


    private void MenuItem_About_Click(object sender, RoutedEventArgs e)
    {        
        
    }
}
