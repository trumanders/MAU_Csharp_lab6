namespace MAU_Charp_lab6;

public class FileManager
{
    private const string FILE_ID = "7904255556ToDoList";
    private TaskManager taskManager;
    private string numberOfTasks;
    
    private string filename;
    public string FileName { get { return filename; } }


    public FileManager(TaskManager taskManager)
    {
        this.taskManager = taskManager;                
        filename = null;
    }

    /// <summary>
    /// Save the tasks to file if the file already is opened.
    /// </summary>
    /// <returns>True if save was successful, otherwise false.</returns>
    public bool Save()
    {
        // Save the existing file
        try
        {
            File.WriteAllLines(this.filename, taskManager.GetTasksAsFileText());
        }
        catch (Exception ex)
        {
            MessageBox.Show("" + ex.ToString());
            return false;
        }
        
        return true;
    }


    /// <summary>
    /// Save the tasks to a new file. This method is called from Save() if there is no opened file yet.
    /// </summary>
    /// <returns>True if save as was successful, otherwise false.</returns>
    public bool SaveAs()
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "Text Files (*.txt)|*.txt";
        sfd.DefaultExt = "txt";
        if (sfd.ShowDialog() == true)
        {
            try
            {
                File.WriteAllLines(sfd.FileName, taskManager.GetTasksAsFileText());
            }
            catch(Exception ex) 
            {
                MessageBox.Show("" + ex.ToString());
                return false;
            }            
        }
        this.filename = sfd.FileName;
        return true;
    }


    /// <summary>
    /// Read all tasks from a text file containing all the task objects' data.
    /// Checks if the file is valid. Then splits each line (task) into separate strings
    /// and converts them to Task object data. Then creates the task object.
    /// </summary>
    /// <returns>True if read was successful, otherwise false.</returns>
    public bool Read()
    {        
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Text Files (*.txt)|*.txt";
        if (ofd.ShowDialog() == true)
        {      
            if (!isFileCompatible(ofd))
                return false;

            taskManager.ClearTasks();

            int numOfLinesInFile = Convert.ToInt32(File.ReadLines(ofd.FileName).Skip(1).Take(1).First());

            // Iterate through each line (task), split each line into a string array with the different data
            // as separate elements. Call the add task method and pass in the data as the correct types.
            for (int i = 0; i < numOfLinesInFile; i++)
            {
                Task t = new Task();
                string line = File.ReadLines(ofd.FileName).Skip(2+i).Take(1).First();
                string[] taskStr = line.Split("_");
                
                // Skip the name of the day of the week (taskStr[0]), it can be generated using the other date-data.
                string dayDigits = taskStr[1];
                string monthDigits= taskStr[2];
                string yearDigits = taskStr[3];
                string hoursDigits = taskStr[4];
                string minutesDigits = taskStr[5];
                string priority = taskStr[6];
                string toDoText = taskStr[7];

                string time = hoursDigits + ":" + minutesDigits;

                //MessageBox.Show("Priority: " + priority + "year: " + yearDigits + "month: " + monthDigits + "day: " + dayDigits + "hour: " + hoursDigits + "minutes: " + minutesDigits);
                int priorityIndex = Convert.ToInt32(priority);
                DateTime dt = new DateTime(Convert.ToInt32(yearDigits), Convert.ToInt32(monthDigits), Convert.ToInt32(dayDigits));

                taskManager.AddOrChangeTask(dt, time, priorityIndex, toDoText, -1);
                this.filename = ofd.FileName;
            }            
        }
        return true;
    }

    /// <summary>
    /// Check if the file is compatible with the progra. It checks for a uniqu File ID (first line), and 
    /// checks whether the number of lines info is valid. The number of lines info must be on the second line and 
    /// not negative.
    /// </summary>
    /// <param name="ofd">The OpenFileDialog object to check.</param>
    /// <returns>True if file is compatible, otherwise false.</returns>
    private bool isFileCompatible(OpenFileDialog ofd)
    {
        if (!File.Exists(ofd.FileName))
        {
            MessageBox.Show("The file does not exist.");
            return false;
        }            

        string fileID = File.ReadLines(ofd.FileName).First();
        string numOfLinesInFile = File.ReadLines(ofd.FileName).Skip(1).Take(1).First();

        if (fileID != FILE_ID || !Int32.TryParse(numOfLinesInFile, out int numOfLines) || numOfLines < 0)
        {
            MessageBox.Show("Incompatible file");
            return false;
        }
        
        if (numOfLines < 1)
        {
            MessageBox.Show("There are no tasks in the file");
            return false;
        }
        return true;
    }    
}
