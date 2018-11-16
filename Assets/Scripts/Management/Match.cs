public class Match
{
    public static int maxScore = 3;
    public static int player1Score = 0;
    public static int player2Score = 0;
    public static int lastWinnerID = -1;
    public static int cageID = 2;

    public static void Reset()
    {
        player1Score = 0;
        player2Score = 0;
        lastWinnerID = -1;
    }

    public static int Winner()
    {
        if (player1Score > maxScore - 1)
            return 1;
        if (player2Score > maxScore - 1)
            return 2;
        return -1;
    }
}
