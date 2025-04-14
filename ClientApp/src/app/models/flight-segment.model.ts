import { AirportInfo } from './airport-info.model';

export interface FlightSegment {
    departure_airport?: AirportInfo;
    arrival_airport?: AirportInfo;
    duration: number; // In minutes
    airplane?: string; // Added based on sample
    airline: string;
    airline_logo: string; // URL
    travel_class: string;
    flight_number: string;
    legroom?: string; // e.g., "31 in"
    extensions?: string[]; // Added based on sample
    // Note: Add ticket_also_sold_by, overnight, etc. if needed, likely snake_case.
} 