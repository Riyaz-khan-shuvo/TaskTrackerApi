namespace TaskTracker.Shared.Exceptions
{
    public class AppGeneralValueException : Exception
    {
        #region To do
        //DuplicateValueException is not working
        #endregion
        public AppGeneralValueException(dynamic name, dynamic secondaryName, string message)
        : base($"This {name} {secondaryName} is {message} cross the limit.")
        {
        }
    }
}
