namespace TaskTracker.Shared.Exceptions
{
    public class AppDuplicateValueException : Exception
    {
        #region To do
        //DuplicateValueException is not working
        #endregion
        public AppDuplicateValueException(dynamic name, string massage)
        : base($"This {name} {massage} is already exist.")
        {
        }
    }
}
