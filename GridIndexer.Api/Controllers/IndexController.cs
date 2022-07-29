using Microsoft.AspNetCore.Mvc;

namespace InvantiTestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IndexController : ControllerBase
{
    private readonly GridIndexer _indexer;
    
    public IndexController(GridIndexer indexer)
    {
        _indexer = indexer;
    }

    // index/a1
    [HttpGet("{index}")]
    public ActionResult<object> GetCoordByIndex(string index)
    {
        var coords = _indexer.GetCoordinateFromIndex(index);
        
        return new
        {
            x = coords.row, 
            y = coords.col
        };;
    }
}