using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACellObject : MonoBehaviour
{
	public abstract int GetZIndex();
	public abstract void Simulate();
	public abstract void Add(Cell[,] p_automaton, Cell[,] p_staticGrid);
}