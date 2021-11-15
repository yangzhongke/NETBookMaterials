record Geo
{
    public double Longitude { get; init; }
    public double Latitude { get; init; }
    public Geo(double longitude, double latitude)
    {
        if (longitude < -180 || longitude > 180)
        {
            throw new ArgumentException("longitude invalid");
        }
        if (latitude < -90 || latitude > 90)
        {
            throw new ArgumentException("longitude invalid");
        }
        this.Longitude = longitude;
        this.Latitude = latitude;
    }
}