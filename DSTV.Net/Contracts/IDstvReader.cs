namespace DSTV.Net.Contracts;

/// <summary>
///     Interface for reading DSTV (.nc) files
/// </summary>
public interface IDstvReader
{
    /// <summary>
    ///     Parses the data in <paramref name="dstvData" /> into the structure as defined in <seealso cref="IDstv" />
    /// </summary>
    /// <param name="dstvData">A string containing all the data for the DSTV</param>
    /// <returns>A parsed structure of the DSTV file</returns>
    Task<IDstv> ParseAsync(string dstvData);

    /// <summary>
    ///     Parses the data in <paramref name="dstvData" /> into the structure as defined in <seealso cref="IDstv" />
    /// </summary>
    /// <param name="dstvData">A string containing all the data for the DSTV</param>
    /// <param name="cancellationToken">A token that can cancel parsing.</param>
    /// <returns>A parsed structure of the DSTV file</returns>
    Task<IDstv> ParseAsync(string dstvData, CancellationToken cancellationToken);

    /// <summary>
    ///     Parses the data in <paramref name="reader" /> into the structure as defined in <seealso cref="IDstv" />
    /// </summary>
    /// <param name="reader">The instance of a text reader that holds the DSTV data</param>
    /// <returns>A parsed structure of the DSTV file</returns>
    Task<IDstv> ParseAsync(TextReader reader);

    /// <summary>
    ///     Parses the data in <paramref name="reader" /> into the structure as defined in <seealso cref="IDstv" />
    /// </summary>
    /// <param name="reader">The instance of a text reader that holds the DSTV data</param>
    /// <param name="cancellationToken">A token that can cancel parsing.</param>
    /// <returns>A parsed structure of the DSTV file</returns>
    Task<IDstv> ParseAsync(TextReader reader, CancellationToken cancellationToken);
}