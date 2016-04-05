namespace Sunlight.Model
{
    public sealed class ZipCode
    {
        public string Zip {get;set;}

        public string CityState { get; set; }

        public override string ToString()
        {
            return $"{Zip} ({CityState})";
        }
    }
}
