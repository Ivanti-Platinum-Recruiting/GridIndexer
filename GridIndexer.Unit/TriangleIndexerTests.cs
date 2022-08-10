using InvantiTestApi;
using InvantiTestApi.Controllers;

namespace IvantiTest.Unit;

[TestFixture]
public class TriangleIndexerTests
{
    private IAlphabetProvider _provider;

    [SetUp]
    public void Setup()
    {
        _provider = new EnglishAlphabetProvider();
    }

    private static object[] _fullAlphabetTestCasesSearchByIdx =
    {
        new object[] { "A1", (0, 0) },
        new object[] { "AA7", (6, 26) },
        new object[] { "XFD6", (5, 16383) },
        new object[] { "XES4", (3, 16372) },
        new object[] { "BDF2", (1, 1461) },
        new object[] { "zt88", (87, 695) },
        new object[] { "LKI22", (21, 8406) },
        new object[] { "AGT1024", (1023, 877) },
        new object[] { "AAAA24", (23, 18278) }
    };

    [Test]
    [TestCaseSource(nameof(_fullAlphabetTestCasesSearchByIdx))]
    public void FullAlphabetValidInputGetCoords(string input, (int, int) result)
    {
        var indexer = new GridIndexer(_provider);

        var idx = indexer.GetCoordinateFromIndex(input);

        Assert.That(result, Is.EqualTo(idx));
    }

    private static object[] _shortAlphabetTestCasesSearchByIdx =
    {
        new object[] { "A1", (0, 0) },
        new object[] { "CA7", (6, 9) },
        new object[] { "AAC6", (5, 14) },
        new object[] { "BCA4", (3, 27) },
        new object[] { "CAC2", (1, 32) },
        new object[] { "CCC34", (33, 38) }
    };

    [Test]
    [TestCaseSource(nameof(_shortAlphabetTestCasesSearchByIdx))]
    public void ShortAlphabetValidInputGetCoords(string input, (int, int) result)
    {
        var shortProvider = new SmallTestAlphabetProvider();
        var indexer = new GridIndexer(shortProvider);

        var idx = indexer.GetCoordinateFromIndex(input);

        Assert.That(result, Is.EqualTo(idx));
    }

    [Test]
    [TestCase("@")]
    [TestCase("?")]
    [TestCase("#")]
    [TestCase("'")]
    [TestCase("[")]
    [TestCase(" ")]
    [TestCase("")]
    [TestCase("1234")]
    [TestCase("abcde")]
    [TestCase("ي")]
    public void BadInput_ExpectingFormattingError(string badInput)
    {
        var indexer = new GridIndexer(_provider);
        Assert.Throws<FormatException>(() => indexer.GetCoordinateFromIndex(badInput));

    }

    [Test]
    [TestCase(0, 0, "A1")]
    [TestCase(6, 26, "AA7")]
    [TestCase(5, 16383, "XFD6")]
    [TestCase(3, 16372, "XES4")]
    [TestCase(1, 1461, "BDF2")]
    [TestCase(87, 695, "ZT88")]
    [TestCase(21, 8406, "LKI22")]
    [TestCase(1023, 877, "AGT1024")]
    [TestCase(103, int.MaxValue - 1, "FXSHRXW104")]
    public void FullAlphabetValidInputGetIndex(int row, int col, string result)
    {
        var indexer = new GridIndexer(_provider);

        var res = indexer.GetIndexFromCoordinate(row, col);

        Assert.That(res, Is.EqualTo(result));
    }

    [Test]
    [TestCase(0, 0, "A1")]
    [TestCase(6, 9, "CA7")]
    [TestCase(5, 14, "AAC6")]
    [TestCase(3, 27, "BCA4")]
    [TestCase(1, 32, "CAC2")]
    [TestCase(33, 38, "CCC34")]
    public void ShortAlphabetValidInputGetIndex(int row, int col, string result)
    {
        var shortProvider = new SmallTestAlphabetProvider();
        var indexer = new GridIndexer(shortProvider);

        var res = indexer.GetIndexFromCoordinate(row, col);

        Assert.That(res, Is.EqualTo(result));
    }

    [TestCase(33, 88, "CCC34")] // max cols = 39 where len(alphabet)= 3
    public void ShortAlphabetColumnTooBig_ExpectingFormatError(int row, int col, string result)
    {
        var shortProvider = new SmallTestAlphabetProvider();
        var indexer = new GridIndexer(shortProvider);

        Assert.Throws<FormatException>(() => indexer.GetIndexFromCoordinate(row, col));
    }
}