using Microsoft.AspNetCore.Mvc;
using TechnicalTest.API.DTOs;
using TechnicalTest.Core;
using TechnicalTest.Core.Interfaces;
using TechnicalTest.Core.Models;

namespace TechnicalTest.API.Controllers
{
    /// <summary>
    /// Shape Controller which is responsible for calculating coordinates and grid value.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ShapeController : ControllerBase
    {
        private readonly IShapeFactory _shapeFactory;

        /// <summary>
        /// Constructor of the Shape Controller.
        /// </summary>
        /// <param name="shapeFactory"></param>
        public ShapeController(IShapeFactory shapeFactory)
        {
            _shapeFactory = shapeFactory;
        }

        /// <summary>
        /// Calculates the Coordinates of a shape given the Grid Value.
        /// </summary>
        /// <param name="calculateCoordinatesRequest"></param>   
        /// <returns>A Coordinates response with a list of coordinates.</returns>
        /// <response code="200">Returns the Coordinates response model.</response>
        /// <response code="400">If an error occurred while calculating the Coordinates.</response>   
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Shape))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("CalculateCoordinates")]
        [HttpPost]
        public IActionResult CalculateCoordinates([FromBody]CalculateCoordinatesDTO calculateCoordinatesRequest)
        {
            // Retrieve data from request
            var grid = new Grid(calculateCoordinatesRequest.Grid.Size);
            var gridValue = new GridValue(calculateCoordinatesRequest.GridValue);
            var shapeEnum = (ShapeEnum)calculateCoordinatesRequest.ShapeType;

            // Return BadRequest if ShapeEnum is not Triangle (1)
            if (shapeEnum != ShapeEnum.Triangle)
            {
                return BadRequest("Error: ShapeEnum is only configured for Triangle!");
            }

            // Call the function in the shape factory to calculate coordinates.
            Shape? shape = _shapeFactory.CalculateCoordinates(shapeEnum, grid, gridValue);
            
            // Return BadRequest if result is null
            if (shape == null)
            {
                return BadRequest("Shape is null");
            }

            // Create ResponseModel with Coordinates and return as OK with responseModel.
            CalculateCoordinatesResponseDTO calculateCoordinatesResponse = new CalculateCoordinatesResponseDTO();
            List<CalculateCoordinatesResponseDTO.Coordinate> coords = new List<CalculateCoordinatesResponseDTO.Coordinate>();

            foreach (Coordinate c in shape.Coordinates)
            {
                CalculateCoordinatesResponseDTO.Coordinate coord = new CalculateCoordinatesResponseDTO.Coordinate(c.X, c.Y);
                coords.Add(coord);
            }

            calculateCoordinatesResponse.Coordinates = coords;
            return Ok(calculateCoordinatesResponse);
        }

        /// <summary>
        /// Calculates the Grid Value of a shape given the Coordinates.
        /// </summary>
        /// <remarks>
        /// A Triangle Shape must have 3 vertices, in this order: Top Left Vertex, Outer Vertex, Bottom Right Vertex.
        /// </remarks>
        /// <param name="gridValueRequest"></param>   
        /// <returns>A Grid Value response with a Row and a Column.</returns>
        /// <response code="200">Returns the Grid Value response model.</response>
        /// <response code="400">If an error occurred while calculating the Grid Value.</response>   
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GridValue))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("CalculateGridValue")]
        [HttpPost]
        public IActionResult CalculateGridValue([FromBody]CalculateGridValueDTO gridValueRequest)
        {
            // Retrieve data from request
            var grid = new Grid(gridValueRequest.Grid.Size);
            var shapeEnum = (ShapeEnum)gridValueRequest.ShapeType;
            var x1 = gridValueRequest.x1;
            var x2 = gridValueRequest.x2;
            var x3 = gridValueRequest.x3;
            var y1 = gridValueRequest.y1;
            var y2 = gridValueRequest.y2;
            var y3 = gridValueRequest.y3;

            // Return BadRequest if ShapeEnum is not Triangle (1)
            if (shapeEnum != ShapeEnum.Triangle)
            {
                return BadRequest("Error: ShapeEnum is only configured for Triangle!");
            }

            // Create new Shape with coordinates based on the new parameters from the DTO.
            Shape shape = new Shape(new List<Coordinate>
            {
                new Coordinate(x1, y1),
                new Coordinate(x2, y2),
                new Coordinate(x3, y3)
            });

            // Call the function in the shape factory to calculate grid value.
            var gridValue = _shapeFactory.CalculateGridValue(shapeEnum, grid, shape);

            // Return BadRequest if result is null
            if (gridValue == null)
            {
                return BadRequest("GridValue is null");
            }

            // TODO: Generate a ResponseModel based on the result and return it in Ok();
            CalculateGridValueResponseDTO gridValueResponse = new CalculateGridValueResponseDTO(gridValue.Row, gridValue.Column);
            return Ok(gridValueResponse);
        }
    }
}
