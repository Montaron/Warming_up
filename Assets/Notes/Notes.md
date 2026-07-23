# ToDo
put the shield on the hand while charging // make a system for each spells has a way to handle weapon
-> spellData has the information about the weapon -> SpellRuntime use it to communicate with the WeaponHandler class
How spellData define the information ? LH (Left Hand) RH (Right hand) BH (Both hand) TH (Two_Hand) and then which kind of weapon (BOW ? TWO_HANDERS ? 1H_SWORD ? 2H_Arbalest ? 1H_SHIELD ?)
How to create a nice hierarchy of class and objects. Class are just patron to objects so maybe I just should track the object and reference to other objects
Use Layer to make hierarchy
Layer Event Object trigger event ? Which ? Where 
Layer Objects how they interact and keep reference of each other

# Game Monolith
## GameManager
## Character
functions split in multiple class:
 -CharacterMovement that handle the character movement
-CharacterAnimation that handle the character animation
-CharacterCombat that handle the character combat
*OnSpeedEnded
-CharacterStateMachine that handle the state transition of the character
-CharacterManager where character road cross, receive event from input, statemachine and characterCombat to change character behaviour (moving, casting,...) 
-WeaponHandler a class that instantiate weapon on the right slot on the character  
## Input
-InputHandler handle the input and keybinds associated with spells and send change to the characterManager and prob UIManager next...
*OnMoveInput, *OnSpellRequested
## Spell Error token
-SpellFateToken is a class that can create a token object linked to a spell and if the token is cancelled to spell is stopped (BaseSpellRuntime) and trigger an event
*OnSpellCancelled

track the event with *
Create a spell Manager that can easily provide spell to the character
8/7/26
## Spells architecture
main class (abstract) is BaseSpellRuntime that has a constructor
the class has a main coroutine method to run spell : StartSpell 
This method will run Start, Loop and End coroutine
the spell specificities is defined in Spell_data (SO) and his childs like ChargedSpell_data
the Spell_data has a method to create the BaseSpellRuntime GameObject
the BaseSpellRuntime implement an interface that :
-demand validation (caster exists, animator present,...) through validate method
-demand StartSpell implementation
-has method that is called when spell is finished

## others
zz : center on cursor
:only : remove all other windows with all scripts in it
space + b + o : remove all other scripts
search and replace in vscode ^(\s*)(.*Debug\.Log.*)$ -> 
* next word under cursor # backward direction
Search file in explorer : ctrl + p
Search for text insides files : ctrl + shift + p
Toggle Maximize Editor Group : shit alt m
Command Palette : Ctrl + shift + p
7/7/26
Animation end trop lente de la charge
## vim
in settings.json in order to have relative line in normal mode
and real line number on insert mode
`
// Line numbers: absolute base, relative in normal mode, absolute in insert
  "editor.lineNumbers": "on",
  "vim.smartRelativeLine": true,

  // To improve performance
  "extensions.experimental.affinity": {
    "vscodevim.vim": 1
`
move 10 line up : 10k
move 10 line down : 10j
move to the 10th line : 10G
delete 10 line + the current one : d10j
delete 10 line from the current one : 10dd
## git
**before pulling if I made change already**
`git stash`
`git pull origin main`
`git stash pop`

**check new update on remote rep before pulling**
fetch
`git fetch`
check for the new commit on the remote repo
`git log HEAD..origin/main --oneline`
check for files where change happen
`git diff --stat HEAD..origin/main`
check for merge conflict
`git merge --no-commit --no-ff origin/main`
merge if everything is ok
`git merge`
## Mermaid
Install Mermaid support tool on VScode
ctrl + shift + v to see preview mode

#Objects
used doted arrow for reference between objects
used arrow for an object creating another one

#Events
type de name of the event with the type it carry between the arrow