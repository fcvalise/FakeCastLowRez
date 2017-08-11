using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTmp : MonoBehaviour {

	private bool m_leftMovement = true;

	public void Simulate()
	{
		Vector2 position = transform.position;

		if (m_leftMovement)
		{
			position += Vector2.left;
			if (position.x < 10)
				m_leftMovement = false;
		}
		else
		{
			position += Vector2.right;
			if (position.x > Core._width - 10)
				m_leftMovement = true;
		}
		transform.position = position;
	}
}
