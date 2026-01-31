namespace RogueLite.Core
{
    /// <summary>
    /// GameConstants: Constantes globais do jogo
    /// </summary>
    public static class GameConstants
    {
        // Layers
        public const string LAYER_PLAYER = "Player";
        public const string LAYER_ENEMY = "Enemy";
        public const string LAYER_PROJECTILE = "Projectile";
        public const string LAYER_PICKUP = "Pickup";

        // Tags
        public const string TAG_PLAYER = "Player";
        public const string TAG_ENEMY = "Enemy";
        public const string TAG_PROJECTILE = "Projectile";

        // Scene Names
        public const string SCENE_MENU = "Menu";
        public const string SCENE_GAME = "Game";

        // XP System
        public const float XP_PICKUP_RADIUS = 1.5f;
        public const float XP_MAGNET_RADIUS = 5f;
        public const float XP_MAGNET_SPEED = 10f;

        // Combat
        public const float DEFAULT_KNOCKBACK_DURATION = 0.2f;
        public const float DEFAULT_INVULNERABILITY_DURATION = 0.5f;
    }
}