namespace System
{
    public static class RandomExtensions
    {
        /// <summary>
        ///  Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns></returns>
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            if (minValue >= maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), "minValue cannot be bigger than maxValue");
            }
            //https://stackoverflow.com/questions/65900931/c-sharp-random-number-between-double-minvalue-and-double-maxvalue
            double x = random.NextDouble();
            return x * maxValue + (1 - x) * minValue;
        }
    }
}
