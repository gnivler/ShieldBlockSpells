using System.Reflection;
using BepInEx;
using Harmony;
using UnityEngine;

namespace ShieldBlockSpells
{
    [BepInPlugin("com.gnivler.ShieldBlockSpells", "ShieldBlockSpells", "1.0")]
    public class ShieldBlockSpells : BaseUnityPlugin
    {
        public void Awake()
        {
            var harmony = HarmonyInstance.Create("com.gnivler.ShieldBlockSpells");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(Character), "ReceiveHit", MethodType.Normal)]
        [HarmonyPatch(new[]
        {
            typeof(Weapon),
            typeof(DamageList),
            typeof(Vector3),
            typeof(Vector3),
            typeof(float),
            typeof(float),
            typeof(Character),
            typeof(float),
            typeof(bool)
        })]
        public class ReceiveHitPatch
        {
            public static void Prefix(Character __instance, Character _dealerChar, ref DamageList _damage)
            {
                if (!__instance.ShieldEquipped || !__instance.Blocking) return;
                for (var i = 0; i < _damage.Count; i++)
                {
                    if (_damage[i].Type == DamageType.Types.Fire ||
                        _damage[i].Type == DamageType.Types.Frost ||
                        _damage[i].Type == DamageType.Types.Decay ||
                        _damage[i].Type == DamageType.Types.Electric ||
                        _damage[i].Type == DamageType.Types.Ethereal)
                    {
                        _damage[i].Damage = 0f;
                    }
                }
            }
        }
    }
}