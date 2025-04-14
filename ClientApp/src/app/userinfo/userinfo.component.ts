import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-userinfo',
  templateUrl: './userinfo.component.html',
  styleUrls: ['./userinfo.component.css']
})
export class UserinfoComponent implements OnInit {
  flightSearchForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.flightSearchForm = this.fb.group({
      departureId: ['', Validators.required],
      arrivalId: ['', Validators.required],
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
    if (this.flightSearchForm.valid) {
      console.log('Form Submitted!', this.flightSearchForm.value);
    } else {
      console.log('Form is invalid');
      this.flightSearchForm.markAllAsTouched();
    }
  }
}
