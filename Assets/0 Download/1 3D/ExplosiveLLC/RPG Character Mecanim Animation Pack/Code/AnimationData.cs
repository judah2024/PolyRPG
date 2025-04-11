using RPGCharacterAnims.Extensions;
using RPGCharacterAnims.Lookups;
using UnityEngine;

namespace RPGCharacterAnims
{
	/// <summary>
	/// Static class which contains hardcoded animation constants and helper functions.
	/// </summary>
	public class AnimationData
	{
		/// <summary>
		/// Converts left and right-hand weapon numbers into the legacy weapon number usable by the
		/// animator's "Weapon" parameter.
		/// </summary>
		/// <param name="leftWeapon">Left-hand weapon.</param>
		/// <param name="rightWeapon">Right-hand weapon.</param>
		public static AnimatorWeapon ConvertToAnimatorWeapon(Lookups.Weapon leftWeapon, Lookups.Weapon rightWeapon)
		{
			// 2-handed weapon.
			if (rightWeapon.Is2HandedWeapon()) { return ( AnimatorWeapon )rightWeapon; }

			// Unarmed or Relax.
			if (rightWeapon.HasNoWeapon() && leftWeapon.HasNoWeapon()) { return ( AnimatorWeapon )rightWeapon; }

			// Armed.
			return AnimatorWeapon.ARMED;
		}

		/// <summary>
		/// Returns the duration of an attack animation. Use side 0 (none) for two-handed weapons.
		/// </summary>
		/// <param name="attackSide">Side of the attack: 0- None, 1- Left, 2- Right, 3- Dual.</param>
		/// <param name="weapon">Weapon that's attacking.</param>
		/// <param name="attackNumber">Attack animation number.</param>
		/// <returns>Duration in seconds of attack animation.</returns>
		public static float AttackDuration(Side attackSide, Lookups.Weapon weapon, int attackNumber)
		{
			var duration = 1f;

			switch (attackSide) {
				case Side.None:						// Unspecified (2-Handed Weapons)
					switch (weapon) {
						case Lookups.Weapon.TwoHandSword:
							duration = 1.1f;
							break;
						case Lookups.Weapon.TwoHandSpear:
							duration = 1.1f;
							break;
						case Lookups.Weapon.TwoHandAxe:
							duration = 1.5f;
							break;
						case Lookups.Weapon.TwoHandBow:
							duration = 0.75f;
							break;
						case Lookups.Weapon.TwoHandCrossbow:
							duration = 0.75f;
							break;
						case Lookups.Weapon.TwoHandStaff:
							duration = 1f;
							break;
						case Lookups.Weapon.Rifle:
							duration = 1.1f;
							break;
						default:
							Debug.LogError("RPG Character: no weapon number " + weapon + " for Side 0");
							break;
					}
					break;

				case Side.Left:						// Left Side
					switch (weapon) {
						case Lookups.Weapon.Unarmed:
							duration = 0.75f;
							break;					// Unarmed  (1-3)
						case Lookups.Weapon.Shield:
							duration = 1.1f;
							break;					// Shield   (1-1)
						case Lookups.Weapon.LeftSword:
							duration = 0.75f;
							break;					// L Sword  (1-7)
						case Lookups.Weapon.LeftMace:
							duration = 0.75f;
							break;					// L Mace   (1-3)
						case Lookups.Weapon.LeftDagger:
							duration = 1f;
							break;					// L Dagger (1-3)
						case Lookups.Weapon.LeftItem:
							duration = 1f;
							break;					// L Item   (1-4)
						case Lookups.Weapon.LeftPistol:
							duration = 0.75f;
							break;					// L Pistol (1-3)
						default:
							Debug.LogError("RPG Character: no weapon number " + weapon + " for Side 1 (Left)");
							break;
					}
					break;
				case Side.Right:					// Right Side
					switch (weapon) {
						case Lookups.Weapon.Unarmed:
							duration = 0.75f;
							break;					// Unarmed  (4-6)
						case Lookups.Weapon.RightSword:
							duration = 0.75f;
							break;					// R Sword  (8-14)
						case Lookups.Weapon.RightMace:
							duration = 0.75f;
							break;					// R Mace   (4-6)
						case Lookups.Weapon.RightDagger:
							duration = 1f;
							break;					// R Dagger (4-6)
						case Lookups.Weapon.RightItem:
							duration = 1f;
							break;					// R Item   (5-8)
						case Lookups.Weapon.RightPistol:
							duration = 0.75f;
							break;					// R Pistol (4-6)
						case Lookups.Weapon.RightSpear:
							duration = 0.75f;
							break;					// R Spear  (1-7)
						default:
							Debug.LogError("RPG Character: no weapon number " + weapon + " for Side 2 (Right)");
							break;
					}
					break;
				case Side.Dual:
					duration = 0.75f;
					break;							// Dual Attacks (1-3)
			}

			return duration;
		}

