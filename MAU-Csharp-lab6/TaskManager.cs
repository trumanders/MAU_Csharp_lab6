
using System.Windows;

public class TaskManager
{
    private List<Task> allTasks;

    public TaskManager()
    {
        allTasks = new List<Task>();
    }


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
