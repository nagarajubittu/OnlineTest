import { Component, OnInit } from '@angular/core';

import { AccountService } from '../shared/account.service';
import { Account } from '../shared/account.model';

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.css']
})
export class AccountListComponent implements OnInit {

  constructor(public service: AccountService) { }

  ngOnInit(): void {
    this.service.refreshList();
  }

  populateForm(model: Account): void {
    this.service.editAccount = true;
    this.service.formData = Object.assign({}, model);
  }

  onDelete(accountId: number): void {
    if (confirm('Are you sure to delete this record ?')) {
      this.service.deleteAccount(accountId)
        .subscribe(res => {
          this.service.refreshList();
        },
        err => { console.log(err); });
    }
  }

}
