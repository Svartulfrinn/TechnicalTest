using TechnicalTest.Core.Interfaces;
using TechnicalTest.Core.Models;

namespace TechnicalTest.Core.Factories
{
    public class ShapeFactory : IShapeFactory
    {
	    private readonly IShapeService _shapeService;

        public ShapeFactory(IShapeService shapeService)
        {
	        _shapeService = shapeService;
        }

        public Shape? CalculateCoordinates(ShapeEnum shapeEnum, Grid grid, GridValue gridValue)
        {
            switch (shapeEnum)
            {
                case ShapeEnum.Triangle:
                    // Call the function in the shape service to calculate and return coordinates.
                    var shape = _shapeService.ProcessTriangle(grid, gridValue);
                    return shape;
                default:
                    return null;
            }
        }

        public GridValue? CalculateGridValue(ShapeEnum shapeEnum, Grid grid, Shape shape)
        {
            switch (shapeEnum)
            {
                case ShapeEnum.Triangle:
                    // Verify shape has 3 coordinates
                    if (shape.Coordinates.Count != 3)
                        return null;
                    
                    // Create Triangle using Shape coordinates
                    var triangle = new Triangle(shape.Coordinates[0], shape.Coordinates[1], shape.Coordinates[2]);

                    // Call the function in the shape service to calculate and return grid value.
                    var gridValue = _shapeService.ProcessGridValueFromTriangularShape(grid, triangle);
                    var row = gridValue.GetNumericRow();
                    var column = gridValue.Column;
                    return new GridValue(row, column);
                default:
                    return null;
            }
        }
    }
}
