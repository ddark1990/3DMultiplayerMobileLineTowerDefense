public static class EventIdHandler
{
    public enum EVENT_IDs : byte
    {
        PLACE_TOWER_EVENT = 0,
        SEND_CREEP_EVENT = 1,
        TOWER_SHOOT_EVENT = 2,
        TIMER_EVENT = 3,
        MATCH_START = 4,
        MATCH_END = 5,
        PLAYER_WON = 6,
        PLAYER_DEDUCT_LIFE = 7,
        PLAYER_INCREASE_GOLD = 8,
        PLAYER_DEDUCT_GOLD = 9,
        PLAYER_INCREASE_INCOME = 10,
        PLAYER_ANNOUNCE_LOSS = 11
    }
}
