using System.Collections.ObjectModel;
using System.Text;

namespace RangeApp.ViewModel;
public class Test2ViewModel
{
    public ObservableCollection<MyReading> Readings { get; private set; }


    public Test2ViewModel()
    {
        Readings = new ObservableCollection<MyReading>();

        Readings.Add(new MyReading() { Dia = 1, Sys = 2, Weight = 0.2f, Glocouse = 0.3f, TimeOfDay = DateTime.Now });
        Readings.Add(new MyReading() { Dia = 2, Sys = 2, Weight = 0.2f, Glocouse = 0.3f, TimeOfDay = DateTime.Now });
        Readings.Add(new MyReading() { Dia = 3, Sys = 2, Weight = 0.2f, Glocouse = 0.3f, TimeOfDay = DateTime.Now });
        Readings.Add(new MyReading() { Dia = 4, Sys = 2, Weight = 0.2f, Glocouse = 0.3f, TimeOfDay = DateTime.Now });
        Readings.Add(new MyReading() { Dia = 5, Sys = 2, Weight = 0.2f, Glocouse = 0.3f, TimeOfDay = DateTime.Now });
    }


}

public class MyReading
{
    public DateTime TimeOfDay { get; set; }
    public float Glocouse { get; set; }
    public int Dia { get; set; }
    public int Sys { get; set; }
    public float Weight { get; set; }

    public override string ToString()
    {
        return Glocouse.ToString();
    }

}