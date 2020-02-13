using System;

namespace CSharpWars.Scripting
{
    public static class Constants
    {
        // Damage inflicted on melee attack.
        public const int MELEE_DAMAGE = 10;
       
        // Damage inflicted on melee attack from behind.
        public const int MELEE_BACKSTAB_DAMAGE = 15;
        
        // Damage inflicted on ranged attack.
        public const int RANGED_DAMAGE = 1;
        
        // Damage inflicted on neighboring bots, range 1, on self destruction.
        public const int SELF_DESTRUCT_MAX_DAMAGE = 10;
        
        // Damage inflicted on neighboring bots, range 2, on self destruction.
        public const int SELF_DESTRUCT_MED_DAMAGE = 4;
        
        // Damage inflicted on neighboring bots, range 3, on self destruction.
        public const int SELF_DESTRUCT_MIN_DAMAGE = 1;
        
        // Maximum range for ranged attacks.
        public const int MAXIMUM_RANGE = 6;
        
        // Maximum range for teleporting.
        public const int MAXIMUM_TELEPORT = 6;

        // Stamina use for a single move
        public const int STAMINA_ON_MOVE = 1;

        // Stamina use for a teleport
        public const int STAMINA_ON_TELEPORT = 10;
    }
}