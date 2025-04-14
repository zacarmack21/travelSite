import { BookingRequest } from './booking-request.model';
import { LocalPrice } from './local-price.model';

export interface BookingTogether {
  bookWith: string | null; // e.g., "JAL", "Kiwi.com"
  airlineLogos: string[] | null; // URLs
  marketedAs: string[] | null; // Flight numbers
  price: number;
  localPrices: LocalPrice[] | null;
  baggagePrices: string[] | null; // Note: Duplicates BaggagePrices structure slightly
  bookingRequest: BookingRequest | null;
} 