		/// <summary>
		/// Returns the duration of the weapon sheath animation.
		/// </summary>
		/// <param name="attackSide">Side of the attack: 0- None, 1- Left, 2- Right, 3- Dual.</param>
		/// <param name="weaponNumber">Weapon being sheathed.</param>
		/// <returns>Duration in seconds of sheath animation.</returns>
		public static float SheathDuration(Side attackSide, Lookups.Weapon weapon)
		{
			var duration = 1f;

			if (weapon.HasNoWeapon()) { duration = 0f; }
			else if (weapon.Is2HandedWeapon()) { duration = 1.2f; }
			else if (attackSide == Side.Dual) { duration = 1f; }
			else { duration = 1.05f; }

			return duration;
		}

		/// <summary>
		/// Returns a random attack number usable as the animator's Action parameter.
		/// </summary>
		/// <param name="sideType">Side of the attack: 0- None, 1- Left, 2- Right, 3- Dual.</param>
		/// <param name="weapon">Weapon attacking.</param>
		/// <returns>Attack animation number.</returns>
		public static int RandomAttackNumber(Side sideType, Lookups.Weapon weapon)
		{
			switch (sideType) {
				case Side.None:
					switch (weapon) {
						case Lookups.Weapon.TwoHandSword:
							return ( int )AnimationVariations.TwoHandedSwordAttacks.TakeRandom();
						case Lookups.Weapon.TwoHandSpear:
							return ( int )AnimationVariations.TwoHandedSpearAttacks.TakeRandom();
						case Lookups.Weapon.TwoHandAxe:
							return ( int )AnimationVariations.TwoHandedAxeAttacks.TakeRandom();
						case Lookups.Weapon.TwoHandBow:
							return ( int )AnimationVariations.TwoHandedBowAttacks.TakeRandom();
						case Lookups.Weapon.TwoHandCrossbow:
							return ( int )AnimationVariations.TwoHandedCrossbowAttacks.TakeRandom();
						case Lookups.Weapon.TwoHandStaff:
							return ( int )AnimationVariations.TwoHandedStaffAttacks.TakeRandom();
						case Lookups.Weapon.Rifle:
							return ( int )AnimationVariations.ShootingAttacks.TakeRandom();
						default:
							Debug.LogError($"RPG Character: no weapon number {weapon} for Side 0");
							break;
					}
					break;

				case Side.Left:
					switch (weapon) {
						case Lookups.Weapon.Unarmed:
							return ( int )AnimationVariations.UnarmedLeftAttacks.TakeRandom();
						case Lookups.Weapon.Shield:
							return ( int )AnimationVariations.ShieldAttacks.TakeRandom();
						case Lookups.Weapon.LeftSword:
							return ( int )AnimationVariations.LeftSwordAttacks.TakeRandom();
						case Lookups.Weapon.LeftMace:
							return ( int )AnimationVariations.LeftMaceAttacks.TakeRandom();
						case Lookups.Weapon.LeftDagger:
							return ( int )AnimationVariations.LeftDaggerAttacks.TakeRandom();
						case Lookups.Weapon.LeftItem:
							return ( int )AnimationVariations.LeftItemAttacks.TakeRandom();
						case Lookups.Weapon.LeftPistol:
							return ( int )AnimationVariations.LeftPistolAttacks.TakeRandom();
						default:
							Debug.LogError($"RPG Character: no weapon number {weapon} for Side 1 (Left)");
							break;
					}
					break;
				case Side.Right:
					switch (weapon) {
						case Lookups.Weapon.Unarmed:
							return ( int )AnimationVariations.UnarmedRightAttacks.TakeRandom();
						case Lookups.Weapon.RightSword:
							return ( int )AnimationVariations.RightSwordAttacks.TakeRandom();
						case Lookups.Weapon.RightMace:
							return ( int )AnimationVariations.RightMaceAttacks.TakeRandom();
						case Lookups.Weapon.RightDagger:
							return ( int )AnimationVariations.RightDaggerAttacks.TakeRandom();
						case Lookups.Weapon.RightItem:
							return ( int )AnimationVariations.RightItemAttacks.TakeRandom();
						case Lookups.Weapon.RightPistol:
							return ( int )AnimationVariations.RightPistolAttacks.TakeRandom();
						case Lookups.Weapon.RightSpear:
							return ( int )AnimationVariations.RightSpearAttacks.TakeRandom();
						default:
							Debug.LogError($"RPG Character: no weapon number {weapon} for Side 2 (Right)");
							break;
					}
					break;
				case Side.Dual:
					return ( int )AnimationVariations.DualAttacks.TakeRandom();
			}

			return 1;
		}

		public static EmoteType RandomBow()
		{ return AnimationVariations.Bow.TakeRandom(); }

		public static Vector3 HitDirection(HitType hitType)
		{
			switch (hitType) {
				case HitType.Back1:
					return Vector3.forward;
				case HitType.Left1:
					return Vector3.right;
				case HitType.Right1:
					return Vector3.left;
				case HitType.Forward1:
				case HitType.Forward2:
				default:
					return Vector3.back;
			}
		}

		public static Vector3 HitDirection(KnockbackType hitType)
		{
			switch (hitType) {
				case KnockbackType.Knockback1:
				case KnockbackType.Knockback2:
				default:
					return Vector3.back;
			}
		}

		public static Vector3 HitDirection(BlockedHitType hitType)
		{ return Vector3.back; }

		public static Vector3 HitDirection(KnockdownType hitType)
		{ return Vector3.back; }
	}
}