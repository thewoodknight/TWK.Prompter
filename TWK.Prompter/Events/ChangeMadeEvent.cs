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
}