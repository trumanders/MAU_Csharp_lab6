namespace MAU_Csharp_lab6;
public class ViewModel
{
    public string[] Times { get; private set; }
    public string[] Priority { get; private set; }

    public ViewModel()
    {
        // The times in this array can be modified without needing to change anything else in the code. The conversion works for all hours and minutes.
        // Example: "00:15", "00:30", "00:45" etc...
        Times = new string[] {"00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00",
        "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00"};

        Priority = new string[] { "Very important", "Important", "Normal", "Less important", "Not important" };
    }
}
