#private ToDo
add damage to charge when channeling
add damage to crossbow when target is close
i have a dash, it interrupt spell but its not casted afterward need a cast on top feature
system to push other player (collider if a player charge the ally in the path must be pushed, two ally player charger should cancel and stun such as collision with wall)
make a system where the direction of impact count, if an arrow it in front it can be blocked
create a cd manager for spells ? Or if i just have the dash on cd and dont need a whole manager for that
make an enemy that can shoot stuff so character can take dmg and test stuff
i need to add phase option that allow action and movement
AnyState transition for the exit_loop trigger -> CHECK
Who call the end of the spell ? The combatCharacter raised event or the Fatetoken. Its obvious, the token is used to communicate between spells and combatManager to interrupt phase/spell. When the spell end, the combat manager send an event to the gamemanager
if its the charge, if we interrupt while looping the endphase is skipped -- or I need to play an alternative endphase -- but i still need to implement a system that can kill the spell and the animation on the spot
attach weapon with weaponHandler
-Link spell with weapon apparition
-create a projectile for the fire spell // add damage when the projectile hit an enemy
-Improve damage component and stat component to have buff and debuff, one entry point. Use SO to make buff and a class to play them during runtime 1 SO (Buffs_data) 1 Class (BuffRuntime) 1 Mono (StatsComponent)
1 BuffRuntime not like spell that handle all the buff and debuff of the character and send data to a new CombatManager more like GameManager
Composition over inheritance : avoir la possibilite de creer un buff avec une combinaison de sous buff (Hot + damage reduction + Increase Damage)
Charge need to be stopped and maybe two end animation or skip end animation totally so i need a way to exit coroutine entirely
merge the new interrupt function in the CharacterCombat
Remove the logic from the CharacterManager as much as possible and send clear event to change the CharacterState
what if my character is rooted and can still cast ? there is two state that should coexists but only one will -> state that impair character control and state that can run in parallel
what if my character is immune for a short time create a buff list ? I think it is independant of the state of the character and it should be in a class that handle health loss, damage, buff etc
Rename the current class CharacterCombat to something that represent its function (CharacterSpellCasting) and create a CharacterCombat that can instantiate it. And I could add a class that handle buff, debuff, damage, modify character stats
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

## Warrior
### Shield
when the charge is stopped manually before touching something, make a stop animation that take a bit of time to make the char vulnerable ?
if second end animation, add a list instead of one animation in the SO.
In the scenario of two character charging at each other, the damage could be dependant of the charge duration (charge give an bloc )
add the option to slightly turn when the charge is on (like sion)
add more damage if the charge is canalized from far away
when charging the character is slower because exhausted and need a bit of time to recover (dash still alowed)
Talents idea:
able to slighlty move during charge
abse to interrupt charge
### Crossbow (pour les squelettes faire un gros gun qui tire un boulet plus coherent)
Input can be hold to increase the damage -> wind up animation wind up end animation
make the damage near the target way bigger (use the wind up to do that much dmg from range) to force player to close the gap
Have a big reload animation so if the player want to shot twice it pays the risk of having to be stuck in the animation
### Two Hander (impaling verion : sword, lance)
having a wind up and dash forward, impale
### Two Hander (Smash version, hammer, axes)

## Wizard
### Fire (hand in fire cooler ?)
Become a ball of fire (movement to the cursor, find a way to make it slippery with momentum) doing area damage then when user want explode, killing him in the process ?
Laser ofc
### Lightning Staff (all body electrified)
the classic eletric chain
### Arcane
create a wall that can be destroyed
### Ice
ice block

## Warden
### Aspect of the Bear
### Aspect of the Crow
### Aspect of the Cheetah 

## Warlock

### Necromancy
### Life stealer  
### Chaos
timer >= tick appy dmg and reset timer
### Create SFX & VFX
Faire jouer les sons et veffets depuis le sort directement ou bien trigger un event a un SpellEffectCoordinator qui aura les refs des SFX et VFX
SFX sur chaque object envoyant un son ? Non uniquement un singleton SFX.
Faire des effets bandes dessinees pour la vitesse (hades), les impacts -> Claude peut generer des VFX ? Non mais peut aider a generer des shaders et des VFX graph et shuriken particle system

### GamePlay Idea
Two main gameplay issue
- How many weapon can a player carry ? lets start by 3
Have an idea where it could be one Weapon, one Element, one Aspect OR Warrior Archetype, Wizard, Warden, Warlock, WrathBringer
- How to make the player want to use all the weapon he has (crossbow reload time but for other one ? Shield explode when block ? surcharge for lightning spell ?)
- How to make kill conditon fun and not frustrating ?
Before starting the game, the player have the option to pick 3 weapons and maybe enchantment next (mimicking gear set in eso) and choose one ultimate
put the spells where you want
3 bases spells : punch when close stun ? maybe not , dash (dash into someone stun him), block (small immunity different than shield abilities, shield block from front attack only outplay can happen here ? In the orientation and animation lock)
1 weapon could have 5 spells and the player has to choose two
The ultimate generation cant work like in eso because i want the combat to be fast paced with huge dmg. A charge can kill someone on the spot such 

Having ragdoll effect on deaths

Character could have multiple live to make the combat more interesting (when killing some one else took their soul ? visual looking skeleton aura red, blue etc)

having a dash really often that can interrupt animation 
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
## VFX / Shader
![alt text](C:/Users/monta/OneDrive/Documents/Unity/Images/lightreflexion.png)
## Coding eurekas
How to merge the two interrupt method is the CharacterCombat ? pass a string and if it is null its the method that dont need to check data spell name (also spellData data = null to set a default value to a method parameter) 
Monobehaviour hook with static event in a monobehaviour class that trigger an event every update tick with the time
rule of thumb, if class use unity api, initilize them in awake or start. If not, its fine to initialize them in field

usage of out and creation of the variable in the parameter out + var
`if (!TryGetInterruption(interrupt_reason, currentPhase, out var interrupt_data))`
        `return false;`
`if (TryGetImmunity(interrupt_data.Interrupt, interrupt_data.Phase, out var interruptWindow)`
        `&& interruptWindow > spellElapsedTime)`
shadowing de variables