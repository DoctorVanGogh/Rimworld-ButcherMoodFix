using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Harmony;
using Verse;

namespace DoctorVanGogh.RimworldButcherMoodFix {
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    class ButcherMoodFix : Mod {
        public ButcherMoodFix(ModContentPack content) : base(content) {
            HarmonyInstance harmony = HarmonyInstance.Create("DoctorVanGogh.ButcherMoodFix");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Log.Message("Installed Butcher mood fix");
        }
    }
}
