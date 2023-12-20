export class User {
  id: number;
  username: string;
  email: string;
  passwordHash: string[];
  passwordSalt: string[];

  constructor(
    id: number,
    username: string,
    email: string,
    passwordHash: string[],
    passwordSalt: string[]
  ) {
    this.id = id;
    this.username = username;
    this.email = email;
    this.passwordHash = passwordHash;
    this.passwordSalt = passwordSalt;
  }
}
