import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-order',
  templateUrl: './add-order.component.html',
  styleUrls: ['./add-order.component.css']
})
export class AddOrderComponent implements OnInit {
  orderForm: FormGroup;

  constructor(private fb: FormBuilder, private http: HttpClient,@Inject('BASE_URL') private baseUrl: string, private router: Router) {

    this.orderForm = this.fb.group({
      numeroPedido: ['', Validators.required],
      descricao: ['', Validators.required],
      valor: ['', [Validators.required, Validators.min(0)]],
      endereco: this.fb.group({
        cep: ['', Validators.required],
        rua: ['', Validators.required],
        numero: ['', Validators.required],
        bairro: ['', Validators.required],
        cidade: ['', Validators.required],
        estado: ['', Validators.required]
      })
    });
  }

  ngOnInit(): void {
  }

  submitForm() {
    if (this.orderForm.valid) {
      console.log('Pedido salvo:', this.orderForm.value);
      var endereco = (this.orderForm.get('endereco') as FormGroup).controls;
      var request = {
        number : this.orderForm.controls['numeroPedido'].value,
        description : this.orderForm.controls['descricao'].value,
        value: this.orderForm.controls['valor'].value,
        deliveryAddress: {
          code : endereco['cep'].value,
          street: endereco['rua'].value,
          number: endereco['numero'].value,
          district: endereco['bairro'].value,
          city: endereco['cidade'].value,
          state: endereco['estado'].value
        }
      };
      this.http.post<any>(`${this.baseUrl}api/order`, request).subscribe(data => {
        this.router.navigate(['orders']);
      })
    } else {
      console.log('Formulário inválido');
    }
  }


  loadAddress(){
    var grp = (this.orderForm.get('endereco') as FormGroup).controls;
    var cep = grp['cep'].value;
    if(cep){
      this.http.get<any>(`${this.baseUrl}api/address/cep/${cep}`).subscribe(data => {
        if(data) {
          grp['rua'].setValue(data.street);
          grp['bairro'].setValue(data.neighborhood);
          grp['cidade'].setValue(data.city);
          grp['estado'].setValue(data.state);
        }
      }, err => {
        (this.orderForm.get('endereco') as FormGroup).reset();
      });
    }

  }

}
