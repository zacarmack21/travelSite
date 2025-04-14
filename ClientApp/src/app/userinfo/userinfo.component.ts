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
  searchError: string | null = null;
  isLoading: boolean = false;

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
    this.searchError = null;
    this.isLoading = true;

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
      },
      error: (error: Error) => {
        console.error('Search failed:', error);
        this.searchError = error.message;
        this.isLoading = false;
      }
    });
  }
}
