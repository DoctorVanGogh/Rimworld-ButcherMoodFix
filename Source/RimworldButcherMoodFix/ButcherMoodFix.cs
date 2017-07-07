using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DoctorVanGogh.RimworldFixes.Patches;
using Harmony;
using Verse;

namespace DoctorVanGogh.RimworldFixes {
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    class ButcherMoodFix : Mod {
        public ButcherMoodFix(ModContentPack content) : base(content) {
            HarmonyInstance harmony = HarmonyInstance.Create("DoctorVanGogh.RimworldFixes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // private nested anon type can't have it's patches declared by attributes - need to look up type at runtime
            Type fu = typeof(RimWorld.FoodUtility);
            Type at = fu.GetNestedType("<BestFoodSourceOnMap>c__AnonStorey259", BindingFlags.NonPublic);        
            MethodInfo m = at.GetMethod("<>m__1", BindingFlags.NonPublic | BindingFlags.Instance);

            harmony.Patch(m,null, null, new HarmonyMethod(typeof(FoodUtility_BestFoodSourceOnMap_AnonStorey259), nameof(FoodUtility_BestFoodSourceOnMap_AnonStorey259.Transpiler)));

            Log.Message("Installed Rimworld fixes");
        }
    }
}
