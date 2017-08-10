using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
	public class Bullet : MonoBehaviour, ICellularObject
	{
		public int					_speed;
		public GameObject			_target;
		public Vector2				_targetPosition;
		public Texture2D			_spriteSheet;

		public Vector2				_bounds;

		private CellSprite			_sprite;
		private CellularCell[,]		_cells;
		private SpriteManager		_cellSprite;
		private Vector2				_size;
		private Vector2				_position;

		public void Setup()
		{
			_position = new Vector2(10, 10);
			_sprite = new CellSprite(_spriteSheet, 3, false);
			//TODO : To get from SpriteManager
			_size = new Vector2(10, 10);
			_cells = new CellularCell[(int)_size.x, (int)_size.y];
		}

		public void Simulate()
		{
			Vector2 direction = (_targetPosition - _position).normalized;
			_position += direction * _speed;
			_sprite.Simulate(_cells);
		}

		public void Add(CellularCell[,] p_automaton, CellularCell[,] p_staticGrid)
		{
			for (int x = 0; x < _size.x; x++)
			{
				for (int y = 0; y < _size.y; y++)
				{
					if (x + _position.x < _bounds.x && y + _position.y < _bounds.y)
					{
						p_staticGrid[x + (int)_position.x, y + (int)_position.y].value = _cells[x, y].value;
						p_staticGrid[x + (int)_position.x, y + (int)_position.y].state = _cells[x, y].state;
						p_staticGrid[x + (int)_position.x, y + (int)_position.y].color = _cells[x, y].color;
					}
				}
			}
		}
	}
}