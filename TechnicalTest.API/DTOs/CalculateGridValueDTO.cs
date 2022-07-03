namespace TechnicalTest.API.DTOs
{
    public class CalculateGridValueDTO
    {
        public GridDTO Grid { get; set; }

        public int x1 { get; set; }
        public int y1 { get; set; }

        public int x2 { get; set; }
        public int y2 { get; set; }

        public int x3 { get; set; }
        public int y3 { get; set; }

        public int ShapeType { get; set; }
    }
}
