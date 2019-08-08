namespace FV.Eth
{
    public static class StateConvert
    {
        // enum StateType { NotVerified, Changed, NotChanged, Invalid}
        // converts state int to string form
        public static string IntToString(int state)
        {
            if (state == 0) return "Not Verified";
            if (state == 1) return "Changed";
            if (state == 2) return "Not Changed";
            if (state == 3) return "Invalid";

            return "";
        }
    }
}
