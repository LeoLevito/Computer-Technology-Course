Regarding being performance conscious, I tried to use jobs (IJobEntity) as much as possible to utilize multithreading, along with decorating the systems and jobs with the BurstCompile decorator to utilize the burst compiler to improve performance.

Relevant files:

/Input:
- GameInput

/Prefabs:
- Player	
- Enemy
- Projectile
- Spawner

/Scenes
/SampleScene:
- SubScene

/Scripts
	/Collision:
 - CollisionLayer, Enum that holds layers that raycasts using Unity Physics package can check for.
  
  /Enemy:
  - EnemyAuthoring, attached to the Enemy prefab, handles baking of IComponentData EnemyComponent.
  - EnemyComponent, holds data for the enemy's MoveSpeed, DeathTimer, TimeToKill & entity.
  - EnemySystem, creates jobs on OnUpdate for the enemy spawning, moving and killing. Those IJobEntity jobs are defined here as well.
  
  /Input:
  - InputComponent, IComponentData that holds float2 MoveInput and bool ShootInput.
  - InputSystem, updates InputComponent data based on the player's input that is being read here as well.
  
  /Player:
  - PlayerAuthoring, attached to the Player prefab, handles baking of IComponentData PlayerComponent & InputComponent.
  - PlayerComponent, holds data for the player's MoveSpeed and the ProjectilePrefab.
  - PlayerSystem, creates job on OnUpdate for the player movement. That IJobEntity job is defined here as well.
  
  /Projetile:
  - ProjectileAuthoring, attached to the Projectile prefab, handles baking of IComponentData ProjectileComponent.
  - ProjectileComponent, holds data for the projectiles's MoveSpeed, DeathTimer, TimeToKill & entity.
  - ProjectileSystem, creats jobs on OnUpdate for the projectile spawning, moving, colliding, and killing. Those IJobEntity jobs are defined here as well.
  
  /Spawner:
  - SpawnerAuthoring, attached to the Spawner prefab, handles baking of IComponentData SpawnerComponent.
  - SpawnerComponent, holds data for the spawner's EnemyPrefab, SpawnPosition, SpawnRate, NextSpawnTime & entity.

Note on the Spawner when it comes to waves: I have just placed 5 Spawner prefabs in the SubScene and they all spawn new enemies every 5 seconds, I'm not sure if you can consider this a proper wave-spawning system since it's very basic but it does spawn in waves kinda!

Added plugin: Unity Physics, the physics system for ECS. Used for collision detection, I wanted to learn this instead of doing distance checks for collision testing. Thankfully I found this video by Sasquatch B Studios (https://www.youtube.com/watch?v=EW4pwSOe5nA) which was helpful in setting up raycasts to perform collision checks with colliders using the Unity Physics package. The logic for this can be found in the ProjectileSystem.

I'm aware that making use of bigger IComponentData structs for each entity may or may not be the best way to do things if I were working on a larger game.
I'm aware that making use of bigger ISystem partial structs that create multiple jobs may or may not be the best way to do things if I were working on a larger game in order to scale further. 
I just wanted to try and keep script file amount manageable as well as to try and have a consistent way of writing the scripts.
