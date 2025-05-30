using RPGCharacterAnims.Lookups;
using RPGCharacterAnims.Extensions;

namespace RPGCharacterAnims.Actions
{
    public class Relax : BaseActionHandler<bool?>
    {
        public override bool CanStartAction(RPGCharacterController controller)
        { return !IsActive(); }

        public override bool CanEndAction(RPGCharacterController controller)
        {  return IsActive(); }

        protected override void _StartAction(RPGCharacterController controller, bool? instant)
        {
            bool useInstant = instant.HasValue && instant.Value == true;

            RPGCharacterWeaponController weaponController = controller.GetComponent<RPGCharacterWeaponController>();

            if (weaponController == null) {
                EndAction(controller);
                return;
            }

			// Instant Switch.
			if (useInstant) {
				weaponController.InstantWeaponSwitch(Lookups.Weapon.Relax);
				return;
			}

			// If a weapon is equipped, we must sheathe it.
			if (controller.leftWeapon.HasEquippedWeapon() || controller.rightWeapon.HasEquippedWeapon()) {

				// Dual sheath.
				if (controller.leftWeapon.HasEquippedWeapon() && controller.rightWeapon.HasEquippedWeapon())
				{ weaponController.SheathWeapon(controller.rightWeapon, Lookups.Weapon.Relax, true); }

				// Right sheath.
				else if (controller.rightWeapon.HasEquippedWeapon())
				{ weaponController.SheathWeapon(controller.rightWeapon, Lookups.Weapon.Relax, false); }

				// Left sheath.
				else if (controller.leftWeapon.HasEquippedWeapon())
				{ weaponController.SheathWeapon(controller.leftWeapon, Lookups.Weapon.Relax, false); }

            }
			// Sheath Unarmed fists.
			else { weaponController.SheathWeapon(Lookups.Weapon.Unarmed, Lookups.Weapon.Relax, false); }

            weaponController.AddCallback(() => {
                controller.leftWeapon = Lookups.Weapon.Relax;
                controller.rightWeapon = Lookups.Weapon.Relax;
                weaponController.SyncWeaponVisibility();
            });
		}

        protected override void _EndAction(RPGCharacterController controller)
        {
            // If switching directly from the Relax state, switch to Unarmed.
            if (controller.leftWeapon == Lookups.Weapon.Relax || controller.rightWeapon == Lookups.Weapon.Relax)
			{ controller.StartAction(HandlerTypes.SwitchWeapon, new SwitchWeaponContext("Unsheath", "Dual", "Back", Lookups.Weapon.Unarmed, Lookups.Weapon.Unarmed)); }
        }
    }
}