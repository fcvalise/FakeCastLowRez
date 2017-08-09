using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProceduralToolkit
{
	public class SpriteManager : MonoBehaviour
	{
		public enum SpriteState
		{
			Idle,
			Walk,
			Lay,
			Cast
		}

		[System.Serializable]
		public struct SpriteData
		{
			public SpriteState	state;
			public Texture2D	spriteSheet;
			public int			numberOfSprites;
			public bool			isLoop;
			[HideInInspector]
			public CellSprite	cellSprite;
		}

		public SpriteData[]		m_spritesData;

		private Dictionary<SpriteState, CellSprite> m_sprites;
		private SpriteState		m_current = SpriteState.Walk;
		private SpriteState		m_next = SpriteState.Walk;
		private Vector2Int		m_side = Vector2Int.down;

		void Start()
		{
			m_sprites = new Dictionary<SpriteState, CellSprite>();
			//for (int i = 0; i < m_spritesData.Length; i++)
			foreach (SpriteData data in m_spritesData)
			{
				CellSprite cellSprite = new CellSprite(data.spriteSheet, data.numberOfSprites, data.isLoop);
				m_sprites.Add(data.state, cellSprite);
			}
		}

		public void Simulate(CellularCell[, ] cells)
		{
			m_sprites[m_current].Simulate(cells);
			if (m_next != m_current)
				m_current = m_next;
			m_sprites[m_current].SetSide(m_side);
		}

		public void SetSide(Vector2Int side)
		{
			m_side = side;
		}

		public void PlayNext(SpriteState spriteState)
		{
			if (m_current != spriteState)
			{
				m_sprites[m_next].Play();
				m_next = spriteState;
			}
		}
	}
}