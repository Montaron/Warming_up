# ToDo
attach weapon with weaponHandler
Link spell with weapon apparition
create a projectile for the fire spell
composable effect system in additon to the SpellData, to add easy damage reduction, heal, status effects to the spell without having to create them each time
Charge need to be stopped and maybe two end animation or skip end animation totally so i need a way to exit coroutine entirely
orientation while channeling
Add two things : -GCD (CharacterCombat), UninterruptableTimerBy enum + Time (SpellData or BaseSpellRuntime or Both)
merge the new interrupt function in the CharacterCombat
Remove the logic from the CharacterManager as much as possible and send clear event to change the CharacterState
what if my character is rooted and can still cast ? there is two state that should coexists but only one will -> state that impair character control and state that can run in parallel
Spell can have two flags -> extension method where I define impairing State
what if my character is immune for a short time create a buff list ? I think it is independant of the state of the character and it should be in a class that handle health loss, damage, buff etc
Rename the current class CharacterCombat to something that represent its function (CharacterSpellCasting) and create a CharacterCombat that can instantiate it. And I could add a class that handle buff, debuff, damage, modify character stats
I need a monobehaviour hook to update spellElapsedTime if the class is no longer a monobehaviour -> I can consider using Time.Time 
CharacterCombat has 2 components : Caster and a buff and debuff class ? Or StatsComponent is good enough -> if an ennemy is hit check buff and reduce the initial amount and then apply reduce health   
How are buffs 1) Applied (through spell, through game bonus) 2) Handled (can start in StatsComponent then refactor)
Prob need a bunch of enums BuffType
CharacterCombat could be more general, for targeting,  
What happen to the SpellPhase when the spell is exited early ? Should create a new phase -> OnPhaseExit
How would I create a tick effect while channeling ? Get the target -> apply damage to the target while looping each tick. That mean I have the damage in the CharacterCombat and not the spell
I can classify 3 way of doing dmg, External collider (arrow), character collider (charging) and the targeting (lightning) -> One entry point many callers -> IDamageable interface
that should be the same for the buffer system. One entry point for everything.
Could be at start or at the end of a spell -> BuffComponent -> TryAddBuff -> the buff already exists or add them ? Need buff logic here (additive, unique, cap, immunity ?)
CharacterManager should be used to get data and not necessarly logic in it.
instead of bool return if a buff is applied I can return a enum : Succes, Fail_Resisted, ... 
### Weapon Change when casting
put the shield on the hand while charging // make a system for each spells has a way to handle weapon
-> spellData has the information about the weapon -> SpellRuntime use it to communicate with the WeaponHandler class
How spellData define the information ? LH (Left Hand) RH (Right hand) BH (Both hand) TH (Two_Hand) and then which kind of weapon (BOW ? TWO_HANDERS ? 1H_SWORD ? 2H_Arbalest ? 1H_SHIELD ?)
1) Easy methods to swap weapon in WeaponHandler 
2) In spell Data add a field to the corresponding weapon the spell is using
3) When the spell is created in CharacterCombat send the data is send to the CharacterManager and with the data, send info to the weaponHandler to swap weapon

### Sword and chield charge
add the weapon change 
when the charge is stopped manually before touching something, make a stop animation that take a bit of time to make the char vulnerable ?
if second end animation, add a list instead of one animation in the SO.
In the scenario of two character charging at each other, the damage could be dependant 
### Arbalest weapon
Create an arbalest in blender and a spell to fire with it
Create an fire animation in blender (start loop end)
How to make an explosive animation ? F curve bezier
Input can be hold to increase the damage -> wind up animation wind up end animation
Add that logic in the Loop coroutine of the Spell class
Interaction between the InputHandler and the Spell execution ?
press -> start hold ? loop wind up use getkeydown and getkeyup to send a bool event to the CharacterManager and when its false interrupt the loop coroutine. How ? Need to have enum for windup type spell
when winding up using a move keybind should interupt the spell and not trigger the fire
1. Spell is hold -> loop animation -> keyUp -> endAnimation -> trigger a projectile -> go back to iddle
2. Spell is hold -> Movement key is pressed -> loopAnimation is canceled -> no end Animation back to iddle
need a fast exit trigger that works for all spells
### Create SFX & VFX
Faire jouer les sons et veffets depuis le sort directement ou bien trigger un event a un SpellEffectCoordinator qui aura les refs des SFX et VFX
SFX sur chaque object envoyant un son ? Non uniquement un singleton SFX.
Faire des effets bandes dessinees pour la vitesse (hades), les impacts -> Claude peut generer des VFX ? Non mais peut aider a generer des shaders et des VFX graph et shuriken particle system

