namespace LINQ2_30集合扩展方法1
{
	class Employee
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
		public bool Gender { get; set; }
		public int Salary { get; set; }

		public override string ToString()
		{
			return $"Id={Id},Name={Name},Age={Age},Gender={Gender},Salary={Salary}";
		}
	}
}
