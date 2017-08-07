﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
	public interface ICellularObject
	{
		void Setup();
		void Simulate();
		void Add(ref CellularAutomaton automaton);
	}
}