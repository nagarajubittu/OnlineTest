import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Account } from '../../shared/account.model';
import { AccountService } from '../../shared/account.service';

@Component({
  selector: 'app-account-create-form',
  templateUrl: './account-create-form.component.html',
  styles: [
  ]
})
export class AccountCreateFormComponent implements OnInit {

  constructor(public service: AccountService) { }

  ngOnInit(): void {
  }

  resetForm(form: NgForm): void {
    form.form.reset();
    this.service.editAccount = false;
    this.service.formData = new Account();
  }

  onSubmit(form: NgForm): void {
    if (this.service.editAccount) {
      this.updateRecord(form);
    } else {
      this.insertRecord(form);
    }
  }

  insertRecord(form: NgForm): void {
    this.service.postAccount().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
      },
      err => { console.log(err); }
    );
  }

  updateRecord(form: NgForm): void {
    this.service.putAccount().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
      },
      err => {
        console.log(err);
      }
    );
  }

}
