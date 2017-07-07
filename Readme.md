[![RimWorld Alpha 17](https://img.shields.io/badge/RimWorld-Alpha%2017-brightgreen.svg)](http://rimworldgame.com/)

# Rimworld butcher & prisoner feed fixes

## Fixes

### Butchering fix

Fixes a bug in Rimworld A17 where any butcher jobs targetting human corpses will throw an error and never complete (to be restarted again &amp; again) if the executing pawn does not have a 'mood' property in it's needs tab.

A case where this might happen is Allestor's [Misc Robots++](http://steamcommunity.com/sharedfiles/filedetails/?id=747645520) mod which includes kitchen bots. And while this mod _exposes_ the underlying bug, it's not the cause of the issue at all - the underlying problem is inside the vanilla Rimworld code.

---

Funnily enough the vanilla code checks if any intended recipient of the '_We butchered humanlike_' debuff can actually get such a mood, but just assumes the butcher itself can always get the '_*I* butchered humanlike_' debuff.

So until a fix is made in vanilla Rimworld for this, just use this mod to prevent the issue from cropping up if you use any mods that add Pawns with no 'mood' need who might 'baconify' certain corpses...

### Prisoner feeding fix

Fixes a bug in Rimworld where feeding an incapacitated prisoner from a Nutrient dispenser by a non person pawn (aka a robot) will log an error each time (Job will be executed though).

Again, the buggy code is all vanilla - it just takes a mod to expose the error, since non-person pawns capable of feeding prisoners are not part of vanilla. 

## Install instructions

Just download this mod from [github](https://github.com/DoctorVanGogh/Rimworld-ButcherMoodFix) or the [Steam workshop](http://steamcommunity.com/sharedfiles/filedetails/?id=955427965).
Put the mod as far up in the load order as possible (there's actually no reason at all not to put it immediately below the 'CORE' mod). Since this mod completely rewrites parts of the butchering code it should be loaded before any other mods that modify that code. Any 3rd party mod that actually replaces (and not just modifies) butchering code will not work with this mod - you'll have to pick one or the other.

## Powered by Harmony

![Powered by Harmony](https://camo.githubusercontent.com/074bf079275fa90809f51b74e9dd0deccc70328f/68747470733a2f2f7332342e706f7374696d672e6f72672f3538626c31727a33392f6c6f676f2e706e67)