using Microsoft.AspNetCore.Mvc;

namespace InvantiTestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GridIndexController : ControllerBase
{
    private readonly GridIndexer _indexer;
    
    public GridIndexController(GridIndexer indexer)
    {
        _indexer = indexer;
    }

    // index/a1
    [HttpGet("index")]
    public ActionResult<object> GetCoordByIndex(string index)
    {
        var coords = _indexer.GetCoordinateFromIndex(index);
        
        return new
        {
            x = coords.row, 
            y = coords.col
        };;
    }
    
    // coord/row=1&col=1
    [HttpGet("coord")]
    public ActionResult<object> GetIndexFromCoordinate([FromQuery]int row, [FromQuery]int col)
    {
        var index = _indexer.GetIndexFromCoordinate(row, col);
        
        return new
        {
            index = index
        };;
    }
}