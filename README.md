# RimWorld-LanguageWorker_Spanish
![]( https://raw.githubusercontent.com/wiki/b606/RimWorld-LanguageWorker_Spanish/images/LWKR_Spanish_Mod_banner.png)

A mod to make the Spanish language in RimWorld as grammatically correct as possible.

Credits:

 - Modified from https://github.com/b606/RimWorld-LanguageWorker_French, which idea originated from Elevator89's LanguageWorker.
 - Include some code snippets from the official LanguageWorker_Spanish and translators work.
 - Thanks to Pardeike for his fabulous libHarmony patching library.

### 1. Goal
 - Maintain the Spanish RimWorld Translation at maximum quality.
 - Make the Spanish LanguageWorker in RimWorld at maximum quality.
 - Testbed for the Spanish LanguageWorker in RimWorld.

### 2. Features

This mod improves the grammar and typography rules specific to the target language in RimWorld, which are impossible to implement without modifying the C# code built in the game. It is a complement to the translators work at https://github.com/Ludeon/, until the patches that it introduces will be obsoleted by some future version of RimWorld.

This mod does not add more translation, so you need to get your language translation as complete and as updated as possible.

The mod is also designed to be as independent as possible and does not require any modification or special annotations in the official translation.It should not theoretically interfere with any other mods, it should work on any saved game, and can be activated or deactivated at will.

**Features list:**

- Fix definite and indefinite articles.
- Fix plurals for single words. The list of exceptions to plurals such as irregular, foreign words, and invariants needs to be updated when needed (WIP). Plurals for compound or hyphenated words is also a WIP (spanih speaking testers needed.)
- Contractions: "de el" -> "del", "a el" -> "al".
- Tribal pawns, cities and towns, factions and geographical features get proper uppercase.
- Quest titles get specialized uppercase algorithm (WIP, adapt to the cultural practice).
- Other text post-processing: "de sexo varón" -> "de sexo masculino", "de sexo mujer" -> "de sexo femenino". The current RimWorld grammar system does not provide a way to deal with the psychic drones affected gender correctly. 
- (*)Epicene animals: the mod defines male only or female only animal labels. As of now, it appends a gender qualification to the generic label, ex. "la rata", "la rata macho" or "el rhinoceros hembra". When the animal is tamed and named, the gender revert back to the physical gender.Please, advise if this behaviour is inappropriate. NB: This is for single animal only, generic animal label (XXX_kindLabel) must be handled correctly in the official translation files.
- (*)WIP: XXX_possessive depends on the object (possessed) gender, not the subject (possessor) gender. In French, that was essential. In Spanish, difference should be detected only for plural ("sus") but the patch can do more than that.
- WIP: other elisions can be implemented (the French version has many of them, here spanih speaking testers can ask if more is needed)

My Spanish speaking level is less than zero so I need testers to verify the correctness of what is implemented and to define the area of improvement. Please check the opened issues if one is related to your subject. Also, please feel free to submit suggestions and bug reports.

Features marked with (*) cannot be implemented in the vanilla LanguageWorker_Spanish and require the provided patches or modification of the RimWorld built-in grammar engine.

### 3. Installation

- **Subscribe to the mod on Steam Workshop** (https://steamcommunity.com/sharedfiles/filedetails/?id=2081845369) and activate ou deactivate the mod "LanguageWorkerSpanish_Mod" in the game,

- or **download the zip archive** (https://github.com/b606/RimWorld-LanguageWorker_Spanish/releases/latest), and extract it in the Mods folder of RimWorld, generally:

    - on Windows : `C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\LanguageWorker_Spanish_Mod`
    - on Linux : `~/.steam/steam/steamapps/common/RimWorld/Mods/LanguageWorker_Spanish_Mod`
    - on Mac : go to Finder, select `~/Library`, Application Support, Steam, SteamApps, Common, Rimworld, RimWorldMac, right-clic and "Show package contents" (https://ludeon.com/forums/index.php?topic=3549.msg468403#msg468403).

![]( https://raw.githubusercontent.com/wiki/b606/RimWorld-LanguageWorker_Spanish/images/LWKR_Spanish_Mod_folders.png)


### 4. Changelog

2020/11/21.
  - Initial release.

2020/11/18.
  - Initial commit.

### 5. Work in progress

  - Any suggestion is welcome.
 
---
[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Z8Z51KQ21)
