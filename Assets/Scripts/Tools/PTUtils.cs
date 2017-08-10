using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Useful utility methods
/// </summary>
public static class PTUtils
{
	/// <summary>
	/// Swaps values of <paramref name="left"/> and <paramref name="right"/>
	/// </summary>
	public static void Swap<T>(ref T left, ref T right)
	{
		T temp = left;
		left = right;
		right = temp;
	}
}