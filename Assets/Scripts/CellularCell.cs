public struct CellularCell
{
	public enum State
	{
		Dead = 0,
		Alive = 1
	}

	public State	state;
	public float	value;
	public ColorHSV color;
	public Ruleset	rulset;
}