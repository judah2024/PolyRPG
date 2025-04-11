using RPGCharacterAnims.Lookups;

namespace RPGCharacterAnims.Extensions
{
	public static class WeaponExtensions
	{
		/// <summary>
		/// Checks if the weapon is a right handed weapon.
		/// </summary>
		/// <param name="weapon">Weapon value to check.</param>
		/// <returns>True if right handed, false if not.</returns>
		public static bool IsRightHandedWeapon(this Lookups.Weapon weapon)
		{
			return weapon == Lookups.Weapon.RightSword || weapon == Lookups.Weapon.RightMace || weapon == Lookups.Weapon.RightDagger ||
				   weapon == Lookups.Weapon.RightItem || weapon == Lookups.Weapon.RightPistol || weapon == Lookups.Weapon.RightSpear;
		}

		/// <summary>
		/// Checks if the weapon is a left handed weapon.
		/// </summary>
		/// <param name="weapon">Weapon value to check.</param>
		/// <returns>True if left handed, false if not.</returns>
		public static bool IsLeftHandedWeapon(this Lookups.Weapon weapon)
		{
			return weapon == Lookups.Weapon.LeftSword || weapon == Lookups.Weapon.LeftMace || weapon == Lookups.Weapon.LeftDagger ||
				   weapon == Lookups.Weapon.LeftItem || weapon == Lookups.Weapon.LeftPistol || weapon == Lookups.Weapon.Shield;
		}

		/// <summary>
		/// Checks if the weapon is a 2 Handed weapon.
		/// </summary>
		/// <param name="weapon">Weapon value to check.</param>
		/// <returns>True if 2 Handed, false if not.</returns>
		public static bool Is2HandedWeapon(this Lookups.Weapon weapon)
		{
			return weapon == Lookups.Weapon.Rifle || weapon == Lookups.Weapon.TwoHandStaff || weapon == Lookups.Weapon.TwoHandCrossbow ||
				   weapon == Lookups.Weapon.TwoHandBow || weapon == Lookups.Weapon.TwoHandAxe || weapon == Lookups.Weapon.TwoHandSpear ||
				   weapon == Lookups.Weapon.TwoHandSword;
		}

		/// <summary>
		/// Checks if the weapon is aimable.
		/// </summary>
		/// <param name="weapon">Weapon value to check.</param>
		/// <returns>True if aimable, false if not.</returns>
		public static bool IsAimedWeapon(this Lookups.Weapon weapon)
		{ return weapon == Lookups.Weapon.Rifle || weapon == Lookups.Weapon.TwoHandBow || weapon == Lookups.Weapon.TwoHandCrossbow; }

		/// <summary>
		/// Checks if the weapon is equipped, i.e not Relaxing, or Unarmed.
		/// </summary>
		/// <param name="weapon">Weapon value to check.</param>
		/// <returns>True or false.</returns>
		public static bool HasEquippedWeapon(this Lookups.Weapon weapon)
		{ return weapon != Lookups.Weapon.Relax && weapon != Lookups.Weapon.Unarmed; }

		/// <summary>
		/// Checks if the weapon is empty, i.e Relaxing, or Unarmed.
		/// </summary>
		/// <param name="weapon">Weapon value to check.</param>
		/// <returns>True or false.</returns>
		public static bool HasNoWeapon(this Lookups.Weapon weapon)
		{ return weapon == Lookups.Weapon.Relax || weapon == Lookups.Weapon.Unarmed; }

		/// <summary>
		/// Checks if the weapon is a 1 Handed weapon.
		/// </summary>
		/// <param name="weapon">Weapon value to check.</param>
		/// <returns>True if 1 Handed, false if not.</returns>
		public static bool Is1HandedWeapon(this Lookups.Weapon weapon)
		{ return IsLeftHandedWeapon(weapon) || IsRightHandedWeapon(weapon); }

