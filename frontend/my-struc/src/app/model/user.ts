export class User {
  public static readonly None = new User('', '');
  public static readonly Template = new User('', '');

  constructor(
      public id: string,
      public name: string
  ) {
  }
}
