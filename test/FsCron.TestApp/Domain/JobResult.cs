namespace FsCron.TestApp.Domain;

public class JobResult : Entity
{
    public JobResult()
    {
        Id = Guid.NewGuid();
        Added = DateTimeOffset.Now;
    }

    public JobType Type { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset FinishTime { get; set; }

    public override string ToString()
        => $"JobResult {{ Id = {Id} , StartTime = {StartTime}, FinishTime = {FinishTime} }}";
}