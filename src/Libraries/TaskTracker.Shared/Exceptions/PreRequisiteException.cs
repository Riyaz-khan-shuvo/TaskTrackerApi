namespace TaskTracker.Shared.Exceptions
{
    public class PreRequisiteException : Exception
    {
        public PreRequisiteException(dynamic name, string massage)
       : base($"Please set \"{name}\" brfore the action !")
        {
        }

        public PreRequisiteException(string massage)
       : base($"{massage}")
        {
        }
    }
}
