import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { FlightSearchRequest } from '../models/flight-search-request.model';
import { FlightSearchResponse } from '../models/flight-search-response.model';
import { BookingOptionsRequest } from '../models/booking-options-request.model';
import { BookingApiResponse } from '../models/booking-api-response.model';

@Injectable({
  providedIn: 'root'
})
export class FlightSearchService {
  private searchApiUrl = '/api/Flights/search';
  private bookingApiUrl = '/api/Flights/booking-options';

  constructor(private http: HttpClient) { }

  searchFlights(request: FlightSearchRequest): Observable<FlightSearchResponse> {
    return this.http.post<FlightSearchResponse>(this.searchApiUrl, request)
      .pipe(
        catchError(this.handleError)
      );
  }

  getBookingOptions(request: BookingOptionsRequest): Observable<BookingApiResponse> {
    return this.http.post<BookingApiResponse>(this.bookingApiUrl, request)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `An error occurred: ${error.error.message}`;
    } else {
      // Backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
       errorMessage = `Server returned code ${error.status}, error message is: ${error.message}`;
       // Check if the error object has a more specific message
       if (error.error && typeof error.error === 'string') {
         errorMessage = error.error; // Use the error message from the backend response body
       } else if (error.error && error.error.title) {
         errorMessage = error.error.title; // Handle ASP.NET Core validation problem details
       }
    }
    console.error(error);
    // Return an observable with a user-facing error message.
    // The backend might send a detailed error in error.error
    return throwError(() => new Error(errorMessage));
  }
}
