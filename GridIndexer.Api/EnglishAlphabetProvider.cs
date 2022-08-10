namespace InvantiTestApi;

public class EnglishAlphabetProvider : IAlphabetProvider
{
    public IEnumerable<char> GetAlphabetCharacters()
    {
        var chars = new List<char>();
        for (var c = 'A'; c <= 'Z'; c++) 
        {
            chars.Add(c);
        }
        
        return chars;
    }
}
