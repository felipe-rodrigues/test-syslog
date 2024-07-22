import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Alert } from '../shared/models/alert';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  userForm: FormGroup;
  alerts: Alert[];

  constructor(
    private fb: FormBuilder,private http: HttpClient,@Inject('BASE_URL') private baseUrl: string, private router: Router
  ) {
    this.userForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });
    this.alerts = [];
  }


  ngOnInit() { }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  submitForm() {
    if (this.userForm.valid) {
      var request = {};
      request = Object.assign(request,this.userForm.value);
      debugger;
      this.http.post(`${this.baseUrl}api/user`, request).subscribe(data => {
        console.log('Usuario criado');
        this.userForm.reset();
        this.alerts.push( {
          type : "success",
          message : 'Usuário cadastrado com sucesso'
        } as Alert)
      });
      return true;
    } else {
      return false;
    }
  }

  close(alert: Alert) {
    this.alerts.splice(this.alerts.indexOf(alert), 1);
  }

}
