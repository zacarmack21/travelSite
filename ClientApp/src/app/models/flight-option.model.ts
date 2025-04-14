import { FlightSegment } from './flight-segment.model';
import { Layover } from './layover.model';
// Assuming CarbonEmissions will be defined elsewhere or is not strictly needed yet.
// import { CarbonEmissions } from './carbon-emissions.model';

// Define CarbonEmissions inline or import if used elsewhere
interface CarbonEmissions {
  this_flight: number;
  typical_for_this_route?: number;
  difference_percent?: number;
}

export interface FlightOption {
    flights: FlightSegment[]; // Should probably not be optional if it's a core part
    layovers?: Layover[];
    total_duration: number; // In minutes
    carbon_emissions?: CarbonEmissions;
    price: number;
    type: string; // e.g., "Round trip"
    airline_logo?: string; // Add this based on sample
    extensions?: string[]; // Add this based on sample
    departure_token?: string;
    booking_token?: string;
    // Note: Add airline_logo and extensions if needed, likely snake_case.
} 