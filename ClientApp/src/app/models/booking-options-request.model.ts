import { FlightSearchRequest } from './flight-search-request.model';

export interface BookingOptionsRequest {
  bookingToken: string | null;
  originalSearchRequest: FlightSearchRequest | null;
} 