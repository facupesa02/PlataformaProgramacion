namespace programmingPlatform;
public class Challenge
{
    private int id;
    private string title;
    private string difficulty;
    private int highScore;
    private bool status;

    public int Id { get => id; set => id = value; }
    public string Title { get => title; set => title = value; }
    public string Difficulty { get => difficulty; set => difficulty = value; }
    public int HighScore { get => highScore; set => highScore = value; }
    public bool Status { get => status; set => status = value; }
}