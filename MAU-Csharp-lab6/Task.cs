public class Task
{    
    private DateTime taskDateAndTime;   /* Contains both date and time */
    private string time;                /* The chosen time as a string */
    private PriorityType pt;  
    private string toDoText;
    public string ToDoText { get { return toDoText; } set { toDoText = value; } }

    public DateTime TaskDateAndTime { get { return taskDateAndTime; } }

    public string Time { get { return time; } }

    public PriorityType Pt { get { return pt; } set { pt = value; } }


    /// <summary>
    /// Takes the date part from the passed in date, and converts the time-string
    /// to hours and minutes as double to assign to the DateTime object.
    /// </summary>
    /// <param name="date">The date that comes from the DatePicker in the UI.</param>
    /// <param name="time">The time as a string, chosen in the UI's combobox</param>
    public void SetTaskDateAndTime(DateTime date, string time)
    {
        
        // First set the time sting
        this.time = time;
        
        // Set the date part
        this.taskDateAndTime = new DateTime(date.Year, date.Month, date.Day);

        // Set the time (clock) part (no need for tryparse since the time-string is constant (from enum))
        int hours = int.Parse(time.Substring(0, 2));
        int minutes = int.Parse(time.Substring(3, 2));
        TimeSpan timeSpan = new TimeSpan(hours, minutes, 0);

        this.taskDateAndTime = this.taskDateAndTime.Date + timeSpan;       
    }    
}