### GamePlay Idea
when mousebutton 1 is hold the character could be oriented toward the cursor to make targetting easier ?
How to make the stance dance fun -> how to make the player switch between them
3 Stances -> 3 spells
Change stance by using one spells or having control of which stance to go with specific binds ?
In Eso we swap with a bind and each bar need to be swaped to rebuff or heal/dps and set bonus change

I like the idea of having an ultimate and ultimate point that can be generated to use it for each stance (later on add the possibility to choice between multiple of them)
The ultimate generation cant work like in eso because i want the combat to be fast paced with huge dmg. A charge can kill someone on the spot such 

Stack of destabilisation that could lead to a 1sec stun and allow charge to 
In the continuation of this idea, the should could block 50dmg but if the shield block more than that amount the character is stun (the stun duration could be dependant of that that surplus blocked)

Maybe right now for proof of concept and code i could make one bar with charge, arbalest hit and a swing attack in 2hand with an ult if I can make that it means i can expand on more complex 
combat system

Having ragdoll effect on deaths

Character could have multiple live to make the combat more interesting (when killing some one else took their soul ? visual looking skeleton aura red, blue etc)

When channeling, could use other keybind to change the spell for instance, arrow could have AoE, Heal, Stun or Single target
the mouse spell could be the main one and more change to it than the other
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
this class is created by the SO dataSpell that has a method to call the constructor. 
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

## vscode

ctrl enter the file in explorer to open it in vertical split
## vim & VScode
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

mousehove to see whats the reason the code is underlined : space tab

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
check for deletion/addition in the file
`git diff HEAD..origin/main -- Assets/Notes/Notes.md`
check for merge conflict
`git merge --no-commit --no-ff origin/main`
merge if everything is ok
`git merge`
check specifil file commit history
`git log -p -- Assets/Notes/Notes.md | vim -`

add multiple -m to a commit to have multiple lines
`git commit -m "hello" -m "world"`
## Mermaid
Install Mermaid support tool on VScode
ctrl + shift + v to see preview mode

#Objects
use doted arrow for reference between objects
use arrow annoted with 1 for an object creating another one (start of the chain that lead to the object creation like the CharacterManager call for the creation of a spell)
use arrow annoted with 0 for object creating another indirectly (the end of the chain like one object call the constructor of another)
use thin arrow for the chain itself if necessary
Arrow From : Object Create --> Arrow To : This Object


#Events
type de name of the event with the type it carry between the arrow
Arrow From : Object has a reference --> Arrow To : To This Object

## Blender
Save as .fbx apply all transform ctrl + a
To import new animation import the whole character and ctrl + d to duplicate in Unity. Now i have animation detached from the character and usable in Unity
But i have to keep the model, so I want to clean up by having one model and not 10 for each new animations
I need one fbx file with an avatar or generic avatar assigned to the Animator. One i have make a .anim (ctrl + d) of all the animation in the fbx i can discard them (animation onglet dont import animation) 1 mesh 1 avatar and the animations

## Coding eurekas
How to merge the two interrupt method is the CharacterCombat ? pass a string and if it is null its the method that dont need to check data spell name (also spellData data = null to set a default value to a method parameter) 
Monobehaviour hook with static event in a monobehaviour class that trigger an event every update tick with the time
rule of thumb, if class use unity api, initilize them in awake or start. If not, its fine to initialize them in field