namespace InvantiTestApi;

public class GridIndexer
{
    /// <summary>
    /// Converts a string index to a row and col numbers.
    /// </summary>
    /// <param name="index">range (A1,A2,A3,...F12)</param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public (int row, int col) GetCoordinateFromIndex(string index)
    {
        return (1, 1);
    }

    /// <summary>
    /// Converts a row and column (from 0 to N) to a string
    /// index.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public string GetIndexFromCoordinate(int row, int col)
    {
        return "";
    }
}
