public enum AimDirection
{
    Right,
    Left
}

public enum GroundGenMethod
{
    PerlinNoise,            // Smooth, natural-looking terrain
    WeightedRandom,         // Random terrain with weighted probabilities
    PatchBlend,             // Multiple noise layers blended
    CellularAutomata,       // Organic, blob-like patterns
    Zones                   // Different zones of different tiles
}