		/// <summary>
		/// Checks if the weapon is a castable weapon.
		/// </summary>
		/// <param name="weapon">Weapon value to check</param>
		/// <returns>True if castable, false if not</returns>
		public static bool IsCastableWeapon(this Lookups.Weapon weapon)
		{
			return weapon != Lookups.Weapon.Rifle && weapon != Lookups.Weapon.TwoHandAxe && weapon != Lookups.Weapon.TwoHandBow &&
				   weapon != Lookups.Weapon.TwoHandCrossbow && weapon != Lookups.Weapon.TwoHandSpear && weapon != Lookups.Weapon.TwoHandSword;
		}

		/// <summary>
		/// Returns true if the weapon number can use IKHands.
		/// </summary>
		/// <param name="weapon">Weapon to test.</param>
		public static bool IsIKWeapon(this Lookups.Weapon weapon)
		{
			return weapon == Lookups.Weapon.TwoHandSword
				   || weapon == Lookups.Weapon.TwoHandSpear
				   || weapon == Lookups.Weapon.TwoHandAxe
				   || weapon == Lookups.Weapon.TwoHandCrossbow
				   || weapon == Lookups.Weapon.Rifle;
		}

		/// <summary>
		/// This converts the Weapon into AnimatorWeapon, which is used in the Animator component to determine the
		/// proper state to set the character into, because all 1 Handed weapons use the ARMED state. 2 Handed weapons,
		/// Unarmed, and Relax map directly from Weapon to AnimatorWeapon.
		/// </summary>
		/// <param name="weapon">Weapon to convert.</param>
		/// <returns></returns>
		public static AnimatorWeapon ToAnimatorWeapon(this Lookups.Weapon weapon)
		{
			if (weapon == Lookups.Weapon.Unarmed || weapon == Lookups.Weapon.TwoHandAxe || weapon == Lookups.Weapon.TwoHandBow
				|| weapon == Lookups.Weapon.TwoHandCrossbow || weapon == Lookups.Weapon.TwoHandSpear
				|| weapon == Lookups.Weapon.TwoHandStaff  || weapon == Lookups.Weapon.TwoHandSword || weapon == Lookups.Weapon.Rifle)
			{ return ( AnimatorWeapon )weapon; }

			if (weapon == Lookups.Weapon.Relax) { return AnimatorWeapon.RELAX; }

			return AnimatorWeapon.ARMED;
		}

		/// <summary>
		/// Checks if the animator weapon is a 1 Handed weapon.
		/// </summary>
		/// <param name="weapon">Weapon value to check.</param>
		/// <returns>True if 1 Handed, false if not.</returns>
		public static bool Is1HandedAnimWeapon(this AnimatorWeapon weapon)
		{ return weapon == AnimatorWeapon.ARMED; }

		/// <summary>
		/// Checks if the animator weapon is a 2 Handed weapon.
		/// </summary>
		/// <param name="weapon">Weapon value to check.</param>
		/// <returns>True if 1 Handed, false if not.</returns>
		public static bool Is2HandedAnimWeapon(this AnimatorWeapon weapon)
		{
			return weapon == AnimatorWeapon.RIFLE || weapon == AnimatorWeapon.STAFF ||
				 weapon == AnimatorWeapon.TWOHANDAXE || weapon == AnimatorWeapon.TWOHANDBOW ||
				 weapon == AnimatorWeapon.TWOHANDSPEAR || weapon == AnimatorWeapon.TWOHANDSWORD ||
				 weapon == AnimatorWeapon.TWOHANDCROSSBOW;
		}

		/// <summary>
		/// Checks if the animator weapon is Unarmed or Relaxed.
		/// </summary>
		/// <param name="weapon">Weapon value to check.</param>
		/// <returns>True if 1 Handed, false if not.</returns>
		public static bool HasNoAnimWeapon(this AnimatorWeapon weapon)
		{ return weapon == AnimatorWeapon.UNARMED || weapon == AnimatorWeapon.RELAX; }
	}
}