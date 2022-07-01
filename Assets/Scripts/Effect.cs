abstract public class Effect
{
    public float Duration;

    public bool Alive { get { return Duration > 0; } }

    public Effect(float duration)
    {
        Duration = duration;
    }

    public class Stun : Root
    {
        public Stun(float duration) : base(duration) { }
    }

    public class SpeedMuliplier : Effect
    {
        public float Amount;
        public SpeedMuliplier(float duration, float amount) : base(duration)
        {
            Amount = amount;
        }
    }

    public class Root : Effect
    {
        public Root(float duration) : base(duration) { }
    }

    public class Invincibility : Effect
    {
        public Invincibility(float duration) : base(duration) { }
    }
}