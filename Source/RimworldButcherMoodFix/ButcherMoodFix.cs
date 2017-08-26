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

            Log.Message("Installed Rimworld fixes");
        }
    }
}
