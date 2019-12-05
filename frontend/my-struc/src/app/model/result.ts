export class Result<TSuccess, TError> {
  constructor(
    public success: TSuccess|null,
    public error: TError|null
  ) {}
}
