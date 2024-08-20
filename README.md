## How to Add Monsters
- [Youtube Video](https://youtu.be/f6c1Nyx26AU)


## How to build
1. Download Godot 4.1.2: https://godotengine.org/download/windows/
2. Add Godot to PATH variable
3. Run the Importer to build the database
4. Copy built databse to <root>/BeastiaryDB.sqlite


## What's Next
Next flow: Add Skill
	- Program: upload known skills to db
	- Beast Workflow: add skill workflow
	- Skill Workflow: create
		+ include Resolver and dependencies
	- Instance (db): 
		+ Implement not implemented methods
			* use https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder

	
## Bugs	
	- get rid of any interface methods that don't need to be implemented
	- nuke database to get rid of orphans (maybe add this as a clean up task?)
	- list of impossible book creatures (Razorbird, Cutterpillar, Hivekin, Cockatrice, Alraune)
	- What happens if the process is interrupted?
	- allow for lower case attributes
	- check for disallowed max hp/mp values
	- attack names should be in quotes for importer
	- implement magic accuracy check skill
	

Pixel Art Generation:
	- use a image to pixel art generator with crazy colors	



Random stuff:
	