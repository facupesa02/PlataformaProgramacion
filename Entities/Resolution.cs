namespace programmingPlatform;
public class Resolution
{
    private int id;
    private int participantId;
    private int ChallengeId;
    private int obtainedPoints;
    private DateOnly deliverDate;

    public int Id { get => id; set => id = value; }
    public int ParticipantId { get => participantId; set => participantId = value; }
    public int ObtainedPoints { get => obtainedPoints; set => obtainedPoints = value; }
    public DateOnly DeliverDate { get => deliverDate; set => deliverDate = value; }
    public int ChallengeId1 { get => ChallengeId; set => ChallengeId = value; }
}