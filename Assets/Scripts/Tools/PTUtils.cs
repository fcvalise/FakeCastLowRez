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

	public static void CopyUp<T>(T[,] p_target, T[,] p_source, Vector2Int p_size)
	{
		for (int y = 0; y < p_size.y; y++)
		{
			for (int x = 0 ; x < p_size.x; x++)
			{
				p_target[x, y] = p_source[x, y];
			}
		}
	}

	public static void CopyDown<T>(T[,] p_target, T[,] p_source, Vector2Int p_size)
	{
		for (int y = 0; y < p_size.y; y++)
		{
			for (int x = 0 ; x < p_size.x; x++)
			{
				p_target[x, y] = p_source[x, p_size.y - 1 - y];
			}
		}
	}

	public static void CopyLeft<T>(T[,] p_target, T[,] p_source, Vector2Int p_size)
	{
		//TODO : Only working with square textures
		for (int y = 0; y < p_size.y; y++)
		{
			for (int x = 0 ; x < p_size.x; x++)
			{
				p_target[y, x] = p_source[x, p_size.y - 1 - y];
			}
		}
	}

	public static void CopyRight<T>(T[,] p_target, T[,] p_source, Vector2Int p_size)
	{
		//TODO : Only working with square textures
		for (int y = 0; y < p_size.y; y++)
		{
			for (int x = 0 ; x < p_size.x; x++)
			{
				p_target[y, x] = p_source[x, y];
			}
		}
	}
}