using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RulesetList : MonoBehaviour
{
	public static Dictionary<Ruleset.Name, Ruleset>	_rulesets;

	public static Ruleset Life { get { return new Ruleset(_rulesets[Ruleset.Name.Life]); } }
	public static Ruleset Death { get { return new Ruleset(_rulesets[Ruleset.Name.Death]); } }
	public static Ruleset Mazectric { get { return new Ruleset(_rulesets[Ruleset.Name.Mazectric]); } }
	public static Ruleset Coral { get { return new Ruleset(_rulesets[Ruleset.Name.Coral]); } }
	public static Ruleset Anneal { get { return new Ruleset(_rulesets[Ruleset.Name.Anneal]); } }
	public static Ruleset Majority { get { return new Ruleset(_rulesets[Ruleset.Name.Majority]); } }
	public static Ruleset Coagulations { get { return new Ruleset(_rulesets[Ruleset.Name.Coagulations]); } }
	public static Ruleset WalledCities { get { return new Ruleset(_rulesets[Ruleset.Name.WalledCities]); } }

	void Awake ()
	{
		_rulesets = new Dictionary<Ruleset.Name, Ruleset>();
		_rulesets.Add(Ruleset.Name.Life, new Ruleset(Ruleset.Name.Life, "3", "32"));
		_rulesets.Add(Ruleset.Name.Death , new Ruleset(Ruleset.Name.Majority, "", ""));
		_rulesets.Add(Ruleset.Name.Mazectric, new Ruleset(Ruleset.Name.Mazectric, "3", "1234"));
		_rulesets.Add(Ruleset.Name.Coral, new Ruleset(Ruleset.Name.Coral, "3", "45678"));
		_rulesets.Add(Ruleset.Name.Anneal, new Ruleset(Ruleset.Name.Anneal, "4678", "35678"));
		_rulesets.Add(Ruleset.Name.Majority, new Ruleset(Ruleset.Name.Majority, "5678", "45678"));
		_rulesets.Add(Ruleset.Name.Coagulations, new Ruleset(Ruleset.Name.Majority, "378", "235678"));
		_rulesets.Add(Ruleset.Name.WalledCities, new Ruleset(Ruleset.Name.WalledCities, "45678", "2345"));
	}

	public static Ruleset Get(Ruleset.Name name)
	{
		return new Ruleset(_rulesets[name]);
	}
}
