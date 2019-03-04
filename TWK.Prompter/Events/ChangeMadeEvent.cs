namespace TWK.Prompter.Events
{
    public class ChangeMadeEvent
    {
        public ChangeMadeEnum ChangeType;
        public ChangeMadeEvent(ChangeMadeEnum type)
        {
            ChangeType = type;
        }


    }

    public enum ChangeMadeEnum
    {
        PlayPause,
        SizeUp,
        SizeDown,
        Faster,
        Slower,
        Stronger,
        Better,
        ScrollUp,
        ScrollDown
    }
}