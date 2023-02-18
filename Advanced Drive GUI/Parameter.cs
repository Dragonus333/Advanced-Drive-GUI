namespace Advanced_Drive_GUI
{
    public class Parameter
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? type { get; set; }
        public int dimensions { get; set; }
        public string? readLevel { get; set; }
        public string? writeLevel { get; set; }
        public string? storageClass { get; set; }
        public string? units { get; set; }
        public int exponent { get; set; }
        public int decimalPlaces { get; set; }
        public string? enumeration { get; set; }
        public double upperLimit { get; set; }
        public double lowerLimit { get; set; }
        public bool isTuneable { get; set; }
        public bool isUserParam { get; set; }
        public string? description { get; set; }

        public float[]? values { get; set; }

        public TextBox[]? textBoxes { get; set; }
    }
}