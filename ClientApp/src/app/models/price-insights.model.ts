export interface PriceInsights {
    lowest_price?: number;
    typical_price?: number;
    price_level?: string; // e.g., "high", "typical"
    typical_price_range?: [number, number]; // [low_bound, high_bound]
    price_history?: [number, number][]; // [[timestamp, price], ...]
} 