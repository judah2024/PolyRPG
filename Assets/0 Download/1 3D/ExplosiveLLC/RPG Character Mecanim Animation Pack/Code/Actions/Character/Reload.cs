using RPGCharacterAnims.Lookups;

namespace RPGCharacterAnims.Actions
{
    public class Reload : InstantActionHandler<EmptyContext>
    {
        public override bool CanStartAction(RPGCharacterController controller)
        {
            return !controller.isRelaxed &&
                   (controller.rightWeapon == Lookups.Weapon.TwoHandCrossbow ||
                    controller.rightWeapon == Lookups.Weapon.Rifle ||
                    controller.rightWeapon == Lookups.Weapon.RightPistol ||
                    controller.leftWeapon == Lookups.Weapon.LeftPistol);
        }

        protected override void _StartAction(RPGCharacterController controller, EmptyContext context)
        { controller.Reload(); }
    }
}