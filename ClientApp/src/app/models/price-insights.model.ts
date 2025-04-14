export interface PriceInsights {
    lowestPrice: number;
    priceLevel?: string; // e.g., "high", "typical"
    typicalPriceRange?: [number, number]; // [low_bound, high_bound]
    priceHistory?: [number, number][]; // [[timestamp, price], ...]
} 