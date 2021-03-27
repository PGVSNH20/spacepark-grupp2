namespace SpacePark.Classes
{
    public class SwStarship
    {
        //public int StarshipID { get; set; }
        public string Model { get; set; }
        public double LengthInM { get; set; }

        public SwStarship(string model, double length)
        {
            Model = model;
            LengthInM = length;
        }
    }
}
