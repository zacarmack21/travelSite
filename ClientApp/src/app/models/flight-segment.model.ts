import { AirportInfo } from './airport-info.model';

export interface FlightSegment {
    departure_airport: AirportInfo;
    arrival_airport: AirportInfo;
    duration: number; // In minutes
    airline: string;
    airline_logo: string; // URL
    flight_number: string;
    travel_class: string;
    legroom?: string; // e.g., "31 in"
    // Note: Add airplane, extensions, ticket_also_sold_by, overnight, etc. if needed, likely snake_case.
} 