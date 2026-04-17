namespace Scighost.PixivApi.Exceptions;

/// <summary>
/// Exception thrown when a Pixiv API request returns HTTP StatusCode=200 but the content is marked as an error.
/// </summary>
public class PixivException : Exception
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public PixivException(string? message, Exception? innerException = null) : base(message, innerException) { }

}
