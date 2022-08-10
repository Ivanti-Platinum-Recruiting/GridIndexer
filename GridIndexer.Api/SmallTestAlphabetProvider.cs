namespace InvantiTestApi;

public class SmallTestAlphabetProvider : IAlphabetProvider
{
    public IEnumerable<char> GetAlphabetCharacters()
    {
        var chars = new List<char>(); 
        
        for (var c = 'A'; c <= 'C'; c++)
        {
            chars.Add(c);
        }
        
        return chars;
    }
}