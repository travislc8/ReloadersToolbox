namespace Model.Utils;
public static class Validate
{
    public static bool String(string? value, ref string status, int length)
    {
        if (value == null)
        {
            status = "Entry must have a value";
            return false;
        }
        bool check = false;
        if (value.Length > length)
        {
            check = false;
            status = string.Format("Entry must be less than {0} characters.", length);
        }
        else if (value.Length < 1)
        {
            check = false;
            status = string.Empty;
        }
        else
        {
            check = true;
            status = string.Empty;
        }
        return check;
    }

    ///<summary>
    /// Checks if the int can be converted to a string
    ///</summary>
    public static bool IntFromString(string? value)
    {
        bool check = false;
        if (value == null)
        {
            return false;
        }
        check = int.TryParse(value, out int num);
        if (check == true)
        {
            if (num <= int.MaxValue)
            {
                check = true;
            }
            else
            {
                check = false;
            }
        }

        return check;
    }

    ///<summary>
    /// checks if the int can be converted from a string and sets the status 
    /// message
    ///</summary>
    public static bool IntFromString(string? value, ref string status)
    {
        bool check = false;
        if (value == null)
        {
            status = "Value is null";
            return false;
        }
        check = int.TryParse(value, out int num);
        if (check == true)
        {
            if (num <= int.MaxValue)
            {
                check = true;
                status = string.Empty;
            }
            else
            {
                check = false;
                status = "Number is to large";
            }
        }
        else
        {
            status = "Entry is not a number";
        }

        return check;
    }
    //gets the number from the string
    public static bool IntFromString(string? value, out int num, ref string status)
    {
        bool check = false;
        if (value == null)
        {
            status = "Value is null";
            num = 0;
            return false;
        }
        check = int.TryParse(value, out num);
        if (check == true)
        {
            if (num <= int.MaxValue)
            {
                check = true;
                status = string.Empty;
            }
            else
            {
                check = false;
                status = "Number is to large";
            }
        }
        else
        {
            num = 0;
            status = "Entry is not a number";
        }

        return check;
    }
}
