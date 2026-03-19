using CalamityMod;
using CalamityMod.Enums;
using InfernumMode.Core.GlobalInstances.Players;
using Microsoft.Xna.Framework;
using Terraria;

namespace InfernumMode
{
    public static partial class Utilities
    {
        /// <summary>
        /// Applies screen shake to all nearby players with distance falloff.
        /// Use this instead of setting a single target's <c>GeneralScreenShakePower</c> in multiplayer.
        /// </summary>
        /// <param name="source">The world position of the shake source.</param>
        /// <param name="power">The maximum shake power at close range.</param>
        /// <param name="range">The maximum range at which the shake is felt. Defaults to 2600.</param>
        public static void ApplyScreenShakeToNearbyPlayers(Vector2 source, float power, float range = 2600f)
        {
            foreach (Player player in Main.ActivePlayers)
            {
                float dist = player.Distance(source);
                if (dist < range)
                {
                    float scaled = power * Utils.GetLerpValue(range, range * 0.5f, dist, true);
                    if (player.Calamity().GeneralScreenShakePower < scaled)
                        player.Calamity().GeneralScreenShakePower = scaled;
                }
            }
        }

        /// <summary>
        /// Applies Infernum camera shake to all nearby players with distance falloff.
        /// Use this instead of setting a single target's <c>Infernum_Camera().CurrentScreenShakePower</c> in multiplayer.
        /// </summary>
        /// <param name="source">The world position of the shake source.</param>
        /// <param name="power">The maximum shake power at close range.</param>
        /// <param name="range">The maximum range at which the shake is felt. Defaults to 2600.</param>
        public static void ApplyCameraShakeToNearbyPlayers(Vector2 source, float power, float range = 2600f)
        {
            foreach (Player player in Main.ActivePlayers)
            {
                float dist = player.Distance(source);
                if (dist < range)
                {
                    float scaled = power * Utils.GetLerpValue(range, range * 0.5f, dist, true);
                    if (player.Infernum_Camera().CurrentScreenShakePower < scaled)
                        player.Infernum_Camera().CurrentScreenShakePower = scaled;
                }
            }
        }

        /// <summary>
        /// Applies camera focus to all players within range.
        /// Use this instead of setting a single target/local player's camera focus in multiplayer.
        /// </summary>
        /// <param name="focusPos">The world position to focus the camera on.</param>
        /// <param name="interpolant">How strongly the camera snaps to the focus (0 = no snap, 1 = full snap).</param>
        /// <param name="source">The world position from which to measure range.</param>
        /// <param name="range">The maximum range for players to be affected. Defaults to 2700.</param>
        public static void ApplyCameraFocusToNearbyPlayers(Vector2 focusPos, float interpolant, Vector2 source, float range = 2700f)
        {
            foreach (Player player in Main.ActivePlayers)
            {
                if (player.WithinRange(source, range))
                {
                    player.Infernum_Camera().ScreenFocusPosition = focusPos;
                    player.Infernum_Camera().ScreenFocusInterpolant = interpolant;
                }
            }
        }


        public static bool HasShieldBash(this Player player)
        {
            var dashType = player.Calamity().UsedDash;
            if (dashType is null && player.dashType != 3)
                return false;

            return player.dashType == 3 || dashType.CollisionType == DashCollisionType.ShieldSlam;
        }

        public static bool HasDash(this Player player)
        {
            var dashType = player.Calamity().UsedDash;
            if (player.Calamity().blockAllDashes)
                return false;

            return dashType is not null || player.dashType >= 1;
        }

        public static void DoInfiniteFlightCheck(this Player player, Color textColor)
        {
            if (player.dead || !player.active)
                return;

            //if (!player.HasCooldown(InfiniteFlight.ID))
            //{
            //    CombatText.NewText(player.Hitbox, textColor, Language.GetTextValue("Mods.InfernumMode.Status.InfiniteFlight"), true);
            //    SoundEngine.PlaySound(SoundID.Item35 with { Volume = 4f, Pitch = 0.3f }, player.Center);
            //}

            //player.AddCooldown(InfiniteFlight.ID, CalamityUtils.SecondsToFrames(0.5f));
            player.wingTime = player.wingTimeMax;
            player.Calamity().infiniteFlight = true;
        }
    }
}
