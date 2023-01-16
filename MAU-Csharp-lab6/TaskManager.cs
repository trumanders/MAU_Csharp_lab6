public class TaskManager
{
    private List<Task> allTasks;
    private List<string> taskAsFileText;

    public List<string> TaskAsFileText { get { return taskAsFileText; } }
    public TaskManager()
    {
        allTasks = new List<Task>();
    }

    /// <summary>
    /// Delete all the tasks in the Task list.
    /// </summary>
    public void ClearTasks()
    {
        allTasks.Clear();
    }

    /// <summary>
    /// Get the number of registered tasks in the task list.
    /// </summary>
    /// <returns>Integer of the number of tasks in the allTasks list.</returns>
    public int GetNumberOfTasks()
    {
        return allTasks.Count;
    }


    /// <summary>
    /// Takes an index and returns the corresponding Task object in the allTasks list.
    /// </summary>
    /// <param name="index">The index of the task to return</param>
    /// <returns>One Task object.</returns>
    public Task GetOneTask(int index)
    {
        return allTasks[index];
    }


    /// <summary>
    /// Convert the Task list to a string list representing all the tasks.
    /// </summary>
    /// <returns>A list of strings representing all the tasks.</returns>
    public List<string> GetTasksAsFileText()
    {
        List<string> tasksAsFileText = new List<string>();
        tasksAsFileText.Add("7904255556ToDoList");
        tasksAsFileText.Add(allTasks.Count.ToString());

        /* Iterate through all tasks and save each task as a string (line) of text. The data in the task is
           separated by '_' */

        for (int i = 0; i < allTasks.Count; i++)
        {
            string taskString = "";
            Task t = allTasks[i];

            // Add day string (Friday) 
            taskString += t.TaskDateAndTime.Date.ToString("dddd") + "_";

            // Add day of month (21) string
            string dayOneOrTwoDigits = t.TaskDateAndTime.Date.ToString("dd") + "_";
            if (dayOneOrTwoDigits[0] == '0')
                dayOneOrTwoDigits = dayOneOrTwoDigits.Remove(0, 1);
            taskString += dayOneOrTwoDigits;

            // Add month digits string
            taskString += t.TaskDateAndTime.Date.ToString("MM") + "_";

            // Add year (2023) string
            taskString += t.TaskDateAndTime.Date.ToString("yyyy") + "_";

            // Add hour string (09)
            taskString += t.TaskDateAndTime.ToString("HH") + "_";

            // Add minute string (59)
            taskString += t.TaskDateAndTime.ToString("mm") + "_";

            // Add priority string
            taskString += ((int)t.Pt).ToString() + "_";  /* Replace the enum string "_" character with space */

            // Add todo string
            taskString += t.ToDoText + "_";
            tasksAsFileText.Add(taskString);
        }
        return tasksAsFileText;
    }

    /// <summary>
    /// Add a new Task to the Task list or change an existing one.
    /// </summary>
    /// <param name="dt">DateTime: The date to assign to the task object.</param>
    /// <param name="time">string: The time to assign to the task object.</param>
    /// <param name="priorityIndex">Enum: The priority to assign.</param>
    /// <param name="toDoText">string: The to-do-text to assign. </param>
    /// <param name="index">int: If index is negative, the method will add a new Task, otherwise change the Task at the passed in index.</param>
    public void AddOrChangeTask(DateTime dt, string time, int priorityIndex, string toDoText, int index)
    {
        Task task;
        bool isAdding = false;

        // Pass in negative number to indicate adding a task. If index is not negative, that task will be changed.
        if (index < 0)
        {
            isAdding = true;
            task = new Task();
        }
        else
            task = allTasks[index];

        // Set both date and time in the DateTime object
        task.SetTaskDateAndTime(dt, time);

        task.Pt = (PriorityType)priorityIndex;
        task.ToDoText = toDoText;

        if (isAdding)
            allTasks.Add(task);        
    }


    /// <summary>
    /// Delete the task in allTasks list at the passed in index
    /// </summary>
    /// <param name="index">Integer: the index of the task to delete.</param>
    public void DeleteTask(int index)
    {
        allTasks.RemoveAt(index);                
    }



    /// <summary>
    /// Takes an index and converts that task to a string
    /// </summary>
    /// <param name="index">The index of the task in allTasks to convert to a string.</param>
    /// <returns>A string representation of the task at the passed in index in allTasks.</returns>
    public string GetOneTaskAsString(int index)
    {
        Task t = allTasks[index];
        string taskString = "";

        // Date
        string dateString = t.TaskDateAndTime.ToString("dddd d MMMM yyyy");
        string timeString = t.TaskDateAndTime.ToString("HH:mm");
        string priorityString = t.Pt.ToString().Replace('_', ' ');       /* Remove the underscore in the enum value */
        string toDoString = t.ToDoText;

        // The longsest possible date string 30 characters inkl spaces
        taskString += $"{dateString,-30}{t.TaskDateAndTime.ToString("HH:mm")}";
        
        taskString += $"  {priorityString,-17}";
        taskString += t.ToDoText;
        return taskString;
    }


    /// <summary>
    /// Creates a string list and calls a method to convert each task to a string
    /// </summary>
    /// <returns>A list of strings representing all the tasks as strings.</returns>
    public List<string> GetAllTasksAsList()
    {
        // Create a list for all the tasks
        List<string> allTasksAsList = new List<string>();
        
        // Iterate though all tasks and call GetOneTaskAsString to add to the list.
        for (int i = 0; i < allTasks.Count; i++)
        {
            allTasksAsList.Add(GetOneTaskAsString(i));
        }
        return allTasksAsList;
    }


 }
