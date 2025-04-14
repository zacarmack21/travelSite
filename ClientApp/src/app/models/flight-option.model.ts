import { FlightSegment } from './flight-segment.model';
import { Layover } from './layover.model';
// Assuming CarbonEmissions will be defined elsewhere or is not strictly needed yet.
// import { CarbonEmissions } from './carbon-emissions.model';

export interface FlightOption {
    flights?: FlightSegment[];
    layovers?: Layover[];
    totalDuration: number; // In minutes
    price: number;
    type: string; // e.g., "Round trip"
    // carbonEmissions?: CarbonEmissions;
    departureToken?: string;
    bookingToken?: string;
    // Note: Add airline_logo and extensions if needed.
} 