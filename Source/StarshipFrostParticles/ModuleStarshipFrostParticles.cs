using UnityEngine;

namespace StarshipFrostParticles;

public class ModuleStarshipFrostParticles : PartModule
{
	public const string MODULENAME = "ModuleStarshipFrostParticles";

	private Animation FuelAnim;

	private Animation OxidAnim;

	private Part parentpart;

	private PartResource FuelTank;

	private PartResource OxidTank;

	[KSPField]
	public string ModuleID = "ModuleStarshipFrostParticles";

	[KSPField]
	public string FuelAnimName = "";

	[KSPField]
	public string OxidAnimName = "";

	[KSPField]
	public bool revClampFrostDirection = false;

	public void Start()
	{
		FuelAnim = ((PartModule)this).part.FindModelAnimator(FuelAnimName);
		OxidAnim = ((PartModule)this).part.FindModelAnimator(OxidAnimName);
		if ((Object)(object)FuelAnim == (Object)null)
		{
			Debug.LogWarning((object)string.Format("[{0}] Animation: {1} not found on part: {2}", "ModuleStarshipFrostParticles", FuelAnimName, ((PartModule)this).part));
		}
		else
		{
			FuelAnim[FuelAnimName].enabled = true;
			FuelAnim[FuelAnimName].normalizedSpeed = 0f;
			FuelAnim[FuelAnimName].normalizedTime = 0f;
			FuelAnim.Play(FuelAnimName);
		}
		if ((Object)(object)OxidAnim == (Object)null)
		{
			Debug.LogWarning((object)string.Format("[{0}] Animation: {1} not found on part: {2}", "ModuleStarshipFrostParticles", OxidAnimName, ((PartModule)this).part));
			return;
		}
		OxidAnim[OxidAnimName].enabled = true;
		OxidAnim[OxidAnimName].normalizedSpeed = 0f;
		OxidAnim[OxidAnimName].normalizedTime = 0f;
		OxidAnim.Play(OxidAnimName);
	}

	public override void OnLoad(ConfigNode node)
	{
		if ((Object)(object)FuelAnim != (Object)null)
		{
			FuelAnim[FuelAnimName].enabled = true;
			FuelAnim[FuelAnimName].normalizedSpeed = 1f;
			FuelAnim[FuelAnimName].normalizedTime = 0f;
			FuelAnim.Play(FuelAnimName);
		}
		if ((Object)(object)OxidAnim != (Object)null)
		{
			OxidAnim[OxidAnimName].enabled = true;
			OxidAnim[OxidAnimName].normalizedSpeed = 0f;
			OxidAnim[OxidAnimName].normalizedTime = 0f;
			OxidAnim.Play(OxidAnimName);
		}
	}

	public void FixedUpdate()
	{
		if ((Object)(object)parentpart == (Object)null && (Object)(object)((PartModule)this).part.parent != (Object)null)
		{
			parentpart = ((PartModule)this).part.parent;
			double num = 0.0;
			int num2 = -1;
			for (int i = 0; i < parentpart.Resources.Count; i++)
			{
				if ((double)parentpart.Resources[i].info.density * parentpart.Resources[i].maxAmount > num)
				{
					num = (double)parentpart.Resources[i].info.density * parentpart.Resources[i].maxAmount;
					num2 = i;
				}
			}
			if (num2 != -1)
			{
				OxidTank = parentpart.Resources[num2];
				Debug.Log((object)("[ModuleStarshipFrostParticles] Found oxidizer tank with resource: " + OxidTank.resourceName));
			}
			num = 0.0;
			num2 = -1;
			for (int j = 0; j < parentpart.Resources.Count; j++)
			{
				if ((double)parentpart.Resources[j].info.density * parentpart.Resources[j].maxAmount > num && parentpart.Resources[j].resourceName != OxidTank.resourceName)
				{
					num = (double)parentpart.Resources[j].info.density * parentpart.Resources[j].maxAmount;
					num2 = j;
				}
			}
			if (num2 != -1)
			{
				FuelTank = parentpart.Resources[num2];
				Debug.Log((object)("[ModuleStarshipFrostParticles] Found fuel tank with resource: " + FuelTank.resourceName));
			}
		}
		if ((Object)(object)FuelAnim != (Object)null && FuelTank != null)
		{
			float num3 = (float)(FuelTank.amount / FuelTank.maxAmount);
			if (revClampFrostDirection)
			{
				FuelAnim[FuelAnimName].normalizedTime = 1f - num3;
			}
			else
			{
				FuelAnim[FuelAnimName].normalizedTime = num3;
			}
		}
		if ((Object)(object)OxidAnim != (Object)null && OxidTank != null)
		{
			float num4 = (float)(OxidTank.amount / OxidTank.maxAmount);
			if (revClampFrostDirection)
			{
				OxidAnim[OxidAnimName].normalizedTime = 1f - num4;
			}
			else
			{
				OxidAnim[OxidAnimName].normalizedTime = num4;
			}
		}
	}
}
