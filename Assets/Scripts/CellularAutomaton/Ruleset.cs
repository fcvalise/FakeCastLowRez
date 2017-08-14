using System.Collections.Generic;
using UnityEngine;

public class Ruleset
{
	public enum Name
	{
		Life,
		Death,
		Anneal,
		Coagulations,
		Coral,
		Majority,
		Mazectric,
		WalledCities
	}

	public Name	_name;
	public int	_birth;
	public int	_survival;

	public Ruleset(Ruleset.Name p_name, string p_birth, string p_survival)
	{
		_name = p_name;
		_birth = StringToMask(p_birth);
		_survival = StringToMask(p_survival);
	}

	public Ruleset(Ruleset ruleset)
	{
		_name = ruleset._name;
		_birth = ruleset._birth;
		_survival = ruleset._survival;
	}

	private int StringToMask(string rule)
	{
		int mask = 0;

		for (int i = 0; i < rule.Length; i++)
			mask += (int)Mathf.Pow(2, (int)char.GetNumericValue(rule[i]));
		return mask;
	}
}
