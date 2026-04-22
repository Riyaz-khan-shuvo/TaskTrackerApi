namespace TaskTracker.Shared.Exceptions
{
    public class FileNotFoundException : Exception
    {
        public FileNotFoundException(string message) : base(message)
        {
        }

        //public FileNotFoundException(string name, object key)
        //    : base($"File Name \"{name}\" was not found.")
        //{
        //}
    }
}
