import { AirportInfo } from './airport-info.model';

export interface FlightSegment {
    departureAirport: AirportInfo;
    arrivalAirport: AirportInfo;
    duration: number; // In minutes
    airline: string;
    airlineLogo: string; // URL
    flightNumber: string;
    travelClass: string;
    legroom?: string; // e.g., "31 in"
    // Note: Add airplane, extensions, ticket_also_sold_by, overnight, etc. if needed.
} 