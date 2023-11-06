public interface ISlowable
{
    bool slowed { get; set; }
    public void ApplySlow(float slowModifier);
    public void RemoveSlow(float slowModifier);
}