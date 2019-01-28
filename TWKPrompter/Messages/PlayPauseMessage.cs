namespace TWKPrompter.Messages
{
    public class PlayPauseMessage
    {
        public bool Playing;
        public PlayPauseMessage(bool _playing)
        {
            Playing = _playing;
        }
    }
}