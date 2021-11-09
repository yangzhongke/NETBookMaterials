public class Student
{
	public string Name { get; set; }
	public string? PhoneNumber { get; set; }
	public Student(string name)
	{
		this.Name = name;
	}
}