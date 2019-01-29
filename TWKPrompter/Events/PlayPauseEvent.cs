namespace TWKPrompter.Events
{
    public class PlayPauseEvent
    {
        public bool Playing;
        public PlayPauseEvent(bool _playing)
        {
            Playing = _playing;
        }
    }
}