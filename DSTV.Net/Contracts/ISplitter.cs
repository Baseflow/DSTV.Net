namespace DSTV.Net.Contracts;

public interface ISplitter
{
    /// <summary>
    ///     Splits the specified input.
    /// </summary>
    /// <param name="input">
    ///     The input to split.
    /// </param>
    /// <returns>
    ///     The split input.
    /// </returns>
    string[] Split(string input);
}