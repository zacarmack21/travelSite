export interface FlightSearchRequest {
    departureId: string;
    arrivalId: string;
    outboundDate: string; // Format: YYYY-MM-DD
    returnDate?: string; // Format: YYYY-MM-DD, Optional for one-way
    type: number; // 1=Round trip, 2=One way
    adults: number; // Default to 1 adult
    children?: number;
    infantsInSeat?: number;
    infantsOnLap?: number;
    hl?: string; // Language
    gl?: string; // Country
    currency?: string; // e.g., USD
    departureToken?: string; // Token for fetching return flights
} 