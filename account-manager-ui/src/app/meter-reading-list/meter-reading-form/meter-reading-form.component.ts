import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { PostMeterReading } from '../../shared/meter-reading.model';
import { MeterReadingService } from '../../shared/meter-reading.service';

@Component({
  selector: 'app-meter-reading-form',
  templateUrl: './meter-reading-form.component.html',
  styles: [
  ]
})
export class MeterReadingFormComponent implements OnInit {

  constructor(public service: MeterReadingService) { }

  ngOnInit(): void {
  }

  resetForm(form: NgForm): void {
    form.form.reset();
    this.service.formData = new PostMeterReading();
  }

  onSubmit(form: NgForm): void {
    this.insertRecord(form);
  }

  insertRecord(form: NgForm): void {
    this.service.postMeterReading().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
      },
      err => { console.log(err); }
    );
  }

}
