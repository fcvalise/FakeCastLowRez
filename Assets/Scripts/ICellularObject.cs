using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICellularObject
{
	void Setup();
	void Simulate();
	void Add(CellularCell[,] p_automaton, CellularCell[,] p_staticGrid);
}