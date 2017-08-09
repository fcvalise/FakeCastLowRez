using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
	[RequireComponent(typeof(SpriteManager))]
	public class Player : MonoBehaviour, ICellularObject
	{
		public enum PlayerState
		{
			None,
			CastSkill
		}

		private CellularCell[,]		m_cells;
		public Vector2Int			m_bounds;

		private SpriteManager		m_spriteManager;

		private Vector2Int			m_size;
		[HideInInspector]
		public Vector2Int			m_position ;

		private Vector2Int			m_movement = Vector2Int.down;
		private Vector2Int			m_lastMovement = Vector2Int.down;

		private bool				m_isSilence = false;
		private PlayerState			m_state = PlayerState.None;

		public void Setup()
		{
			m_spriteManager = GetComponent<SpriteManager>();
			//TODO : To get from SpriteManager
			m_size = new Vector2Int(10, 10);
			m_position = new Vector2Int(64 / 2, 64 / 2);
			m_cells = new CellularCell[m_size.x, m_size.y];
		}

		public void Simulate()
		{
			switch (m_state)
			{
			case PlayerState.None:
				UpdatePosition();
				break;
			case PlayerState.CastSkill:
				break;
			default:
				break;
			}

			UpdateSpriteManager();
			m_lastMovement = m_movement;
		}

		private void Update()
		{
			m_movement = Vector2Int.zero;

			if (Input.anyKeyDown)
				m_state = PlayerState.None;
			if (Input.GetKeyDown(KeyCode.Space))
				m_state = PlayerState.CastSkill;

			if (m_state == PlayerState.None)
			{
				if (Input.GetKey(KeyCode.RightArrow))
					m_movement += Vector2Int.right;
				if (Input.GetKey(KeyCode.LeftArrow))
					m_movement += Vector2Int.left;
				if (Input.GetKey(KeyCode.UpArrow))
					m_movement += Vector2Int.up;
				if (Input.GetKey(KeyCode.DownArrow))
					m_movement += Vector2Int.down;
			}
		}

		private void UpdatePosition()
		{
			if (Mathf.Abs(m_lastMovement.x) == 1 && Mathf.Abs(m_lastMovement.y) == 1)
				m_movement = Vector2Int.zero;

			m_position += m_movement;
			m_position.x = Mathf.Clamp(m_position.x, 1, m_bounds.x - m_size.x);
			m_position.y = Mathf.Clamp(m_position.y, 1, m_bounds.y - m_size.y);
		}

		private void UpdateSpriteManager()
		{
			m_spriteManager.SetSide(m_movement);

			switch (m_state)
			{
				case PlayerState.CastSkill:
				{
					if (m_spriteManager.GetState() != SpriteManager.SpriteState.CastIncant)
						m_spriteManager.PlayNext(SpriteManager.SpriteState.CastStart);
					if (m_spriteManager.isFinished())
						m_spriteManager.PlayNext(SpriteManager.SpriteState.CastIncant);
					break;
				}
				default:
				{
					if (m_movement != Vector2Int.zero || m_lastMovement != Vector2Int.zero)
						m_spriteManager.PlayNext(SpriteManager.SpriteState.Walk);
					else
						m_spriteManager.PlayNext(SpriteManager.SpriteState.Lay);
				}
				break;
			}

			m_spriteManager.Simulate(m_cells);
		}

		public void Add(CellularCell[,] automaton, CellularCell[,] staticGrid)
		{
			for (int x = 0; x < m_size.x; x++)
			{
				for (int y = 0; y < m_size.y; y++)
				{
					if (x + m_position.x < m_bounds.x && y + m_position.y < m_bounds.y)
					{
						staticGrid[x + m_position.x, y + m_position.y].value = m_cells[x, y].value;
						staticGrid[x + m_position.x, y + m_position.y].state = m_cells[x, y].state;
						staticGrid[x + m_position.x, y + m_position.y].color = m_cells[x, y].color;

						if (m_cells[x, y].state == CellularCell.State.Alive)
							automaton[x + m_position.x, y + m_position.y].state = m_cells[x, y].state;
					}
				}
			}
		}

		public void SetState(Player.PlayerState p_state)
		{
			m_state = p_state;
		}

		public bool CanCastSkill()
		{
			return m_state == PlayerState.None && !m_isSilence;
		}

		public void Silence()
		{
			m_isSilence = true;
		}

		public bool IsSilence()
		{
			return m_isSilence;
		}

		public GameObject GetTarget()
		{
			return null; //TODO get the other player
		}

		/*
		 * Debug
		 * 
		private void Fill()
		{
			for (int x = 0; x < m_size.x; x++)
			{
				for (int y = 0; y < m_size.y; y++)
				{
					m_cells[x, y].state = CellularCell.State.Alive;
					m_cells[x, y].value = 1f;
					m_cells[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);
				}
			}
		}
		*/
	}
}
