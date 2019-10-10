using System;
public class MathFunctions
{
	public static T Constrain<T>(T value, T min, T max) where T : IComparable<T>
	{
		if (value.CompareTo(min) < 0)
			return min;
		if (value.CompareTo(max) > 0)
			return max;

		return value;
	}
}