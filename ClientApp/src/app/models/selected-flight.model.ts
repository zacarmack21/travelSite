import { FlightSegment } from './flight-segment.model';
import { Layover } from './layover.model';
import { CarbonEmissions } from './carbon-emissions.model'; // Assuming this exists or create it

export interface SelectedFlight {
    // Properties typically found in a flight option, adjust based on actual needs
    // Note: departure_token might not be present here, unlike in the initial FlightOption
    flights?: FlightSegment[];
    layovers?: Layover[];
    total_duration?: number;
    carbon_emissions?: CarbonEmissions;
    price?: number; // Use number for currency values in TS
    type?: string;
    airline_logo?: string;
    extensions?: string[];
    // Add any other specific properties from the 'selected_flights' structure if needed
} 