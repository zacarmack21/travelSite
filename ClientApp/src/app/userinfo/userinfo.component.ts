import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FlightSearchService } from '../services/flight-search.service';
import { FlightSearchRequest } from '../models/flight-search-request.model';
import { FlightSearchResponse } from '../models/flight-search-response.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-userinfo',
  templateUrl: './userinfo.component.html',
  styleUrls: ['./userinfo.component.css']
})
export class UserinfoComponent implements OnInit {
  flightSearchForm!: FormGroup;
  flightResults: FlightSearchResponse | null = null;
  finalFlightPairResponse: FlightSearchResponse | null = null;
  selectedDepartureFlightToken: string | null = null;
  searchError: string | null = null;
  isLoading: boolean = false;
  isProcessingSelection: boolean = false;
  selectionError: string | null = null;

  constructor(
    private fb: FormBuilder,
    private flightSearchService: FlightSearchService
  ) {}

  ngOnInit(): void {
    this.flightSearchForm = this.fb.group({
      departureId: ['AUS', Validators.required],
      arrivalId: ['DAL', Validators.required],
      outboundDate: ['', Validators.required],
      returnDate: [''],
      type: [1, Validators.required],
      adults: [1, [Validators.required, Validators.min(1)]],
      children: [0, Validators.min(0)],
      infantsInSeat: [0, Validators.min(0)],
      infantsOnLap: [0, Validators.min(0)]
    });
  }

  onSubmit(): void {
    this.flightResults = null;
    this.finalFlightPairResponse = null;
    this.selectedDepartureFlightToken = null;
    this.searchError = null;
    this.selectionError = null;
    this.isLoading = true;
    this.isProcessingSelection = false;

    if (this.flightSearchForm.invalid) {
      console.log('Form is invalid');
      this.flightSearchForm.markAllAsTouched();
      this.isLoading = false;
      return;
    }

    const request: FlightSearchRequest = {
      departureId: this.flightSearchForm.value.departureId,
      arrivalId: this.flightSearchForm.value.arrivalId,
      outboundDate: this.flightSearchForm.value.outboundDate,
      returnDate: this.flightSearchForm.value.type === 1 ? this.flightSearchForm.value.returnDate : undefined,
      type: this.flightSearchForm.value.type,
      adults: this.flightSearchForm.value.adults,
      children: this.flightSearchForm.value.children || undefined,
      infantsInSeat: this.flightSearchForm.value.infantsInSeat || undefined,
      infantsOnLap: this.flightSearchForm.value.infantsOnLap || undefined,
    };

    console.log('Submitting initial search request:', request);

    this.flightSearchService.searchFlights(request).subscribe({
      next: (response: FlightSearchResponse) => {
        console.log('Initial search successful:', response);
        if (response.error) {
           console.error('API Error during initial search:', response.error);
           this.searchError = `API Error: ${response.error}`;
           this.flightResults = null;
        } else if (!(response.best_flights?.length || response.other_flights?.length) && this.flightSearchForm.value.type === 1) {
            this.searchError = 'No departing flights found for the selected criteria.';
             this.flightResults = null;
        } else if (this.flightSearchForm.value.type === 2 && !(response.best_flights?.length || response.other_flights?.length)){
            this.searchError = 'No flights found for the selected criteria.';
             this.flightResults = null;
        } else {
          this.flightResults = response;
          if (this.flightSearchForm.value.type === 2) {
             this.selectedDepartureFlightToken = 'one-way';
          }
        }
        this.isLoading = false;
      },
      error: (error: Error) => {
        console.error('Initial search failed:', error);
        this.searchError = error.message;
        this.isLoading = false;
      }
    });
  }

  selectDepartureFlight(token: string): void {
    if (!this.flightResults || (this.flightSearchForm.value.type === 1 && !this.flightSearchForm.value.returnDate)) {
      console.error('Cannot select departure without initial results or return date for round trip.');
      this.selectionError = 'Initial search results missing or return date not specified for round trip.';
      return;
    }

    this.selectedDepartureFlightToken = token;
    this.finalFlightPairResponse = null;
    this.selectionError = null;
    this.isProcessingSelection = true;

    const originalRequestValues = this.flightSearchForm.value;
    const selectionRequest: FlightSearchRequest = {
      departureId: originalRequestValues.departureId,
      arrivalId: originalRequestValues.arrivalId,
      outboundDate: originalRequestValues.outboundDate,
      returnDate: originalRequestValues.returnDate,
      type: originalRequestValues.type,
      adults: originalRequestValues.adults,
      children: originalRequestValues.children || undefined,
      infantsInSeat: originalRequestValues.infantsInSeat || undefined,
      infantsOnLap: originalRequestValues.infantsOnLap || undefined,
      departure_token: token
    };

    console.log('Submitting flight selection request:', selectionRequest);

    this.flightSearchService.searchFlights(selectionRequest).subscribe({
      next: (response: FlightSearchResponse) => {
        console.log('Flight selection successful:', response);
         if (response.error) {
           console.error('API Error during flight selection:', response.error);
           this.selectionError = `API Error: ${response.error}`;
           this.finalFlightPairResponse = null;
        } else if (!(response.best_flights?.length || response.other_flights?.length)) {
             console.error('Incomplete response after selection (missing return flights):', response);
             this.selectionError = 'No return flights found for the selected departure.';
             this.finalFlightPairResponse = null;
        } else {
          this.finalFlightPairResponse = response;
        }
        this.isProcessingSelection = false;
      },
      error: (error: Error) => {
        console.error('Flight selection failed:', error);
        this.selectionError = error.message;
        this.isProcessingSelection = false;
      }
    });
  }

  initiateBooking(postData: string | undefined): void {
    if (!postData) {
        console.error('No booking post_data found for selected option.');
        return;
    }
    console.log('--- Initiate Booking --- ');
    console.log('Booking Request URL might be needed from booking_request.url');
    console.log('Booking Request POST data:', postData);
    alert('Booking initiated! Check console for POST data. (Actual booking API call not implemented)');
  }
}
