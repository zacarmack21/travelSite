import { FlightSegment } from './flight-segment.model';
import { Layover } from './layover.model';
// Assuming CarbonEmissions will be defined elsewhere or is not strictly needed yet.
// import { CarbonEmissions } from './carbon-emissions.model';

export interface FlightOption {
    flights?: FlightSegment[];
    layovers?: Layover[];
    total_duration: number; // In minutes
    price: number;
    type: string; // e.g., "Round trip"
    // carbon_emissions?: CarbonEmissions;
    departure_token?: string;
    booking_token?: string;
    // Note: Add airline_logo and extensions if needed, likely snake_case.
} 