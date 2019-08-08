namespace Flabber.Eth
{
    public static class StateConvert
    {
        // enum StateType { Stopped, Success, Failure }
        // converts state int to string form
        public static string IntToString(int state)
        {
            if (state == 0) return "Stopped";
            if (state == 1) return "Success";
            if (state == 2) return "Failure";

            return "";
        }
    }
}
