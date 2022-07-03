using TechnicalTest.Core.Interfaces;
using TechnicalTest.Core.Models;

namespace TechnicalTest.Core.Services
{
    public class ShapeService : IShapeService
    {
        public Shape ProcessTriangle(Grid grid, GridValue gridValue)
        {
            int x1, y1, x2, y2;

            if (gridValue.Column % 2 == 0)
            {
                // Calculate Right Triangle and return Shape with coordinates.
                x2 = (((gridValue.Column / 2) - 1) * grid.Size);
                y1 = (gridValue.GetNumericRow() - 1) * grid.Size;
                x1 = x2 + grid.Size;
                y2 = y1 + grid.Size;

                // Return Shape with coordinates
                return new Shape(new List<Coordinate>
                {
                    new(x2, y1),
                    new(x1, y1),
                    new(x1, y2)

                });
            }
            else
            {
                // Calculate Left Triangle and return Shape with coordinates
                x1 = ((gridValue.Column - 1) * grid.Size) / 2;
                x2 = x1 + grid.Size;
                y1 = gridValue.GetNumericRow() * grid.Size;
                y2 = (gridValue.GetNumericRow() - 1) * grid.Size;

                // Return Shape with coordinates
                return new Shape(new List<Coordinate>
                {
                    new(x1, y2),
                    new(x1, y1),
                    new(x2, y1)

                }); 
            }
        }

        public GridValue ProcessGridValueFromTriangularShape(Grid grid, Triangle triangle)
        {
            // Get vertices from Triangle
            var left = triangle.TopLeftVertex;
            var outer = triangle.OuterVertex;
            var right = triangle.BottomRightVertex;

            // Calculate grid row
            var row = outer.Y / grid.Size;
            if (left.Y == outer.Y)
            {
                row++;
            }
            
            // Calculate grid column
            var column = (outer.X / grid.Size) * 2;
            if (left.X == outer.X)
            {
                column++;
            }
            
            // Return GridValue
            return new GridValue(row, column);
        }
    }
}