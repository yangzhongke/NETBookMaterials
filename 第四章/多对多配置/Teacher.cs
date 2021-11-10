class Teacher
{
	public long Id { get; set; }
	public string Name { get; set; }
	public List<Student> Students { get; set; } = new List<Student>();
}