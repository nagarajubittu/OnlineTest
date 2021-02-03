import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { AccountService } from '../shared/account.service';
import { MeterReadingService } from '../shared/meter-reading.service';
import { MeterReading } from '../shared/meter-reading.model';

@Component({
  selector: 'app-meter-reading-list',
  templateUrl: './meter-reading-list.component.html',
  styleUrls: ['./meter-reading-list.component.css']
})
export class MeterReadingListComponent implements OnInit {

  fileToUpload: File | null = null;

  constructor(public service: MeterReadingService, public accountService: AccountService) { }

  ngOnInit(): void {
    this.service.refreshList();
    this.accountService.refreshList();
  }

  onDelete(accountId: number): void {
    if (confirm('Are you sure to delete this record ?')) {
      this.service.deleteMeterReading(accountId)
        .subscribe(res => {
          this.service.refreshList();
        },
        err => { console.log(err); });
    }
  }

  handleFileInput(e: any): void {
    const ele = e.target as HTMLInputElement;
    if (ele != null && ele.files && ele.files.length > 0) {
      this.fileToUpload = ele.files[0];
    }
  }

  onUpload(form: NgForm): void {
    if (this.fileToUpload != null && this.fileToUpload.name.split('.').pop() === 'csv') {
      this.service.postFile(this.fileToUpload).subscribe(
        res => {
          form.reset();
          this.service.refreshList();
        },
        err => { console.log(err); }
      );
    } else {
      alert('Invalid file selected. Only csv is allowed');
    }
  }

  onOptionsSelected(value: string): void {
    this.service.filterAccountId = Number(value);
    // this.service.formData.accountId = this.service.filterAccountId;
    this.service.refreshList();
  }

}
