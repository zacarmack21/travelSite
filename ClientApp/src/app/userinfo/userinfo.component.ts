import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FlightSearchService } from '../services/flight-search.service';
import { FlightSearchRequest } from '../models/flight-search-request.model';
import { FlightSearchResponse } from '../models/flight-search-response.model';
import { BookingOptionsRequest } from '../models/booking-options-request.model';
import { BookingApiResponse } from '../models/booking-api-response.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-userinfo',
  templateUrl: './userinfo.component.html',
  styleUrls: ['./userinfo.component.css']
})
export class UserinfoComponent implements OnInit {
  @ViewChild('bookingOptionsSection') bookingOptionsSection!: ElementRef;

  flightSearchForm!: FormGroup;
  flightResults: FlightSearchResponse | null = null;
  returnFlightResults: FlightSearchResponse | null = null;
  selectedDepartureFlightToken: string | null = null;
  searchError: string | null = null;
  returnSearchError: string | null = null;
  isLoading: boolean = false;
  isLoadingReturn: boolean = false;
  isLoadingBookingOptions: boolean = false;
  bookingOptions: BookingApiResponse | null = null;
  bookingOptionsError: string | null = null;

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
    this.returnFlightResults = null;
    this.selectedDepartureFlightToken = null;
    this.searchError = null;
    this.returnSearchError = null;
    this.isLoading = true;
    this.bookingOptions = null;
    this.bookingOptionsError = null;

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

    console.log('Submitting search request:', request);

    this.flightSearchService.searchFlights(request).subscribe({
      next: (response: FlightSearchResponse) => {
        console.log('Search successful:', response);
        this.flightResults = response;
        this.isLoading = false;
        if (this.flightSearchForm.value.type === 2) {
          this.selectedDepartureFlightToken = 'one-way';
        }
      },
      error: (error: Error) => {
        console.error('Search failed:', error);
        this.searchError = error.message;
        this.isLoading = false;
      }
    });
  }

  selectDepartureFlight(token: string): void {
    if (!this.flightResults || !this.flightSearchForm.value.returnDate) {
      console.error('Cannot select departure without initial results or return date.');
      this.returnSearchError = 'Initial search results or return date missing.';
      return;
    }

    this.selectedDepartureFlightToken = token;
    this.returnFlightResults = null;
    this.returnSearchError = null;
    this.isLoadingReturn = true;

    const originalRequest = this.flightSearchForm.value;
    const returnRequest: FlightSearchRequest = {
      departureId: originalRequest.departureId,
      arrivalId: originalRequest.arrivalId,
      outboundDate: originalRequest.outboundDate,
      returnDate: originalRequest.returnDate,
      type: 1,
      adults: originalRequest.adults,
      children: originalRequest.children || undefined,
      infantsInSeat: originalRequest.infantsInSeat || undefined,
      infantsOnLap: originalRequest.infantsOnLap || undefined,
      departureToken: token
    };

    console.log('Submitting return flight search request with token:', returnRequest);

    this.flightSearchService.searchFlights(returnRequest).subscribe({
      next: (response: FlightSearchResponse) => {
        console.log('Return search successful:', response);
        this.returnFlightResults = response;
        this.isLoadingReturn = false;
      },
      error: (error: Error) => {
        console.error('Return search failed:', error);
        this.returnSearchError = error.message;
        this.isLoadingReturn = false;
      }
    });
  }

  fetchBookingOptions(bookingToken: string): void {
    if (!this.flightSearchForm.value) {
      console.error('Original search request details are missing.');
      this.bookingOptionsError = 'Original search details are missing.';
      return;
    }
    this.isLoadingBookingOptions = true;
    this.bookingOptions = null;
    this.bookingOptionsError = null;

    const originalRequest: FlightSearchRequest = {
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

    console.log('Constructed originalRequest:', originalRequest);

    const request: BookingOptionsRequest = {
      bookingToken: bookingToken,
      originalSearchRequest: originalRequest
    };

    console.log('Fetching booking options with request:', request);

    this.flightSearchService.getBookingOptions(request).subscribe({
      next: (response: BookingApiResponse) => {
        console.log('Booking options fetched successfully:', response);
        this.bookingOptions = response;
        this.isLoadingBookingOptions = false;
        // Use setTimeout to ensure the element is rendered before scrolling
        setTimeout(() => {
          console.log('Attempting to scroll...');
          if (this.bookingOptionsSection?.nativeElement) {
            console.log('Found bookingOptionsSection element:', this.bookingOptionsSection.nativeElement);
            this.bookingOptionsSection.nativeElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
          } else {
            console.error('bookingOptionsSection element not found when attempting to scroll.');
          }
        }, 0);
      },
      error: (error: Error) => {
        console.error('Fetching booking options failed:', error);
        this.bookingOptionsError = error.message;
        this.isLoadingBookingOptions = false;
      }
    });
  }
}
