record Region
{
	public long Id { get; init; }
	public MultilingualString Name { get; init; }
	public Area Area { get; init; }
	public RegionLevel Level { get; private set; }
	public long? Population { get; private set; }
	public Geo Location { get; init; }
	private Region() { }
	public Region(MultilingualString name, Area area, Geo location,
		RegionLevel level)
	{
		this.Name = name;
		this.Area = area;
		this.Location = location;
		this.Level = level;
	}
	public void ChangePopulation(long value)
	{
		this.Population = value;
	}
	public void ChangeLevel(RegionLevel value)
	{
		this.Level = value;
	}
}