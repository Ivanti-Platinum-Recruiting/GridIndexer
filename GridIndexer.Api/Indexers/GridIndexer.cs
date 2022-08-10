namespace InvantiTestApi;

public class GridIndexer
{
    private readonly IDictionary<char, int> _charToIntDict = new Dictionary<char, int>();
    private readonly IDictionary<int, char> _intToCharDict = new Dictionary<int, char>();
    private IAlphabetProvider _provider;

    public GridIndexer(IAlphabetProvider provider)
    {
        _provider = provider;
        _setupDictionaries();
    }

    /// <summary>
    /// Converts a string index to a row and col numbers.
    /// </summary>
    /// <param name="index">range (A1,A2,A3,...F12)</param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public (int row, int col) GetCoordinateFromIndex(string index)
    {
        var clean = index.ToUpper().Replace(" ", "");

        try
        {
            (int row, string letters) = _splitLettersAndNumbers(clean);

            var last = letters[letters.Length - 1];
            var remainder = _charToIntDict[last];
            var power = letters.Length - 1;
            var total = 0.0;

            for (int i = 0; i < letters.Length - 1; i++) // less than only is because of remainder
            {
                var key = _charToIntDict[letters[i]] + 1; // get letter index

                total += key * Math.Pow(_charToIntDict.Count, power);
                power--;
            }

            total += remainder;

            return (row, (int)total);
        }
        catch (FormatException ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Converts a row and column (from 0 to N) to a string
    /// index.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public string GetIndexFromCoordinate(int row, int col)
    {
        try
        {
            _getLettersLength(col);
        }
        catch (Exception ex)
        {
            throw new FormatException(ex.Message);
            //return ex.Message; not sure - exception could be a breaking change if someone was using this
        }

        var rowOut = row + 1;
        var count = col + 1;
        var colStr = "";

        while (count > 0)
        {
            var mod = (count - 1) % _charToIntDict.Count;
            count -= mod;

            colStr = _intToCharDict[mod].ToString() + colStr;

            count = (int)(count / _charToIntDict.Count);
        }

        return $"{colStr}{rowOut}";
    }

    private void _setupDictionaries()
    { // sets up char/int dictionaries that are used to translate char/int values throughout the class

        var chars = _provider.GetAlphabetCharacters().ToList();

        for (int i = 0; i < chars.Count; i++)
        {
            _charToIntDict.Add(chars[i], i);
            _intToCharDict.Add(i, chars[i]);
        }
    }

    private (int row, string letters) _splitLettersAndNumbers(string idx) // separate input: "AA28" --> "AA" & "28"
    {
        var clean = idx.ToUpper().Replace(" ", "");
        var letters = "";
        var nums = "";

        for (int i = 0; i < clean.Length; i++)
        {
            if (char.IsDigit(clean[i]))
            {
                nums += clean[i].ToString();
                continue;
            }
            if (char.IsLetter(clean[i]))
            {
                letters += clean[i].ToString();
                continue;
            }
            throw new FormatException("unknown character in provided index");
        }

        if (!int.TryParse(nums.ToString(), out int row))
        {
            throw new FormatException("unable to parse numeric portion of input");
        }

        if (string.IsNullOrEmpty(letters))
        {
            throw new FormatException("no letters in input");
        }

        row--;
        return (row, letters);
    }

    private int _getLettersLength(int col) // number of letters in returned index
    {
        var total = 0.0;

        for (int i = 1; i <= _intToCharDict.Count; i++)
        {
            total += Math.Pow(_intToCharDict.Count, i);

            if (total <= col) continue;
            return i;
        }

        throw new ArgumentOutOfRangeException(nameof(col), "column cannot be represented by provided alphabet");
    }
}
