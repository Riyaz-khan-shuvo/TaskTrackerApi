namespace TaskTracker.Shared.Exceptions
{
    public class DuplicateValueException : Exception
    {
        #region To do
        //DuplicateValueException is not working
        #endregion
        public DuplicateValueException(dynamic name, string massage)
        : base($"This \"{name}\" ({massage}) is already exist.")
        {
        }

        public DuplicateValueException(string massage)
        : base($"{massage}")
        {
        }
    }
}
