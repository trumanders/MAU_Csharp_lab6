using System.Linq;
using System.Windows;

namespace MAU_Charp_lab6;

public class FileManager
{
    private const string FILE_ID = "7904255556ToDoList";
    private TaskManager taskManager;
    private string numberOfTasks;
    private List<string> tasksAsFileText = new List<string>();  

    public FileManager(TaskManager taskManager)
    {
        this.taskManager = taskManager;
        this.numberOfTasks = taskManager.GetNumberOfTasks().ToString();        
    }


    public bool Save()
    {
        tasksAsFileText.Add("7904255556ToDoList");
        tasksAsFileText.Add(numberOfTasks);

        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "Text Files (*.txt)|*.txt";
        sfd.DefaultExt = "txt";
        if (sfd.ShowDialog() == true)
        {
            // Save 4 lines per task            
            File.WriteAllLines(sfd.FileName, GetTasksAsFileText());
        }
        return true;
    }


    public bool Read()
    {        
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Text Files (*.txt)|*.txt";
        if (ofd.ShowDialog() == true)
        {
            string firstLine = File.ReadLines(ofd.FileName).First();
            string secondLine = File.ReadLines(ofd.FileName).Skip(1).Take(1).First();
            if (firstLine != FILE_ID || !Int32.TryParse(secondLine, out int num))
            {
                MessageBox.Show("Incompatible file");
                return false;
            }
            
            int numOfLines = Convert.ToInt32(File.ReadLines(ofd.FileName).Skip(1).Take(1).First());
            if (numOfLines < 1)
            {
                MessageBox.Show("There are no tasks in the file");
                return false;
            }

            taskManager.ClearTasks();
            for (int i = 0; i < numOfLines; i++)
            {
                Task t = new Task();
                string line = File.ReadLines(ofd.FileName).Skip(2+i).Take(1).First();
                string[] taskStr = line.Split("_");

                //string dayLetters = taskStr[0];
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
            }            
        }
        return true;
    }


    private List<string> GetTasksAsFileText()
    {
        /* Iterate through all tasks and save each task as a string (line) of text. The data in the task is
           separated by '_' */
       
        for (int i = 0; i < taskManager.GetNumberOfTasks(); i++)
        {
            string taskString = "";
            Task t = taskManager.GetOneTask(i);

            // Add day string (Friday) 
            taskString += t.TaskDateAndTime.Date.ToString("dddd") + "_";

            // Add day of month (21) string
            string dayOneOrTwoDigits = t.TaskDateAndTime.Date.ToString("dd") + "_";
            if (dayOneOrTwoDigits[0] == '0')
                dayOneOrTwoDigits = dayOneOrTwoDigits.Remove(0,1);
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
}
