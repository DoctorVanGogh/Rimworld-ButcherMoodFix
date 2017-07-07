using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using RimWorld;
using Verse;

namespace DoctorVanGogh.RimworldFixes.Patches {

    [HarmonyPatch(typeof(Corpse), nameof(Corpse.ButcherProducts))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    class Corpse_ButcherProducts {

        private static readonly MethodInfo miGetProducts;

        static Corpse_ButcherProducts() {
            Type[] signature = new[] { typeof(Corpse), typeof(Pawn), typeof(float) };

            miGetProducts = typeof(Corpse_ButcherProducts).GetMethod(nameof(GetProducts), BindingFlags.Public | BindingFlags.Static, null, signature, null);
        }

        /// <summary>
        /// Original code taken from Corpse.ButcherProducts iterator.
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
        public static IEnumerable<Thing> GetProducts(Corpse corpse, Pawn butcher, float efficiency) {
            foreach (Thing product in corpse.InnerPawn.ButcherProducts(butcher, efficiency)) {
                yield return product;
            }

            if (corpse.InnerPawn.RaceProps.BloodDef != null)
                FilthMaker.MakeFilth(butcher.Position, butcher.Map, corpse.InnerPawn.RaceProps.BloodDef, corpse.InnerPawn.LabelIndefinite(), 1);

            if (corpse.InnerPawn.RaceProps.Humanlike) {
                butcher.needs.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.ButcheredHumanlikeCorpse, null);             // CHANGE/FIX HERE
                foreach (Pawn pawn in butcher.Map.mapPawns.SpawnedPawnsInFaction(butcher.Faction)) {
                    pawn.needs.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.KnowButcheredHumanlikeCorpse);
                }
            }

            TaleRecorder.RecordTale(TaleDefOf.ButcheredHumanlikeCorpse, butcher);
        }

        /// <remarks>
        /// The returned instructions are invoked from <em>inside</em> <see cref="Corpse.ButcherProducts"/>, so any refered methods must allow access (can't be private!!!) and
        /// the local stack layout at invocatrion location needs to be kept in mind
        ///  - arg0: 'this' (<see cref="Corpse"/>)
        ///  - arg1: 'butcher' (<see cref="Pawn"/>)
        ///  - arg2: 'efficiency' (<see cref="float"/>)
        /// </remarks>
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr) {
            return new[] {
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Ldarg_2),
                        new CodeInstruction(OpCodes.Call, miGetProducts),
                        new CodeInstruction(OpCodes.Ret)
                   };
        }
    }
}
