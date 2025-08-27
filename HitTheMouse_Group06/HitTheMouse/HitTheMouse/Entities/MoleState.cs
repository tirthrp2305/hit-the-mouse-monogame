namespace HitTheMouse.Entities
{
    /// <summary>
    /// Represents the different states a mole can be in.
    /// </summary>
    public enum MoleState
    {
        Ideal,          // No mole is present in the hole.
        GettingOut1,    // Mole begins to pop out (Frame 0).
        GettingOut2,    // Mole continues to pop out (Frame 1).
        FullyOut,       // Mole is fully visible (Frame 2).
        GettingIn1,     // Mole begins to retreat (Frame 3).
        GettingIn2      // Mole continues to retreat (Frame 4).
    }
}
