# RimWorld-LanguageWorker_Spanish
![]( https://raw.githubusercontent.com/wiki/b606/RimWorld-LanguageWorker_Spanish/images/LWKR_Spanish_Mod_banner.png)

A mod to make the Spanish language in RimWorld as grammatically correct as possible.

Credits:
 - Modified from https://github.com/Elevator89/RimWorld-LanguageWorker_Russian
   (Credit to Elevator89)
 - Include some code snippets from the official LanguageWorker_Spanish and translators work at
   https://github.com/Ludeon/RimWorld-fr/ (Credit to Adirelle for the first regexes)
 - Thanks to Pardeike for his fabulous libHarmony patching library.

### 1. Goal
 - Maintain the Spanish RimWorld Translation at maximum quality.
 - Make the Spanish LanguageWorker in RimWorld at maximum quality.
 - Testbed for the Spanish LanguageWorker in RimWorld.
 
### 2. Vérification des résultats

Les modifications faites par ce mod concernent essentiellement la qualité de la traduction, qui sont impossibles à faire sans modifier le code C# du jeu. Ce mod est donc un complément du travail des traducteurs de RimWorld-fr (https://github.com/Ludeon/RimWorld-fr/) pour le jeu actuel, ou en attendant l'intégration de ces modifications dans une prochaine version du jeu.

Le mod est focalisé sur les accords de mots, les règles de grammaire ou de typographie etc. Si vous avez l'œil averti, vous verrez un subtil changement par-ci par-là. J'espère que vous apprecierez les quelques améliorations dans le français du jeu :smile:

Des captures d'écran et des rapports de test détaillés les illustrent dans le Wiki (https://github.com/b606/RimWorld-LanguageWorker_Spanish/wiki).

N'hésitez pas à remonter les suggestions, les problèmes ou bugs sur ce site en ouvrant une nouvelle page dans l'onglet Issue.

### 3. Installation

#### a. Installation pour les versions supérieures à 1.1.0

À partir de la version 1.1.0, le mod est sur Steam (https://steamcommunity.com/sharedfiles/filedetails/?id=2081845369). Il suffit d'y souscrire (bouton **Subscribe**) et d'activer ou désactiver le mod "LanguageWorkerSpanish_Mod" dans le jeu.

Sinon, il faut de télécharger l'archive zip des binaires compilés (https://github.com/b606/RimWorld-LanguageWorker_Spanish/releases/latest), de l'extraire dans le répertoire de Mods de RimWorld :

 - sous Windows : `C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\LanguageWorker_Spanish_Mod`
 - sous Linux : `~/.steam/steam/steamapps/common/RimWorld/Mods/LanguageWorker_Spanish_Mod`
 - sous Mac : aller dans le Finder, mettre `~/Library`, Application Support, Steam, SteamApps, Common, Rimworld, RimWorldMac, clic-droit et "Show package contents" (https://ludeon.com/forums/index.php?topic=3549.msg468403#msg468403).

![]( https://raw.githubusercontent.com/wiki/b606/RimWorld-LanguageWorker_Spanish/images/LWKR_Spanish_Mod_folders.png)
 
#### b. Installation alternative pour les versions avant 1.1.0

EDIT 2020/05/01: la procédure pour les versions avant le 1.1.0 est toujours valable mais non nécessaire.

  1. Télécharger l'archive zip des binaires compilés (https://github.com/b606/RimWorld-LanguageWorker_Spanish/releases/latest) et l'extraire dans le répertoire de Mods de RimWorld.
 
  2. Activer le mod "LanguageWorkerSpanish_Test" dans le jeu.
    
  3. Modifier le fichier LanguageInfo.xml dans le répertoire `Core/Languages/RimWorld-fr` (ou `Core/Languages/Spanish`) pour contenir `<languageWorkerClass>RimWorld_LanguageWorker_Spanish.LanguageWorker_Spanish</languageWorkerClass>` au lieu de `<languageWorkerClass>LanguageWorker_Spanish</languageWorkerClass>`.
   
  4. Relancer RimWorld.
 
  *Désinstallation* : refaire les étapes précédentes dans l'ordre inverse et désactiver le mod dans le jeu.
 
### 4. Changelog

2020/11/14.
  - MaJ de la description sur Steam.

2020/10/20.
  - Correction des textes avec genre dans GameConditions, ex. pour les drones psychiques.

2020/10/20.
  - Correction des élisions "(à|À) <tag>le(s?) " séparées par un tag de couleur de texte.

2020/09/26.
  - Mise-à-jour vers la version 1.2. Les prénoms commençant par 'H' ou les prénoms courts
    (fixés à moins de 5 caractères) débutant par une voyelle n'ont pas d'élision.
    Ex. "le chapeau de Anne" ou "le chapeau de Hector" au lieu de "le chapeau d'Anne" ou
    "le chapeau d'Hector". Sinon, on devrait avoir des textes similaires à "le chapeau
    d'Alexandre".
	
2020/05/05.
  - Corrections pour fixer le genre des mots considérés neutre en anglais (patchs des RulesForDef et 
    RulesForBodyPartRecord). Plusieurs marqueurs "x_possessive" sont donc fonctionnels pour les traducteurs de
    RimWorld-fr, au lieu de "son/sa" (ou "le/la") jusqu'ici.
  - Les labels toujours au pluriel sont détectés. Comme il se doit, ils se résoudront en "ses" pour les tags 
   "x_possessive", "les x" pour "x_definite" et "des x" pour "x_indefinite".
  - Développement complet de la fonction de pluralisation pour presque tous les labels dans le jeu (1555 labels),
    et correction de certains d'entre eux dans la traduction RimWorld-fr.

2020/05/01.
  - Depuis la version 1.1.0, le mod paramètre automatiquement le module de traitement pour le langage français (languageWorkerClass). L'activation est basée sur le nom interne du langage ("friendly name: Spanish") et ne concerne pas les autres langages qui n'ont pas ce nom interne. Il n'y a plus besoin de toucher au fichier LanguageInfo.xml.

2020/04/29.
  - Correction des genres grammaticaux des espèces d'animaux :
    PawnKind toujours au féminin en français : Boomalope, Gazelle, Megaspider, Ostrich, Tortoise.
    PawnKind toujours au masculin en français : Alphabeaver, Bear_Grizzly, Boomrat, Capybara, Caribou, Cassowary, Chinchilla, Cobra, Cougar, Dromedary, Elk, Emu, Fox_Fennec, GuineaPig, Husky, Iguana, LabradorRetriever, Lynx, Megascarab, Megasloth, Muffalo, Raccoon, Rhinoceros, Spelopede, Squirrel, Thrumbo, Warg, YorkshireTerrier.

2020/04/27.
  - Patch pour corriger les accords de genre des noms d'animaux (et des pawns en général), illustration dans le wiki.
  - Le nom du mod est officiellement LanguageWorker_Spanish_Mod (b606.languageworkerfrench.mod).

2020/04/21.
  - Tous les noms dans le jeu sont catégorisés correctement, avec une première version de mise en majuscules ou minuscules comme il se doit (amélioration à suivre).
  - Les titres des quêtes ne sont plus tout en majuscule.
  - Complète la liste des mots avec exception aux règles.
  - En interne, cartographie du code de RimWorld.

2020/04/05.
  - Mise en place des fichiers pour utiliser Monodevelop et NUnit testing framework.
  - Corrige les élisions y compris avec les tags de soulignement de noms :
    - Corrige les listes de voyelles.
    - Modification des Regex.
    - Prend en compte les h muets et h aspirés (liste de mots limitée à ceux contenus
      dans RimWorld, à mettre à jour si besoin).
  - Corrige les X_possessive [mst]a et [mst]on/[mst]a en [mst]on avant une voyelle et h muet.
  
### 5. Travaux en cours

  - ~~Corriger les noms d'espèces au masculin ou féminin dans les textes du jeu (si la prochaine version de RW ne le corrige pas).~~
  - ~~Pouvoir activer le mod sans toucher au fichier LanguageInfo.xml : nécessite de se brancher dans le code de démarrage.~~
  - Développer d'autres patches là où il y aurait besoin (les Tales, les textes des interfaces etc).
  - Toute suggestion sera la bienvenue.
 
---
[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Z8Z51KQ21)
