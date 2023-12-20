import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from "../Environments/environment";

@Injectable({
  providedIn: 'root'
})
export class MailService {
  constructor(private http: HttpClient) {
  }

  sendEmail(to: string, subject: string, body: string) {
    const emailData = {to, subject, body};
    return this.http.post(`${environment.baseUrl}/SendMail`, emailData);
  }
}
