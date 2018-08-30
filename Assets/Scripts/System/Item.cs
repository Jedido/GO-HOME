public class Item {
    public enum Type { Gold, Red, Blue, Yellow, Green, Count };

    public static string GetNameFromType(int type)
    {
        switch(type)
        {
            case (int)Type.Gold: return "Dollars";
            case (int)Type.Red: return "Red Essence";
            case (int)Type.Blue: return "Blue Essence";
            case (int)Type.Yellow: return "Yellow Essence";
            case (int)Type.Green: return "Green Essence";
        }
        return "";
    }
}
