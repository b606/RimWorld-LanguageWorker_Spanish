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
  
### 5. Work in progress

  - Any suggestion is welcome.
 
---
[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Z8Z51KQ21)
