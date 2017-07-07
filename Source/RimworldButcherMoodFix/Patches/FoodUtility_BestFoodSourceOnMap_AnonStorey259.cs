using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Harmony;
using RimWorld;
using Verse;

namespace DoctorVanGogh.RimworldFixes.Patches {

    class FoodUtility_BestFoodSourceOnMap_AnonStorey259 {

        public static bool IsIngestibleBetterThanDesperate(Thing t) {
            return (t is Building_NutrientPasteDispenser
                           ? ThingDefOf.MealNutrientPaste
                           : t.def).ingestible?.preferability > FoodPreferability.DesperateOnly;
        }

        /// <remarks>
        /// Workaround for:<br />
        /// RimWorld.FoodUtility+&lt;BestFoodSourceOnMap&gt;c__AnonStorey259.&lt;&gt;m__1 method (compiler genreated anonymous type - yay!)
        /// has a missing <see langword="null" /> check.
        /// Funnily enough this branch of the code is only ever reached if the performing pawn is a robot (I'm assuming any 
        /// non-person/animal would be enough). 'People' pawns break out earlier and never arrive at this missing null check.
        /// </remarks>
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr) {
            /*  Replace
             *  
             *      t.def.ingestible.preferability > FoodPreferability.DesperateOnly
             *      
             *  check with 
             *  
             *      (t is Building_NutrientPasteDispenser ? ThingDefOf.MealNutrientPaste.ingestible : t.def.ingestible)?.preferability > FoodPreferability.DesperateOnly
             */

             // extra checks introduce more bytes, and extra Labels - just substitute with method call and pad no-op

            /* IL Changes:
                    [#I=Instruction Index, A=Action, B=Byte size change, C=IL position/Label, Op=Opcode, Arg=Argument]
              
			        #I	A	B	C			Op			Arg 
									        ...
					13			IL_0025:	ldarg.1
					14	<<<	-5	IL_0026:	ldfld		class Verse.ThingDef Verse.Thing::def
						>>>	+5				call		bool DoctorVanGogh.RimworldButcherMoodFix.Patches.FoodUtility_BestFoodSourceOnMap_AnonStorey259::IsIngestibleBetterThanDesperate(class ['Assembly-CSharp']Verse.Thing)
					15	<<<	-5	IL_002b:	ldfld		class RimWorld.IngestibleProperties Verse.ThingDef::ingestible
						>>>	+1				nop					
						>>>	+1				nop					
						>>>	+1				nop					
						>>>	+1				nop					
						>>>	+1				nop					
					16	<<<	-5	IL_0030:	ldfld		valuetype RimWorld.FoodPreferability RimWorld.IngestibleProperties::preferability
						>>>	+1				nop					
						>>>	+1				nop					
						>>>	+1				nop					
						>>>	+1				nop					
						>>>	+1				nop					
					17	<<<	-1	IL_0035:	ldc.i4.2
						>>>	+1				nop
					18	<<<	-5	IL_0036:	ble			IL_0046
						>>>	+5				brfalse		IL_0046
					19						...
            */

            List<CodeInstruction> instructions = new List<CodeInstruction>(instr);

            var ci = instructions[18];
            instructions[18] = new CodeInstruction(OpCodes.Brfalse, ci.operand);        // 'old' operand contains label to use
            instructions[17] = new CodeInstruction(OpCodes.Nop);
            instructions.RemoveAt(16);
            instructions.InsertRange(16, Enumerable.Repeat(new CodeInstruction(OpCodes.Nop), 5));
            instructions.RemoveAt(15);
            instructions.InsertRange(15, Enumerable.Repeat(new CodeInstruction(OpCodes.Nop), 5));
            instructions[14] = new CodeInstruction(OpCodes.Call, typeof(FoodUtility_BestFoodSourceOnMap_AnonStorey259).GetMethod(nameof(IsIngestibleBetterThanDesperate)));
 
            return instructions;
        }
    }
}
